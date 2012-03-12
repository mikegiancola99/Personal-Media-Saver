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
    public partial class SetupSplash : Form
    {
        public SetupSplash()
        {
            InitializeComponent();
            this.DescriptionLabel.Size = new System.Drawing.Size(0, 83);
            
            String descriptionText = "Welcome! This application will consolidate consolidating your pictures into a single location! \n";
            descriptionText += "You'll no longer have to wonder where any pictures are and backups will be a snap!\n";
            descriptionText += "\nBefore we begin, we'll need:\n";
            descriptionText += "     A place to store the pictures - this needs to be a computer with a lot of hard drive space \n";
            descriptionText += "               and the directory needs to be \'shared\' for everyone (sorry, no password is supported yet)\n";
            descriptionText += "               to learn how to share, visit: \n";
            descriptionText += "                http://windows.microsoft.com/en-US/windows7/Share-files-with-someone\n";
            descriptionText += "\n   The directories where you normally keep your pictures. This is a location where you have access and \n";
            descriptionText += "               don\'t have to give administrator rights. It's normally \'My Pictures\' or on your Desktop\n";
            descriptionText += "\nWhen you're ready, click the Start button below!";


            this.DescriptionLabel.Text = descriptionText;
            try
            {
                PixDBInterface pixDb = new PixDBInterface();
                if (!pixDb.CheckIfDbSetup())
                {
                    pixDb.CreateDatabase();
                    pixDb.SetupTables();
                }
            }
            catch (Exception e)
            {
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            SelectDirectories selDirs = new SelectDirectories();
            selDirs.Show();
           // this.Close();
        }

    }
}
