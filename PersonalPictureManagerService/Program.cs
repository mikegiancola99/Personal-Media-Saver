using System;
using System.Collections.Generic;
using System.Text;
using PictureUploader;
using System.Timers;
using System.Threading;
using System.IO;
using System.Diagnostics;
using PersonalPictureManagerService.Common;
using System.Threading;
using System.Reflection;

namespace PersonalPictureManagerService
{
    class Program
    {
        static private List<string> dirList;
        static private FileHelper m_fileHelper;
        static public Mutex m_lockingMutex;
        static System.Timers.Timer queueTimer = new System.Timers.Timer();
        static System.Timers.Timer configTimer = new System.Timers.Timer();
        static System.Timers.Timer aTimer = new System.Timers.Timer();

        static void Main(string[] args)
        {
            Logger log = new Logger("Program::main");

            if (args.Length > 0)
            {
                string cmd = args[0];
                if (cmd.CompareTo("shutdown") == 0)
                { 
                    PixDBInterface db = new PixDBInterface();
                    if (db.CheckIfDbSetup())
                    {
                        try
                        {
                            // just in case we crashed and the process id in the db is bad
                            string procId = db.ReadConfigValue("ServiceProcessId");
                            int nProcessID = Convert.ToInt32(procId);
                            Process otherProc = Process.GetProcessById(nProcessID);
                            otherProc.Kill();
                        }
                        catch (Exception)
                        { }
                    }
                    return;
                }
            }

            try
            {
                Mutex.OpenExisting("PersonalPictureManagerService");
                log.WriteMessage("Opened the existing mutex");
                return;
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                bool createdNew;
                m_lockingMutex = new Mutex(true, "PersonalPictureManagerService", out createdNew);
                log.WriteMessage("created the mutex");
            }

            try
            {
                PixDBInterface dbConn = new PixDBInterface();
                if (!dbConn.CheckIfDbSetup())
                {
                    log.WriteMessage("setting up database");
                    dbConn.CreateDatabase();
                    dbConn.SetupTables();
                }


                int nProcessID = Process.GetCurrentProcess().Id;
                dbConn.UpdateConfigValue("ServiceProcessId", Convert.ToString(nProcessID));

                String runningVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                dbConn.UpdateConfigValue("ServiceProcessVersion", runningVer);

                log.WriteMessage("starting");

                string runningFile = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                string newFile = ConfigManager.GetDataStoreDir() + ConfigManager.AppName;
                log.WriteMessage("newFile: " + newFile);

                if ((File.Exists(newFile)) && (runningFile.CompareTo(newFile) != 0))
                {
                    log.WriteMessage("running: " + newFile);
                    Process.Start(newFile);
                    System.Environment.Exit(0);
                }

                OnConfigTimedEvent(null, null);


                string server = dbConn.ReadConfigValue("ServerName");
                string serverUser = dbConn.ReadConfigValue("ServerLogin");
                string serverPass = dbConn.ReadConfigValue("ServerPass");

                dirList = dbConn.ReadDirsToWatch(0);

                m_fileHelper = new PixFileHelper(server, serverUser, serverPass);
                m_fileHelper.WatchDirs(dirList);

                // update config timer
                
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                // Set the Interval to 5 min
                aTimer.Interval = 300000;
                aTimer.Enabled = true;

                // check website for new rev timer
                
                configTimer.Elapsed += new ElapsedEventHandler(OnConfigTimedEvent);
                // Set the Interval to 30 min
                configTimer.Interval = 1800000;
                configTimer.Enabled = true;

                // check if anything in the queue

                queueTimer.Elapsed += new ElapsedEventHandler(OnQueueTimedEvent);
                // Set the Interval to 30 min
                queueTimer.Interval = 1800000;
                //queueTimer.Interval = 1000;
                queueTimer.Enabled = true;

                while (true)
                    Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText(@"error_log.txt", e.Message);
                System.IO.File.WriteAllText(@"error_log.txt", e.StackTrace);
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;

            Logger log = new Logger("Program::OnTimedEvent");
            try
            {
                PixDBInterface dbConn = new PixDBInterface();

                string configChanged = dbConn.ReadConfigValue("ConfigUpdated");
                if (!string.IsNullOrEmpty(configChanged) && (configChanged.CompareTo("1") == 0))
                {
                    log.WriteMessage("configuration changed");
                    string server = dbConn.ReadConfigValue("ServerName");
                    string serverUser = dbConn.ReadConfigValue("ServerLogin");
                    string serverPass = dbConn.ReadConfigValue("ServerPass");

                    dirList = dbConn.ReadDirsToWatch(0);

                    FileHelper helper = new PixFileHelper(server, serverUser, serverPass);
                    //helper.WatchDirs(dirList);
                    helper.AddDirListener(dirList);
                    dbConn.UpdateConfigValue("ConfigUpdated", "0");
                }
                else
                    log.WriteMessage("no config changed");
            }
            catch (Exception ex)
            {
                log.WriteMessage("Tossed in OnTimedEvent: " + ex.Message);
                log.WriteMessage("stack: " + ex.StackTrace);
            }
            aTimer.Enabled = true;
        }

        private static void OnConfigTimedEvent(object source, ElapsedEventArgs e)
        {
            configTimer.Enabled = false;

            Logger log = new Logger("Program::OnConfigTimedEvent");
            try
            {
                ConfigManager configMgr = new ConfigManager();
                configMgr.GetConfig();
            }
            catch (Exception ex)
            {
                log.WriteMessage("tossed in OnConfigTimedEvent: " + ex.Message);
                log.WriteMessage("stack: " + ex.Message);
            }
            configTimer.Enabled = true;
        }

        private static void OnQueueTimedEvent(object source, ElapsedEventArgs e)
        {
            queueTimer.Enabled = false;
            Logger log = new Logger("Program::OnQueueTimedEvent");
            try
            {
                PixDBInterface dbHelper = new PixDBInterface();

                dbHelper.Test();

                List<string> queuedFiles = dbHelper.GetAllQueuedPix();
                foreach (string file in queuedFiles)
                {
                    if (File.Exists(file))
                    {
                        FileHelper.FileResults addResult = m_fileHelper.AddFile(file);
                        if (addResult == FileHelper.FileResults.ESuccess)
                        {
                            //log.WriteMessage("Wrote file to server: " + file);
                            dbHelper.RemoveEntryFromPixQueue(file);
                        }
                        else if (addResult == FileHelper.FileResults.EAlreadySent)
                        {
                            //log.WriteMessage("File already sent! " + file);
                            dbHelper.RemoveEntryFromPixQueue(file);
                        }
                    }
                    else
                    {
                        //log.WriteMessage("File no longer exists! " + file);
                        dbHelper.RemoveEntryFromPixQueue(file);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteMessage("tossed in OnQueueTimedEvent: " + ex.Message);
                log.WriteMessage("stack: " + ex.Message);
            }
            queueTimer.Enabled = true;
        }

    }
}
