﻿namespace RoboCopyMan
{
    partial class FormResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResult));
            _menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolToolStripMenuItem = new ToolStripMenuItem();
            makeCommandToolStripMenuItem = new ToolStripMenuItem();
            editSettingFileToolStripMenuItem = new ToolStripMenuItem();
            openSettingDataDirToolStripMenuItem = new ToolStripMenuItem();
            _statusStrip = new StatusStrip();
            _toolStripStatusLabel_SrcDir = new ToolStripStatusLabel();
            _toolStripStatusLabel_DstDir = new ToolStripStatusLabel();
            _listView = new ListView();
            columnHeader_title = new ColumnHeader();
            columnHeader_lastBackupTime = new ColumnHeader();
            columnHeader_nextBackupTime = new ColumnHeader();
            columnHeader_result = new ColumnHeader();
            _splitContainer = new SplitContainer();
            _textBox_stdOut = new TextBox();
            _menuStrip.SuspendLayout();
            _statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
            _splitContainer.Panel1.SuspendLayout();
            _splitContainer.Panel2.SuspendLayout();
            _splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolToolStripMenuItem });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new Size(627, 24);
            _menuStrip.TabIndex = 3;
            _menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(67, 20);
            fileToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(119, 22);
            exitToolStripMenuItem.Text = "閉じる(&C)";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolToolStripMenuItem
            // 
            toolToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { makeCommandToolStripMenuItem, editSettingFileToolStripMenuItem, openSettingDataDirToolStripMenuItem });
            toolToolStripMenuItem.Name = "toolToolStripMenuItem";
            toolToolStripMenuItem.Size = new Size(60, 20);
            toolToolStripMenuItem.Text = "ツール(&T)";
            // 
            // makeCommandToolStripMenuItem
            // 
            makeCommandToolStripMenuItem.Name = "makeCommandToolStripMenuItem";
            makeCommandToolStripMenuItem.Size = new Size(298, 22);
            makeCommandToolStripMenuItem.Text = "robocopy コマンドをクリップボードにコピーする(&C)";
            makeCommandToolStripMenuItem.Click += makeCommandToolStripMenuItem_Click;
            // 
            // editSettingFileToolStripMenuItem
            // 
            editSettingFileToolStripMenuItem.Name = "editSettingFileToolStripMenuItem";
            editSettingFileToolStripMenuItem.Size = new Size(298, 22);
            editSettingFileToolStripMenuItem.Text = "設定ファイルを編集する(&E)";
            editSettingFileToolStripMenuItem.Click += editSettingFileToolStripMenuItem_Click;
            // 
            // openSettingDataDirToolStripMenuItem
            // 
            openSettingDataDirToolStripMenuItem.Name = "openSettingDataDirToolStripMenuItem";
            openSettingDataDirToolStripMenuItem.Size = new Size(298, 22);
            openSettingDataDirToolStripMenuItem.Text = "設定ファイルのフォルダを開く(&F)";
            openSettingDataDirToolStripMenuItem.Click += openSettingDataDirToolStripMenuItem_Click;
            // 
            // _statusStrip
            // 
            _statusStrip.Items.AddRange(new ToolStripItem[] { _toolStripStatusLabel_SrcDir, _toolStripStatusLabel_DstDir });
            _statusStrip.Location = new Point(0, 454);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new Size(627, 22);
            _statusStrip.TabIndex = 4;
            _statusStrip.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel_SrcDir
            // 
            _toolStripStatusLabel_SrcDir.Name = "_toolStripStatusLabel_SrcDir";
            _toolStripStatusLabel_SrcDir.Size = new Size(68, 17);
            _toolStripStatusLabel_SrcDir.Text = "コピー元：　";
            // 
            // _toolStripStatusLabel_DstDir
            // 
            _toolStripStatusLabel_DstDir.Name = "_toolStripStatusLabel_DstDir";
            _toolStripStatusLabel_DstDir.Size = new Size(68, 17);
            _toolStripStatusLabel_DstDir.Text = "コピー先：　";
            // 
            // _listView
            // 
            _listView.Columns.AddRange(new ColumnHeader[] { columnHeader_title, columnHeader_lastBackupTime, columnHeader_nextBackupTime, columnHeader_result });
            _listView.Dock = DockStyle.Fill;
            _listView.FullRowSelect = true;
            _listView.GridLines = true;
            _listView.HideSelection = true;
            _listView.Location = new Point(0, 0);
            _listView.Name = "_listView";
            _listView.Size = new Size(627, 180);
            _listView.TabIndex = 5;
            _listView.UseCompatibleStateImageBehavior = false;
            _listView.View = View.Details;
            _listView.SelectedIndexChanged += _listView_SelectedIndexChanged;
            _listView.MouseDoubleClick += _listView_MouseDoubleClick;
            // 
            // columnHeader_title
            // 
            columnHeader_title.Text = "タイトル";
            columnHeader_title.Width = 200;
            // 
            // columnHeader_lastBackupTime
            // 
            columnHeader_lastBackupTime.Text = "最後のバックアップ";
            columnHeader_lastBackupTime.Width = 150;
            // 
            // columnHeader_nextBackupTime
            // 
            columnHeader_nextBackupTime.Text = "次回バックアップ";
            columnHeader_nextBackupTime.Width = 150;
            // 
            // columnHeader_result
            // 
            columnHeader_result.Text = "状態";
            columnHeader_result.Width = 200;
            // 
            // _splitContainer
            // 
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.FixedPanel = FixedPanel.Panel1;
            _splitContainer.Location = new Point(0, 24);
            _splitContainer.Name = "_splitContainer";
            _splitContainer.Orientation = Orientation.Horizontal;
            // 
            // _splitContainer.Panel1
            // 
            _splitContainer.Panel1.Controls.Add(_listView);
            // 
            // _splitContainer.Panel2
            // 
            _splitContainer.Panel2.Controls.Add(_textBox_stdOut);
            _splitContainer.Size = new Size(627, 430);
            _splitContainer.SplitterDistance = 180;
            _splitContainer.TabIndex = 6;
            // 
            // _textBox_stdOut
            // 
            _textBox_stdOut.Dock = DockStyle.Fill;
            _textBox_stdOut.Location = new Point(0, 0);
            _textBox_stdOut.Multiline = true;
            _textBox_stdOut.Name = "_textBox_stdOut";
            _textBox_stdOut.ReadOnly = true;
            _textBox_stdOut.ScrollBars = ScrollBars.Both;
            _textBox_stdOut.Size = new Size(627, 246);
            _textBox_stdOut.TabIndex = 0;
            _textBox_stdOut.WordWrap = false;
            // 
            // FormResult
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(627, 476);
            Controls.Add(_splitContainer);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "FormResult";
            Text = "バックアップ結果";
            FormClosing += FormResult_FormClosing;
            FormClosed += FormResult_FormClosed;
            Load += FormResult_Load;
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            _splitContainer.Panel1.ResumeLayout(false);
            _splitContainer.Panel2.ResumeLayout(false);
            _splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_splitContainer).EndInit();
            _splitContainer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private StatusStrip _statusStrip;
        private ListView _listView;
        private ColumnHeader columnHeader_title;
        private ColumnHeader columnHeader_lastBackupTime;
        private ColumnHeader columnHeader_nextBackupTime;
        private ColumnHeader columnHeader_result;
        private ToolStripStatusLabel _toolStripStatusLabel_SrcDir;
        private ToolStripStatusLabel _toolStripStatusLabel_DstDir;
        private ToolStripMenuItem toolToolStripMenuItem;
        private ToolStripMenuItem makeCommandToolStripMenuItem;
        private ToolStripMenuItem editSettingFileToolStripMenuItem;
        private SplitContainer _splitContainer;
        private TextBox _textBox_stdOut;
        private ToolStripMenuItem openSettingDataDirToolStripMenuItem;
    }
}