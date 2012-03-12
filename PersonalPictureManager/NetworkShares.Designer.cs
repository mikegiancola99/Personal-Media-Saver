namespace PersonalPictureManager
{
    partial class NetworkShares
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.showSharesBtn = new System.Windows.Forms.Button();
            this.createShareBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // showSharesBtn
            // 
            this.showSharesBtn.Location = new System.Drawing.Point(286, 278);
            this.showSharesBtn.Name = "showSharesBtn";
            this.showSharesBtn.Size = new System.Drawing.Size(123, 23);
            this.showSharesBtn.TabIndex = 0;
            this.showSharesBtn.Text = "Show Current Shares";
            this.showSharesBtn.UseVisualStyleBackColor = true;
            this.showSharesBtn.Click += new System.EventHandler(this.showSharesBtn_Click);
            // 
            // createShareBtn
            // 
            this.createShareBtn.Location = new System.Drawing.Point(341, 318);
            this.createShareBtn.Name = "createShareBtn";
            this.createShareBtn.Size = new System.Drawing.Size(75, 23);
            this.createShareBtn.TabIndex = 1;
            this.createShareBtn.Text = "Create Share";
            this.createShareBtn.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(26, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(235, 289);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // NetworkShares
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 390);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.createShareBtn);
            this.Controls.Add(this.showSharesBtn);
            this.Name = "NetworkShares";
            this.Text = "NetworkShares";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button showSharesBtn;
        private System.Windows.Forms.Button createShareBtn;
        private System.Windows.Forms.ListView listView1;
    }
}