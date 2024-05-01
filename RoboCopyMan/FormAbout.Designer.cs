namespace RoboCopyMan
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            _label_title = new Label();
            _label_copyright = new Label();
            _linkLabel_github = new LinkLabel();
            SuspendLayout();
            // 
            // _label_title
            // 
            _label_title.AutoSize = true;
            _label_title.Location = new Point(12, 18);
            _label_title.Name = "_label_title";
            _label_title.Size = new Size(169, 15);
            _label_title.TabIndex = 0;
            _label_title.Text = "RoboCopyMan Ver 1.0.0-beta.8";
            // 
            // _label_copyright
            // 
            _label_copyright.AutoSize = true;
            _label_copyright.Location = new Point(12, 44);
            _label_copyright.Name = "_label_copyright";
            _label_copyright.Size = new Size(213, 15);
            _label_copyright.TabIndex = 0;
            _label_copyright.Text = "Copyright (C) 2024 Nobuyuki Takahashi";
            // 
            // _linkLabel_github
            // 
            _linkLabel_github.AutoSize = true;
            _linkLabel_github.Location = new Point(12, 74);
            _linkLabel_github.Name = "_linkLabel_github";
            _linkLabel_github.Size = new Size(240, 15);
            _linkLabel_github.TabIndex = 1;
            _linkLabel_github.TabStop = true;
            _linkLabel_github.Text = "https://github.com/nTAKAn/RoboCopyMan";
            _linkLabel_github.LinkClicked += _linkLabel_github_LinkClicked;
            // 
            // FormAbout
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(324, 105);
            Controls.Add(_linkLabel_github);
            Controls.Add(_label_copyright);
            Controls.Add(_label_title);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAbout";
            Text = "RoboCopyMan について";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label _label_title;
        private Label _label_copyright;
        private LinkLabel _linkLabel_github;
    }
}