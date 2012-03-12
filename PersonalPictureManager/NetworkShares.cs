using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ServerEnumerator;
using System.Collections;
namespace PersonalPictureManager
{
    public partial class NetworkShares : Form
    {
        public NetworkShares()
        {
            InitializeComponent();
        }

        private void showSharesBtn_Click(object sender, EventArgs e)
        {
            ServerEnum server = new ServerEnum(ResourceScope.RESOURCE_GLOBALNET, 
                                               ResourceType.RESOURCETYPE_ANY,
                                               ResourceUsage.RESOURCEUSAGE_ALL,
                                               ResourceDisplayType.RESOURCEDISPLAYTYPE_SHARE);
            foreach (string foo in server)
            {
                ListViewItem item1 = new ListViewItem(foo);
                item1.Checked = false;
                listView1.Items.Add(item1);
            }
        }
    }
}
