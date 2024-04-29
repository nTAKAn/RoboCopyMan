#pragma warning disable IDE1006 // 命名スタイル

using System.Diagnostics;

namespace RoboCopyMan
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// 結果ダイアログ
        /// </summary>
        private FormResult? _formResult;
        /// <summary>
        /// About ダイアログ
        /// </summary>
        private FormAbout? _formAbout;

        public FormMain()
        {
            ShowInTaskbar = false;
            InitializeComponent();

            UpdateNotifyIcon();

            Program.BackupManager.BackupTaskExecuted += _backupManager_BackupTaskExecuted;
            Program.BackupManager.BeginBackup += _backupManager_BeginBackupEventHandler;
            Program.BackupManager.EndBackup += _backupManager_EndBackupEventHandler;

            _timer.Start();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // ここは呼ばれない
        }

        /// <summary>
        /// バックアップを実行するためのポーリング処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void _timer_Tick(object sender, EventArgs e)
        {
            if (Program.BackupManager.IsExecuting)
            {
                Debug.WriteLine("バックアップ実行中のためポーリング処理でのバックアップをスキップします.");
                return;
            }

            try
            {
                await Program.BackupManager.ExecuteAsync();
            }
            catch (Exception ex)
            {
                SerilogWrapper.Error(ex, "ポーリング処理でのバックアップに失敗しました.");
            }
        }

        /// <summary>
        /// バックアップタスクが実行されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backupManager_BackupTaskExecuted(object sender, EventArgs e)
        {
            UpdateNotifyIcon();
        }
        /// <summary>
        /// バックアップ開始時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _backupManager_BeginBackupEventHandler(object sender, EventArgs e)
        {
            // バックアップ実行中はメニューを無効化
            _contextMenuStrip.Enabled = false;

            UpdateNotifyIcon();
        }
        /// <summary>
        /// バックアップ終了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _backupManager_EndBackupEventHandler(object sender, EventArgs e)
        {
            // バックアップが終了したらメニューを有効化
            _contextMenuStrip.Enabled = true;

            UpdateNotifyIcon();
        }

        /// <summary>
        /// タスクトレイの通知アイコン更新
        /// </summary>
        private void UpdateNotifyIcon()
        {
            // バックアップ実行中の場合
            if (Program.BackupManager.IsExecuting)
            {
                _notifyIcon.Icon = Properties.Resources.gosenzo_other;
                _notifyIcon.Text = "RoboCopyMan (状態:バックアップ中)";
            }
            else
            {
                if (Program.BackupManager.IsErrorOccured)
                {
                    _notifyIcon.Icon = Properties.Resources.gosenzo_error;
                    _notifyIcon.Text = "RoboCopyMan (状態:異常)" +
                        $"\n次回バックアップ {Program.BackupManager.NextBackupTime:HH:mm:ss}";
                }
                else
                {
                    _notifyIcon.Icon = Properties.Resources.gosenzo_ok;
                    _notifyIcon.Text = "RoboCopyMan (状態:正常)" +
                        $"\n次回バックアップ {Program.BackupManager.NextBackupTime:HH:mm:ss}";
                }
            }
        }

        /// <summary>
        /// アプリケーションを終了する
        /// </summary>
        /// <param name="restart">true: 再起動する</param>
        private void AppExit(bool restart)
        {
            try
            {
                _timer.Stop();
                _timer.Dispose();

                _notifyIcon.Visible = false;

                //var task = Task.Run(() =>
                //{
                //    while (Program.BackupManager.IsExecuting)
                //        Thread.Sleep(1000);
                //});

                //if (task.Wait(10 * 1000))
                //{
                //    Debug.WriteLine("バックアップ中のため終了を待機");
                //}

                // 結果ダイアログが表示されている場合は閉じる
                if (_formResult != null)
                {
                    Program.BackupManager.BackupTaskExecuted -= _backupManager_BackupTaskExecuted;
                    _formResult.Close();
                    _formResult = null;
                }

                // About ダイアログが表示されている場合は閉じる
                if (_formAbout != null)
                {
                    _formAbout.Close();
                    _formAbout = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.APPFULLNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                SerilogWrapper.Error(ex, "終了処理でエラーが発生しました.");
            }
            finally
            {
                Close();

                if (restart)
                    Application.Restart();
                else
                    Application.Exit();
            }
        }

        /// <summary>
        /// メニュー：　終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppExit(false);
        }
        /// <summary>
        /// メニュー：　再起動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("再起動しますか？", Program.APPFULLNAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            AppExit(true);
        }

        /// <summary>
        /// メニュー：　結果ダイアログを表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showResultDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowResultDialog();
        }
        /// <summary>
        /// メニュー：　強制バックアップ（手動バックアップ）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void forcedBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.BackupManager.IsExecuting)
            {
                Debug.WriteLine("バックアップ実行中のため強制バックアップ（手動バックアップ）をスキップします.");
                return;
            }

            try
            {
                await Program.BackupManager.ExecuteAsync(true);
            }
            catch (Exception ex)
            {
                SerilogWrapper.Error(ex, "強制バックアップ（手動バックアップ）に失敗しました.");
            }
        }
        /// <summary>
        /// メニュー：　このアプリについて
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 既に表示されている場合はフォーカスを戻す
            if (_formAbout != null)
            {
                _formAbout.WindowState = FormWindowState.Normal; // 最小化されている場合も考慮
                _formAbout.Activate();
                return;
            }

            _formAbout = new FormAbout();
            _formAbout.FormClosing += _formAbout_FormClosing;
            _formAbout.ShowDialog();
        }

        /// <summary>
        /// タスクトレイアイコンがダブルクリックされたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowResultDialog();
        }

        /// <summary>
        /// 結果ダイアログを表示
        /// </summary>
        private void ShowResultDialog()
        {
            // バックアップ中である場合は何もしない
            if (Program.BackupManager.IsExecuting)
                return;

            // 既に表示されている場合はフォーカスを戻す
            if (_formResult != null)
            {
                _formResult.WindowState = FormWindowState.Normal; // 最小化されている場合も考慮
                _formResult.Activate();
                return;
            }

            _formResult = new FormResult();
            _formResult.FormClosing += _formResult_FormClosing;
            _formResult.ShowDialog();
        }

        /// <summary>
        /// 結果ダイアログが閉じられた時の後始末
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _formResult_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _formResult?.Dispose();
            _formResult = null;
        }
        /// <summary>
        /// About ダイアログが閉じられた時の後始末
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _formAbout_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _formAbout?.Dispose();
            _formAbout = null;
        }


    }
}
