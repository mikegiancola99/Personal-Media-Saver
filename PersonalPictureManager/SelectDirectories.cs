using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PictureUploader;

namespace PersonalPictureManager
{
    public partial class SelectDirectories : Form
    {
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private bool m_shouldShutdown = true;
        public SelectDirectories()
        {
            InitializeComponent();
            String descrTxt = "Pictures in the directories checked below will be copied to the shared folder\n";
            descrTxt += "If listed but **unchecked**, the directory will not monitored and the pictures\n";
            descrTxt += "not copied.\n";
            descrTxt += "\n\nTo add additional directories, press the \"Add Another Directory\" button.\n";
            descrTxt += "When finished, go on to the next step!";
            descrLabel.Text = descrTxt;

            AddBasicDirs();
        }

        private void LoadConfigDirs()
        {
            PixDBInterface pixDb = new PixDBInterface();
            List<string> dirs = pixDb.ReadDirsToWatch(0);
            if (dirs.Count > 0)
            {
                foreach (string entry in dirs)
                {
                    ListViewItem curItem = new ListViewItem(entry);
                    curItem.Checked = true;
                    dirsListView.Items.Add(curItem);
                }
            }
            else
            {
                AddBasicDirs();
            }
        }

        private void AddBasicDirs()
        {
            string folderName = "My Pictures";
            ListViewItem myPixItem = new ListViewItem(folderName);
            myPixItem.Checked = true;
            dirsListView.Items.Add(myPixItem);

            folderName = "Desktop";
            ListViewItem myDesktopItem = new ListViewItem(folderName);
            myDesktopItem.Checked = true;
            dirsListView.Items.Add(myDesktopItem);
        }

        private void addDirBtn_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog1.ShowNewFolderButton = false;

            // Default to the My Documents folder.
            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                ListViewItem item1 = new ListViewItem(folderName);
                item1.Checked = true;
                dirsListView.Items.Add(item1);
            }
        }

        private void nextStepBtn_Click(object sender, EventArgs e)
        {
            if (dirsListView.Items.Count > 0)
            {
                PixDBInterface pixDb = new PixDBInterface();
                pixDb.RemoveAllDirsToWatch();
                foreach (ListViewItem item in dirsListView.Items)
                {
                    string dirToWatch = item.Text;
                    if (dirToWatch.CompareTo("Desktop") == 0)
                        dirToWatch = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    else if (dirToWatch.CompareTo("My Pictures") == 0)
                        dirToWatch = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                    pixDb.AddDirToWatch(dirToWatch, 0);
                }
                m_shouldShutdown = false;
                this.Hide();
                SelectShareLocation shareLoc = new SelectShareLocation();
                shareLoc.Show();
            }
        }

        private void FormClosedEvtHandler(Object sender, FormClosedEventArgs e)
        {
            if (m_shouldShutdown)
                Application.Exit();
        }
    }
}
