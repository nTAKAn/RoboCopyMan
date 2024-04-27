namespace RoboCopyMan
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            _notifyIcon = new NotifyIcon(components);
            _contextMenuStrip = new ContextMenuStrip(components);
            forcedBackupToolStripMenuItem = new ToolStripMenuItem();
            showResultDialogToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            _timer = new System.Windows.Forms.Timer(components);
            _contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _notifyIcon
            // 
            _notifyIcon.ContextMenuStrip = _contextMenuStrip;
            _notifyIcon.Icon = (Icon)resources.GetObject("_notifyIcon.Icon");
            _notifyIcon.Text = "RoboCopyMan";
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += _notifyIcon_DoubleClick;
            // 
            // _contextMenuStrip
            // 
            _contextMenuStrip.ImageScalingSize = new Size(20, 20);
            _contextMenuStrip.Items.AddRange(new ToolStripItem[] { forcedBackupToolStripMenuItem, showResultDialogToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            _contextMenuStrip.Name = "_contextMenuStrip";
            _contextMenuStrip.Size = new Size(195, 98);
            // 
            // forcedBackupToolStripMenuItem
            // 
            forcedBackupToolStripMenuItem.Name = "forcedBackupToolStripMenuItem";
            forcedBackupToolStripMenuItem.Size = new Size(194, 22);
            forcedBackupToolStripMenuItem.Text = "手動バックアップ実行(&E)";
            forcedBackupToolStripMenuItem.Click += forcedBackupToolStripMenuItem_Click;
            // 
            // showResultDialogToolStripMenuItem
            // 
            showResultDialogToolStripMenuItem.Name = "showResultDialogToolStripMenuItem";
            showResultDialogToolStripMenuItem.Size = new Size(194, 22);
            showResultDialogToolStripMenuItem.Text = "結果ダイアログを開く(&V)...";
            showResultDialogToolStripMenuItem.Click += showResultDialogToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(191, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(194, 22);
            exitToolStripMenuItem.Text = "終了(&E)";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // _timer
            // 
            _timer.Interval = 1000;
            _timer.Tick += _timer_Tick;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(280, 157);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormMain";
            Text = "Form1";
            Load += FormMain_Load;
            _contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _contextMenuStrip;
        private ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer _timer;
        private ToolStripMenuItem showResultDialogToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem forcedBackupToolStripMenuItem;
    }
}
