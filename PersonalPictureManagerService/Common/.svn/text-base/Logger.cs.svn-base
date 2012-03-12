using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace PersonalPictureManagerService.Common
{
    public class Logger
    {
        private String m_logfile = "log.txt";
        private String m_logPath;
        private String m_methodName;
        public Logger(String inMethodName)
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDataDir += "\\PersonalPictureManager\\";
            Directory.CreateDirectory(appDataDir);
            m_logPath = appDataDir + m_logfile;

            m_methodName = inMethodName;
        }

        public void WriteMessage(String entry)
        {
            // in case we try and log in the event handlers. It's possible
            // multiple timers will pop at the same time
            bool requestInitialOwnership = true;
            bool mutexWasCreated;
            Mutex m = new Mutex(requestInitialOwnership, "PersonalPictureMgrMutex", out mutexWasCreated);
            if (!(requestInitialOwnership && mutexWasCreated))
            {
                m.WaitOne();
            }

            using (StreamWriter w = File.AppendText(m_logPath))
            {
                try
                {
                    DateTime curTime = DateTime.Now;
                    w.Write(curTime.ToString("s"));
                    w.Write(" " + m_methodName + " ");
                    w.WriteLine(entry);
                }
                catch (Exception)
                { }
                w.Close();
            }

            m.ReleaseMutex();
        }
    }
}
