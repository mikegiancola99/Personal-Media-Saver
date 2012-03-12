using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Globalization;
using System.Windows.Media.Imaging;

using PersonalPictureManagerService.Common;

namespace PictureUploader
{
    public class PixFileHelper: FileHelper
    {
        List<FileSystemWatcher> m_watchers = new List<FileSystemWatcher>();

        public PixFileHelper(string server, string user, string pass)
        {
            m_serverName = server;
            m_user = user;
            m_pass = pass;
        }

        public override void WatchDirs(List<string> inDirList)
        {
            foreach (string dir in inDirList)
            {
                List<string> jpgList = GetFilesRecursive(dir, "JPG");
                List<string> gifList = GetFilesRecursive(dir, "gif");
                List<string> jpegList = GetFilesRecursive(dir, "jpeg");
                List<string> pngList = GetFilesRecursive(dir, "png");
                List<string> bmpList = GetFilesRecursive(dir, "bmp");

                AddFiles(jpgList);
                AddFiles(gifList);
                AddFiles(jpegList);
                AddFiles(pngList);
                AddFiles(bmpList);
            }
        }

        public override void AddDirListener(List<string> inDirList)
        {
            foreach (FileSystemWatcher watcher in m_watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            m_watchers.Clear();


            foreach (string dir in inDirList)
            {
                AddDirListener(dir);
            }
        }


        public override void OnChanged(object sender, FileSystemEventArgs e)
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

        public override FileResults AddFile(string inFile)
        {
            FileResults result = FileResults.EFail;
            Logger log = new Logger("PixFileHelper::AddFile");
            try
            {
                if (File.Exists(inFile))
                {
                    string md5 = GetMD5HashFromFile(inFile);
                    if (!m_pixDb.CheckIfSent(md5))
                    {
                        //log.WriteMessage("attempting to send: " + inFile);

                        if (!SendFileToServer(inFile, md5, GenerateServerDir(inFile)))
                        {
                            //log.WriteMessage("sending file failed, queuing");
                            AddFileToQueueDatabase(inFile);
                            result = FileResults.EUnableToSend;
                        }
                        else
                        {
                            result = FileResults.ESuccess;
                        }
                    }
                    else
                    {
                        result = FileResults.EAlreadySent;
                    }
                }
            }
            catch (Exception ee)
            {
                log.WriteMessage("tossed - message: " + ee.Message);
                log.WriteMessage("tossed - stack: " + ee.StackTrace);
            }
            return result;
        }

        private string GenerateServerDir(string inFile)
        {
            DateTime whenCreated = File.GetCreationTime(inFile);
            String takenDate = GetDateTaken_FromFile(inFile);
            
            if (takenDate != null)
                whenCreated = Convert.ToDateTime(takenDate);

            string subDir = whenCreated.ToShortDateString();


            subDir = subDir.Replace("/", "_");

            var charArr = new Char[] { '_' };
            string[] dirNames = subDir.Split(charArr);
            subDir = dirNames[2] + "\\" + GetMonthPrettyName(Convert.ToInt32(dirNames[0])) + "\\" + dirNames[1];

            string baseDir;
            if (!string.IsNullOrEmpty(m_user) && !string.IsNullOrEmpty(m_pass))
                baseDir = @"\\" + m_user + ":" + m_pass + "@" + m_serverName + "\\Pictures\\" + subDir;
            else
                baseDir = @"\\" + m_serverName + "\\Pictures\\" + subDir;
            return baseDir;

        }

        private string GetDateTaken_FromFile(string inFile)
        {
            string date = null;
            try
            {
                FileStream fs = new FileStream(inFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                BitmapSource img = BitmapFrame.Create(fs);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;
                date = md.DateTaken;
                fs.Close();
            }
            catch (Exception)
            { }
            return date;
        }
    }
}
