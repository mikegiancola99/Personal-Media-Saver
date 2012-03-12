namespace PersonalPictureManager
{
    partial class ChooseExisting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseExisting));
            this.networkShareView = new System.Windows.Forms.ListView();
            this.doneBtn = new System.Windows.Forms.Button();
            this.specifyLocBox = new System.Windows.Forms.TextBox();
            this.specifyLable = new System.Windows.Forms.Label();
            this.searchBtn = new System.Windows.Forms.Button();
            this.descrTxt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // networkShareView
            // 
            this.networkShareView.CheckBoxes = true;
            this.networkShareView.FullRowSelect = true;
            this.networkShareView.Location = new System.Drawing.Point(33, 76);
            this.networkShareView.Name = "networkShareView";
            this.networkShareView.Size = new System.Drawing.Size(500, 136);
            this.networkShareView.TabIndex = 0;
            this.networkShareView.UseCompatibleStateImageBehavior = false;
            this.networkShareView.View = System.Windows.Forms.View.List;
            // 
            // doneBtn
            // 
            this.doneBtn.Location = new System.Drawing.Point(464, 317);
            this.doneBtn.Name = "doneBtn";
            this.doneBtn.Size = new System.Drawing.Size(87, 25);
            this.doneBtn.TabIndex = 1;
            this.doneBtn.Text = "Done!";
            this.doneBtn.UseVisualStyleBackColor = true;
            this.doneBtn.Click += new System.EventHandler(this.doneBtn_Click);
            // 
            // specifyLocBox
            // 
            this.specifyLocBox.Location = new System.Drawing.Point(33, 281);
            this.specifyLocBox.Name = "specifyLocBox";
            this.specifyLocBox.Size = new System.Drawing.Size(502, 22);
            this.specifyLocBox.TabIndex = 2;
            // 
            // specifyLable
            // 
            this.specifyLable.AutoSize = true;
            this.specifyLable.Location = new System.Drawing.Point(32, 261);
            this.specifyLable.Name = "specifyLable";
            this.specifyLable.Size = new System.Drawing.Size(215, 14);
            this.specifyLable.TabIndex = 3;
            this.specifyLable.Text = "Specify the network location yourself:";
            // 
            // searchBtn
            // 
            this.searchBtn.Location = new System.Drawing.Point(193, 218);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(173, 25);
            this.searchBtn.TabIndex = 4;
            this.searchBtn.Text = "Find Available Shares";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // descrTxt
            // 
            this.descrTxt.AutoSize = true;
            this.descrTxt.Location = new System.Drawing.Point(35, 13);
            this.descrTxt.Name = "descrTxt";
            this.descrTxt.Size = new System.Drawing.Size(0, 14);
            this.descrTxt.TabIndex = 5;
            // 
            // ChooseExisting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(563, 354);
            this.Controls.Add(this.descrTxt);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.specifyLable);
            this.Controls.Add(this.specifyLocBox);
            this.Controls.Add(this.doneBtn);
            this.Controls.Add(this.networkShareView);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChooseExisting";
            this.Text = "Choose an Existing Network Share";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.MainWindowDoneResize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView networkShareView;
        private System.Windows.Forms.Button doneBtn;
        private System.Windows.Forms.TextBox specifyLocBox;
        private System.Windows.Forms.Label specifyLable;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Label descrTxt;
    }
}