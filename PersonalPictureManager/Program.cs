using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace PersonalPictureManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool running = true;
            try
            {
                Mutex.OpenExisting("PersonalPictureManagerService");
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                running = false;
            }

            if (!running)
            {
                try
                {
                    string progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

                    string newApp = progFiles + "\\PersonalPictureManager\\PersonalPictureManagerService.exe";
                    //Process.Start(newApp);
                }
                catch (Exception) { }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SetupSplash());
        }
    }
}
