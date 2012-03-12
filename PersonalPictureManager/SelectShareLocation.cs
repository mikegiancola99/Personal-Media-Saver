using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PictureUploader;

namespace PersonalPictureManager
{
    public partial class SelectShareLocation : Form
    {
        public static bool m_LocationSet;
        private bool m_shouldShutdown = true;

        public SelectShareLocation()
        {
            m_LocationSet = false;
            InitializeComponent();

            PixDBInterface pixDb = new PixDBInterface();
            string server = pixDb.ReadConfigValue("ServerName");
            if (string.IsNullOrEmpty(server))
                finishBtn.Enabled = false;
            else
                finishBtn.Enabled = true;

            newShareBtn.Enabled = false;
        }

        private void finishBtn_Click(object sender, EventArgs e)
        {
            PixDBInterface pixDb = new PixDBInterface();
            pixDb.UpdateConfigValue("ConfigUpdated", "1");
            Application.Exit();
        }

        private void existingBtn_Click(object sender, EventArgs e)
        {
            m_shouldShutdown = false;
            this.Hide();
            ChooseExisting choice = new ChooseExisting();
            choice.Show();
        }

        private void newShareBtn_Click(object sender, EventArgs e)
        {
            m_shouldShutdown = false;
            this.Hide();
            AddNew noob = new AddNew();
            noob.Show();
        }

        private void SelectShareLocation_Load(object sender, EventArgs e)
        {

        }

        private void FormClosedEvtHandler(Object sender, FormClosedEventArgs e)
        {
            if (m_shouldShutdown)
                Application.Exit();
        }
    }
}
