namespace PersonalPictureManager
{
    partial class SelectShareLocation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectShareLocation));
            this.label1 = new System.Windows.Forms.Label();
            this.existingBtn = new System.Windows.Forms.Button();
            this.newShareBtn = new System.Windows.Forms.Button();
            this.finishBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 70);
            this.label1.TabIndex = 0;
            this.label1.Text = "This is the backup location - where all the data will be stored.\r\n\r\nThis is the l" +
                "ocation where all the pictures will be sent. \r\nIt should be a computer with a la" +
                "rge amount of free\r\nhard drive space.";
            // 
            // existingBtn
            // 
            this.existingBtn.Location = new System.Drawing.Point(17, 127);
            this.existingBtn.Name = "existingBtn";
            this.existingBtn.Size = new System.Drawing.Size(310, 25);
            this.existingBtn.TabIndex = 1;
            this.existingBtn.Text = "Choose an existing network location (share)";
            this.existingBtn.UseVisualStyleBackColor = true;
            this.existingBtn.Click += new System.EventHandler(this.existingBtn_Click);
            // 
            // newShareBtn
            // 
            this.newShareBtn.Location = new System.Drawing.Point(17, 171);
            this.newShareBtn.Name = "newShareBtn";
            this.newShareBtn.Size = new System.Drawing.Size(310, 25);
            this.newShareBtn.TabIndex = 2;
            this.newShareBtn.Text = "Create a network share on this computer";
            this.newShareBtn.UseVisualStyleBackColor = true;
            this.newShareBtn.Click += new System.EventHandler(this.newShareBtn_Click);
            // 
            // finishBtn
            // 
            this.finishBtn.Location = new System.Drawing.Point(236, 253);
            this.finishBtn.Name = "finishBtn";
            this.finishBtn.Size = new System.Drawing.Size(133, 25);
            this.finishBtn.TabIndex = 3;
            this.finishBtn.Text = "Finish Setup!";
            this.finishBtn.UseVisualStyleBackColor = true;
            this.finishBtn.Click += new System.EventHandler(this.finishBtn_Click);
            // 
            // SelectShareLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(380, 299);
            this.Controls.Add(this.finishBtn);
            this.Controls.Add(this.newShareBtn);
            this.Controls.Add(this.existingBtn);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectShareLocation";
            this.Text = "Picture Backup Location";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormClosedEvtHandler);
            this.Load += new System.EventHandler(this.SelectShareLocation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button existingBtn;
        private System.Windows.Forms.Button newShareBtn;
        private System.Windows.Forms.Button finishBtn;
    }
}