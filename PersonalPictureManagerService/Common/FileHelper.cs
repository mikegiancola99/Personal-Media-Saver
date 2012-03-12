using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Threading;

using PersonalPictureManagerService.Common;

namespace PictureUploader
{
    public class FileHelper
    {
        protected PixDBInterface m_pixDb = new PixDBInterface();
        protected string m_serverName ;
        protected string m_user;
        protected string m_pass;

        List<FileSystemWatcher> m_watchers = new List<FileSystemWatcher>();

        public enum FileResults
        {
            EFail = 0,
            ESuccess,
            EAlreadySent,
            EUnableToSend
        };

        public FileHelper()
        { }

        public virtual void WatchDirs(List<string> inDirList)
        {}

        public virtual void AddDirListener(List<string> inDirList) { }

        protected List<string> GetFilesRecursive(string initDir, string extensions)
        {
            Logger log = new Logger("FileHelper::GetFilesRecursive");
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
                catch (Exception e)
                {
                    log.WriteMessage("tossed message: " + e.Message);
                    log.WriteMessage("stack: " + e.StackTrace);
                }
            }
            return result;
        }

        protected void AddDirListener(string inDir)
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

        public virtual void OnChanged(object sender, FileSystemEventArgs e)
        { }
        
        protected void AddFiles(List<string> files)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
            int count = 0;
            foreach (string p in files)
            {
                count++;
                if (count >= 10)
                {
                    //Thread.Sleep(5000);
                    count = 0;
                }
                AddFile(p);
            }
        }

        public virtual FileResults AddFile(string inFile)
        { return FileResults.EFail; }

        protected string GetMonthPrettyName(int inMonthNum)
        {
            string retVal = "January";
            switch (inMonthNum)
            {
                case 1:
                    retVal = "January";
                    break;

                case 2:
                    retVal = "February";
                    break;
                case 3:
                    retVal = "March";
                    break;
                case 4:
                    retVal = "April";
                    break;
                case 5:
                    retVal = "May";
                    break;
                case 6:
                    retVal = "June";
                    break;
                case 7:
                    retVal = "July";
                    break;
                case 8:
                    retVal = "August";
                    break;
                case 9:
                    retVal = "September";
                    break;
                case 10:
                    retVal = "October";
                    break;
                case 11:
                    retVal = "November";
                    break;
                case 12:
                    retVal = "December";
                    break;

            }
            return retVal;
        }

        protected bool SendFileToServer(string inFile, string md5, string dir)
        {
            bool success = false;
            Logger log = new Logger("FileHelper::SendFileToServer");
            try
            {
                FileInfo fileData = new FileInfo(inFile);

                if (!Directory.Exists(dir))
                {
                    log.WriteMessage("attempting to create directory: " + dir);
                    Directory.CreateDirectory(dir);
                }

                string sTarget = dir + "\\" + fileData.Name;

                if (!File.Exists(sTarget))
                {
                    log.WriteMessage("attempting to send src: " + inFile + " dest: " + sTarget);
                    File.Copy(inFile, sTarget);
                    m_pixDb.AddSentPicture(fileData.Name, inFile, md5, Convert.ToString(DateTime.Now));
                    success = true;
                }
                else
                {
                    if (!FileEquals(inFile, sTarget))
                    {
                        log.WriteMessage("Src File: " + inFile + " is different from: " + sTarget);
                        int count = 0;
                        string newDirName = "";
                        while (File.Exists(sTarget))
                        {
                            newDirName = dir + "\\" + Convert.ToString(count);
                            sTarget = newDirName + "\\" + fileData.Name;
                            count++;
                        }
                        Directory.CreateDirectory(newDirName);
                        File.Copy(inFile, sTarget);
                    }
                    else
                    {
                        log.WriteMessage("file is identical: " + inFile);
                    }
                    m_pixDb.AddSentPicture(fileData.Name, inFile, md5, Convert.ToString(DateTime.Now));
                    success = true;
                }
            }
            catch (Exception e)
            {
                log.WriteMessage("exception: " + e.Message);
            }
            return success;
        }

        protected void AddFileToQueueDatabase(string inFile)
        {
            PixDBInterface dbInter = new PixDBInterface();
            if (!dbInter.CheckIfInPixQueue(inFile))
                dbInter.AddFileToPixQueue(inFile);
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            //FileStream file = new FileStream(fileName, FileMode.Open);
            byte[] fileData = File.ReadAllBytes(fileName);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fileData);
            //file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private bool FileEquals(string fileName1, string fileName2)
        {
            // Check the file size and CRC equality here.. if they are equal...    
            using (var file1 = new FileStream(fileName1, FileMode.Open))
            using (var file2 = new FileStream(fileName2, FileMode.Open))
                return StreamEquals(file1, file2);
        }

        private bool StreamEquals(Stream stream1, Stream stream2)
        {
            const int bufferSize = 2048;
            byte[] buffer1 = new byte[bufferSize]; //buffer size
            byte[] buffer2 = new byte[bufferSize];
            while (true)
            {
                int count1 = stream1.Read(buffer1, 0, bufferSize);
                int count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                    return false;

                if (count1 == 0)
                    return true;

                // You might replace the following with an efficient "memcmp"
                if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
                    return false;
            }
        }

    }


}
