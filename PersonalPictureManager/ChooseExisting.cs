using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ServerEnumerator;
using PictureUploader;

namespace PersonalPictureManager
{
    public partial class ChooseExisting : Form
    {
        public ChooseExisting()
        {
            InitializeComponent();
            String descTxt = "If you know the where you'd like to copy the pictures to (the shared folder)\n";
            descTxt += "you can enter it below. For example, a share could be \\\\home-server\\pictures\n";
            descTxt += "\nIf you don't know the location, we can attempt to find it for you.";
            descrTxt.Text = descTxt;
            PixDBInterface pixDb = new PixDBInterface();
            string serverName = pixDb.ReadConfigValue("ServerName");
            if (serverName.Length > 0)
                specifyLocBox.Text = serverName;
        }

        private void doneBtn_Click(object sender, EventArgs e)
        {
            PixDBInterface pixDb = new PixDBInterface();

            String networkShare = null;

            if (networkShareView.CheckedItems.Count > 0)
            {
                ListViewItem item = networkShareView.CheckedItems[0];
                networkShare = item.Text;
            }
            else if (specifyLocBox.Text.Length > 0)
                networkShare = specifyLocBox.Text;

            if (!String.IsNullOrEmpty(networkShare))
            {
                pixDb.UpdateConfigValue("ServerName", networkShare);
                SelectShareLocation.m_LocationSet = true;

                SelectShareLocation loc = new SelectShareLocation();
                loc.Show();
                this.Hide();
            }
        }

        private void MainWindowDoneResize(object sender, EventArgs e)
        {
            int width = this.Size.Width;
            int height = this.Size.Height;

            if (width < 137)
                width = 137;

            if (height < 278)
                height = 278;

            this.Size = new System.Drawing.Size(width, height);

            int rightMargin = 80;
            if (width < 331)
                rightMargin = 10;

            //networkShareView.Width = width - rightMargin;
            networkShareView.Size = new System.Drawing.Size(width - rightMargin, networkShareView.Height);
        }

        private void Form1_FormClosed(Object sender, FormClosedEventArgs e)
        {
            SelectShareLocation loc = new SelectShareLocation();
            loc.Show();
            this.Hide();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            ServerEnum server = new ServerEnum(ResourceScope.RESOURCE_GLOBALNET,
            ResourceType.RESOURCETYPE_ANY,
            ResourceUsage.RESOURCEUSAGE_ALL,
            ResourceDisplayType.RESOURCEDISPLAYTYPE_SHARE);


            foreach (string serverItem in server)
            {
                ListViewItem item1 = new ListViewItem(serverItem);
                item1.Checked = false;
                networkShareView.Items.Add(item1);
            }
        }
    }
}
