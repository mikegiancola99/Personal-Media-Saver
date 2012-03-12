namespace PersonalPictureManager
{
    partial class AddNew
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
            this.doneBtn = new System.Windows.Forms.Button();
            this.pickDirBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // doneBtn
            // 
            this.doneBtn.Location = new System.Drawing.Point(47, 166);
            this.doneBtn.Name = "doneBtn";
            this.doneBtn.Size = new System.Drawing.Size(180, 23);
            this.doneBtn.TabIndex = 0;
            this.doneBtn.Text = "Done!";
            this.doneBtn.UseVisualStyleBackColor = true;
            this.doneBtn.Click += new System.EventHandler(this.doneBtn_Click);
            // 
            // pickDirBtn
            // 
            this.pickDirBtn.Location = new System.Drawing.Point(47, 123);
            this.pickDirBtn.Name = "pickDirBtn";
            this.pickDirBtn.Size = new System.Drawing.Size(180, 23);
            this.pickDirBtn.TabIndex = 1;
            this.pickDirBtn.Text = "Pick Directory to share";
            this.pickDirBtn.UseVisualStyleBackColor = true;
            this.pickDirBtn.Click += new System.EventHandler(this.pickDirBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "No directory selected.";
            // 
            // AddNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 216);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pickDirBtn);
            this.Controls.Add(this.doneBtn);
            this.Name = "AddNew";
            this.Text = "Add New Network Share";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button doneBtn;
        private System.Windows.Forms.Button pickDirBtn;
        private System.Windows.Forms.Label label1;
    }
}