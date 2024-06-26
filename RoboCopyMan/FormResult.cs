﻿using System.Diagnostics;

#pragma warning disable IDE1006 // 命名スタイル

namespace RoboCopyMan
{
    public partial class FormResult : Form
    {
        /// <summary>
        /// 通常時のタイトル
        /// </summary>
        private static readonly string _TITLE = "バックアップ結果";
        /// <summary>
        /// 実行中のタイトル
        /// </summary>
        private static readonly string _EXECUTE_TITLE = "バックアップ結果 (実行中)";

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
            UpdateResultList();

            // 表示変更のためにバックアップマネージャのバックアップタスク実行イベントを登録
            Program.BackupManager.BeginBackup += _backupManager_BeginBackupEventHandler;
            Program.BackupManager.EndBackup += _backupManager_EndBackupEventHandler;

            Text = _TITLE;
        }

        private void FormResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            // イベントの登録解除
            Program.BackupManager.BeginBackup -= _backupManager_BeginBackupEventHandler;
            Program.BackupManager.EndBackup -= _backupManager_EndBackupEventHandler;
        }

        private void FormResult_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        /// <summary>
        /// バックアップ開始時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _backupManager_BeginBackupEventHandler(object sender, EventArgs e)
        {
            Invoke(() =>
            {
                Text = _EXECUTE_TITLE;
                _listView.Enabled = false;
            });
        }
        /// <summary>
        /// バックアップ終了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _backupManager_EndBackupEventHandler(object sender, EventArgs e)
        {
            Invoke(() =>
            {
                Text = _TITLE;
                _listView.Enabled = true;

                UpdateResultList();
            });
        }

        /// <summary>
        /// 結果リストを更新する
        /// </summary>
        private void UpdateResultList()
        {
            _listView.Items.Clear();

            var tasks = Program.BackupManager.BackupTasks;
            foreach (var task in tasks)
            {
                var item = new ListViewItem(task.Setting.Title);
                item.SubItems.Add(task.LastBackupTime.ToString("yyyy/MM/dd HH:mm:ss"));
                item.SubItems.Add(task.NextTriggerTime.ToString("yyyy/MM/dd HH:mm:ss"));

                if (task.LastException is not null)
                    item.SubItems.Add($"例外で終了: {task.LastException.Message}");
                else if (!task.IsSuccessful)
                    item.SubItems.Add($"エラー: 終了コード {task.ExitCode}");
                else
                    item.SubItems.Add($"正常: 終了コード {task.ExitCode}");

                _listView.Items.Add(item);
            }
        }
        /// <summary>
        /// 結果リストの選択が変更されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var task = SelectedTask;
            if (task is null)
            {
                _toolStripStatusLabel_SrcDir.Text = "コピー元：　";
                _toolStripStatusLabel_DstDir.Text = "コピー先：　";

                _textBox_stdOut.Text = string.Empty;
                return;
            }

            _toolStripStatusLabel_SrcDir.Text = $"コピー元：　{task.Setting.SrcDir}";
            _toolStripStatusLabel_DstDir.Text = $"コピー先：　{task.Setting.DstDir}";

            // robocopy の標準出力を表示
            _textBox_stdOut.Text = task.StdOutput;
        }
        private void _listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var task = SelectedTask;
            if (task is null)
                return;

            if (task.Setting.LogDir is not null)
                Process.Start("explorer.exe", task.Setting.LogDir);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 選択されているバックアップ設定の robocopy コマンドをクリップボードにコピーする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void makeCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var task = SelectedTask;
            if (task is null)
                return;

            Robocopy robocopy = new(task.Setting);
            Clipboard.SetText(robocopy.Command);
        }

        /// <summary>
        /// 選択されているバックアップ設定ファイルを編集する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editSettingFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var task = SelectedTask;
            if (task is null)
                return;

            try
            {
                ProcessStartInfo pi = new()
                {
                    FileName = task.Filepath,
                    UseShellExecute = true, // 重要
                };
                Process.Start(pi);
            }
            catch (Exception ex)
            {
                SerilogWrapper.Error(ex, "設定ファイルの編集に失敗しました.");
                return;
            }
        }

        private void openSettingDataDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var task = SelectedTask;
            if (task is null)
                return;

            try
            {
                var dir = Path.GetDirectoryName(task.Filepath);

                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    Process.Start("explorer.exe", dir);
            }
            catch (Exception ex)
            {
                SerilogWrapper.Error(ex, "設定ファイルの編集に失敗しました.");
                return;
            }
        }
    }
}
