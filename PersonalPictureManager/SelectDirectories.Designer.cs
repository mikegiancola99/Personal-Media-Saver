namespace PersonalPictureManager
{
    partial class SelectDirectories
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectDirectories));
            this.descrLabel = new System.Windows.Forms.Label();
            this.addDirBtn = new System.Windows.Forms.Button();
            this.nextStepBtn = new System.Windows.Forms.Button();
            this.dirsListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // descrLabel
            // 
            this.descrLabel.AutoSize = true;
            this.descrLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descrLabel.Location = new System.Drawing.Point(27, 16);
            this.descrLabel.Name = "descrLabel";
            this.descrLabel.Size = new System.Drawing.Size(0, 14);
            this.descrLabel.TabIndex = 7;
            // 
            // addDirBtn
            // 
            this.addDirBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addDirBtn.Location = new System.Drawing.Point(30, 281);
            this.addDirBtn.Name = "addDirBtn";
            this.addDirBtn.Size = new System.Drawing.Size(148, 23);
            this.addDirBtn.TabIndex = 6;
            this.addDirBtn.Text = "Add Another Directory";
            this.addDirBtn.UseVisualStyleBackColor = true;
            this.addDirBtn.Click += new System.EventHandler(this.addDirBtn_Click);
            // 
            // nextStepBtn
            // 
            this.nextStepBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextStepBtn.Location = new System.Drawing.Point(476, 319);
            this.nextStepBtn.Name = "nextStepBtn";
            this.nextStepBtn.Size = new System.Drawing.Size(75, 23);
            this.nextStepBtn.TabIndex = 5;
            this.nextStepBtn.Text = "Next Step ->";
            this.nextStepBtn.UseVisualStyleBackColor = true;
            this.nextStepBtn.Click += new System.EventHandler(this.nextStepBtn_Click);
            // 
            // dirsListView
            // 
            this.dirsListView.CheckBoxes = true;
            this.dirsListView.FullRowSelect = true;
            this.dirsListView.Location = new System.Drawing.Point(30, 151);
            this.dirsListView.Name = "dirsListView";
            this.dirsListView.Size = new System.Drawing.Size(476, 124);
            this.dirsListView.TabIndex = 4;
            this.dirsListView.UseCompatibleStateImageBehavior = false;
            this.dirsListView.View = System.Windows.Forms.View.List;
            // 
            // SelectDirectories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(563, 354);
            this.Controls.Add(this.descrLabel);
            this.Controls.Add(this.addDirBtn);
            this.Controls.Add(this.nextStepBtn);
            this.Controls.Add(this.dirsListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectDirectories";
            this.Text = "Select Directories";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormClosedEvtHandler);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label descrLabel;
        private System.Windows.Forms.Button addDirBtn;
        private System.Windows.Forms.Button nextStepBtn;
        private System.Windows.Forms.ListView dirsListView;

    }
}