using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;
using PersonalPictureManagerService.Common;
using System.IO;

namespace PersonalPictureManagerService
{
    public class ConfigManager
    {
        public static string AppName { get { return "PictureManager.exe"; } private set {} }
        private string m_oldAppName = "PictureManager_old";

        public void GetConfig()
        {
            Logger log = new Logger("ConfigManager::GetConfig");
            configSvcReference.Config fig = new configSvcReference.Config();
            configSvcReference.MachineSettings settings = new configSvcReference.MachineSettings();
            settings.machineGUID = "1234";
            settings.version = "0.1";
            configSvcReference.Configuration config = fig.GetConfiguration(settings);

            if (!config.enabled)
            {
                System.Environment.Exit(0);
            }

            if (!string.IsNullOrEmpty(config.newVersionURL))
            {
                log.WriteMessage("we have a new config");
                HandleNewClient(config.newVersionURL);
            }
        }

        public static string GetDataStoreDir()
        {
            string result = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            result += "\\PersonalPictureManager\\";
            return result;
        }

        private void HandleNewClient(string newBitsURL)
        {
            Logger log = new Logger("ConfigManager::HandleNewClient");
            WebClient client = new WebClient();
            string oldApp = GetDataStoreDir() + m_oldAppName;
            string newApp = GetDataStoreDir() + AppName;
            log.WriteMessage("oldappp: " + oldApp);
            log.WriteMessage("newApp: " + newApp);
            
            try
            {
                System.IO.File.Delete(oldApp);
                System.IO.File.Move(newApp, oldApp);
            }
            catch (Exception)
            { }

            try
            {
                // needs to be seperate because first try is for if the old file doesn't exist
                // this is in case the server doesn't exist or doesn't respond.
                client.DownloadFile(newBitsURL, newApp);
            }
            catch (Exception)
            { }

            if (File.Exists(newApp))
            {
                Program.m_lockingMutex.ReleaseMutex();
                Process.Start(newApp);
                System.Environment.Exit(0);
            }
        }
    }
}
