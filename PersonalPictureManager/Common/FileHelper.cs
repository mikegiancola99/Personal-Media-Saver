using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Globalization;


namespace PictureUploader
{
    public class FileHelper
    {
        PixDBInterface m_pixDb = new PixDBInterface();
        string m_serverName ;
        string m_user ;
        string m_pass ;
        string m_contentType;

        List<FileSystemWatcher> m_watchers = new List<FileSystemWatcher>();

        public FileHelper(string server, string user, string pass, string inContentType)
        {
            m_serverName = server;
            m_user = user;
            m_pass = pass;
            m_contentType = inContentType;
        }

        public void WatchDirs(List<string> inDirList)
        {
            foreach (FileSystemWatcher watcher in m_watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            m_watchers.Clear();


            foreach (string dir in inDirList)
            {
                List<string> jpgList = GetFilesRecursive(dir,  "JPG");
                List<string> gifList = GetFilesRecursive(dir,  "gif");
                List<string> jpegList = GetFilesRecursive(dir, "jpeg");
                List<string> pngList = GetFilesRecursive(dir,  "png");
                List<string> bmpList = GetFilesRecursive(dir,  "bmp");

                AddFiles(jpgList);
                AddFiles(gifList);
                AddFiles(jpegList);
                AddFiles(pngList);
                AddFiles(bmpList);

                AddDirListener(dir);
            }
        }

        private List<string> GetFilesRecursive(string initDir, string extensions)
        {
            List<string> result = new List<string>();
            Stack<string> stack = new Stack<string>();
            stack.Push(initDir);
            while (stack.Count > 0)
            {
                string dir = stack.Pop();
                try
                {
                    result.AddRange(Directory.GetFiles(dir, "*." + extensions));
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                }
            }
            return result;
        }

        private void AddDirListener(string inDir)
        {
            FileSystemWatcher tmpWatcher = new System.IO.FileSystemWatcher();
            tmpWatcher.Path = inDir;
            tmpWatcher.IncludeSubdirectories = true;
            tmpWatcher.Filter = "*";
            tmpWatcher.NotifyFilter = NotifyFilters.LastWrite |
                                     NotifyFilters.CreationTime |
                                     NotifyFilters.FileName;

            tmpWatcher.Created += new FileSystemEventHandler(OnChanged);
            tmpWatcher.EnableRaisingEvents = true;
            m_watchers.Add(tmpWatcher);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string full = e.FullPath;
            full = full.ToLower();
            if (full.Contains(".jpg") ||
                full.Contains("jpeg") ||
                full.Contains("gif") ||
                full.Contains("png") ||
                full.Contains("bmp"))
            {
                AddFile(full);
            }
        }

        private void AddFiles(List<string> files)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            foreach (string p in files)
            {
                AddFile(p);
            }
        }

        public void AddFile(string inFile)
        {
            string md5 = GetMD5HashFromFile(inFile);
            if (!m_pixDb.CheckIfSent(md5))
            {
                if (!SendFileToServer(inFile, md5))
                    AddFileToQueueDatabase(inFile);
            }
        }

        private bool SendFileToServer(string inFile, string md5)
        {
            bool success = false;
            try
            {
                DateTime whenCreated = File.GetCreationTime(inFile);
                FileInfo fileData = new FileInfo(inFile);
                string subDir = whenCreated.ToShortDateString();
                subDir = subDir.Replace("/", "_");

                var charArr = new Char[] { '_' };
                string[] dirNames = subDir.Split(charArr);
                subDir = dirNames[2] + "\\" + dirNames[0] + "\\" + dirNames[1];

                string baseDir;
                if (!string.IsNullOrEmpty(m_user) && !string.IsNullOrEmpty(m_pass))
                    baseDir = @"\\" + m_user + ":" + m_pass + "@" + m_serverName + "\\" + m_contentType + "\\" + subDir;
                else
                    baseDir = @"\\" + m_serverName + "\\" + m_contentType + "\\" + subDir;

                Directory.CreateDirectory(baseDir);

                string sTarget = baseDir + "\\" + fileData.Name;

                if (!File.Exists(sTarget))
                {
                    File.Copy(inFile, sTarget);
                    m_pixDb.AddSentPicture(fileData.Name, inFile, md5, Convert.ToString(DateTime.Now));
                }
                else
                {
                    string serverMd5 = GetMD5HashFromFile(sTarget);
                    if (serverMd5.CompareTo(md5) != 0)
                    {
                        int count = 0;
                        string newDirName = "";
                        while (File.Exists(sTarget))
                        {
                            newDirName = baseDir + "\\" + Convert.ToString(count);
                            sTarget = newDirName + "\\" + fileData.Name;
                            count++;
                        }
                        Directory.CreateDirectory(newDirName);
                        File.Copy(inFile, sTarget);
                    }
                    else
                    {
                        m_pixDb.AddSentPicture(fileData.Name, inFile, md5, Convert.ToString(DateTime.Now));
                    }
                    success = true;
                }
            }
            catch (Exception)
            { }
            return success;
        }

        private void AddFileToQueueDatabase(string inFile)
        {
            PixDBInterface dbInter = new PixDBInterface();
            if (!dbInter.CheckIfInPixQueue(inFile))
                dbInter.AddFileToPixQueue(inFile);
        }

        private string GetMD5HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        #region COMExports
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr existingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr duplicateTokenHandle);

        // logon types
        const int LOGON32_LOGON_INTERACTIVE = 2;
        const int LOGON32_LOGON_NETWORK = 3;
        const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        // logon providers
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_PROVIDER_WINNT50 = 3;
        const int LOGON32_PROVIDER_WINNT40 = 2;
        const int LOGON32_PROVIDER_WINNT35 = 1;
        #endregion
    }


}
