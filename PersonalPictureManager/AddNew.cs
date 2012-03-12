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
    public partial class AddNew : Form
    {
        private bool m_shouldShutdown = true;

        public AddNew()
        {
            InitializeComponent();
        }

        private void pickDirBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.ShowNewFolderButton = true;

            // Default to the My Documents folder.
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                ListViewItem item1 = new ListViewItem(folderName);
                item1.Checked = true;
                FileShareManager fileMgr = new FileShareManager();
                fileMgr.CreateShare(".", item1.Text, item1.Text, "Backup Location for Pictures", false);
                //directoriesListView.Items.Add(item1);
            }
            ChooseExisting exist = new ChooseExisting();
            exist.Show();
            this.Hide();
        }

        private void doneBtn_Click(object sender, EventArgs e)
        {
            m_shouldShutdown = false;
        }

        private void Form1_FormClosed(Object sender, FormClosedEventArgs e)
        {
            SelectShareLocation loc = new SelectShareLocation();
            loc.Show();
            this.Hide();
        }
    }
}
