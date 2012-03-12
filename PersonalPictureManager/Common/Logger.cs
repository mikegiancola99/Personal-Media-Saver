using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PersonalPictureManagerService.Common
{
    public class Logger
    {
        private String m_logfile = "log_UI.txt";
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
            using (StreamWriter w = File.AppendText(m_logPath))
            {
                DateTime curTime = DateTime.Now;
                w.Write(curTime.ToString("s"));
                w.Write(" " + m_methodName + " ");
                w.WriteLine(entry);
                w.Close();
            }
        }
    }
}
