﻿#pragma warning disable IDE1006 // 命名スタイル

namespace RoboCopyMan
{
    public partial class FormResult : Form
    {
        // リストビューで選択されているタスクを取得する
        private BackupTask? SelectedTask
        {
            get
            {
                if (_listView.SelectedItems.Count == 0)
                    return null;

                var index = _listView.SelectedItems[0].Index;
                return Program.BackupManager.BackupTasks[index];
            }
        }

        public FormResult()
        {
            InitializeComponent();
        }

        private void FormResult_Load(object sender, EventArgs e)
        {
            UpdateResults();

            // 表示変更のためにバックアップマネージャのバックアップタスク実行イベントを登録
            Program.BackupManager.BackupTaskExecuted += _backupManager_BackupTaskExecuted;
        }

        private void FormResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            // イベントの登録解除
            Program.BackupManager.BackupTaskExecuted -= _backupManager_BackupTaskExecuted;
        }

        private void FormResult_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        void _backupManager_BackupTaskExecuted(object sender, EventArgs e)
        {
            UpdateResults();
        }

        private void UpdateResults()
        {
            _listView.Items.Clear();

            var tasks = Program.BackupManager.BackupTasks;
            foreach (var task in tasks)
            {
                var item = new ListViewItem(task.Setting.Title);
                item.SubItems.Add(task.LastBackupTime.ToString("yyyy/MM/dd HH:mm:ss"));
                item.SubItems.Add(task.NextTriggerTime.ToString("yyyy/MM/dd HH:mm:ss"));
                item.SubItems.Add(task.LastException?.Message ?? "正常");
                _listView.Items.Add(item);
            }
        }

        private void _listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var task = SelectedTask;
            if (task is null)
            {
                _toolStripStatusLabel_SrcDir.Text = "コピー元：　";
                _toolStripStatusLabel_DstDir.Text = "コピー先：　";
                return;
            }

            _toolStripStatusLabel_SrcDir.Text = $"コピー元：　{task.Setting.SrcDir}";
            _toolStripStatusLabel_DstDir.Text = $"コピー先：　{task.Setting.DstDir}";
        }
        private void _listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var task = SelectedTask;
            if (task is null)
                return;

            System.Diagnostics.Process.Start("explorer.exe", task.Setting.LogDir);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
