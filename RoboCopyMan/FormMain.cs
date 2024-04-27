#pragma warning disable IDE1006 // 命名スタイル

using Serilog;

namespace RoboCopyMan
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// 結果ダイアログ
        /// </summary>
        private FormResult? _formResult;

        public FormMain()
        {
            ShowInTaskbar = false;
            InitializeComponent();

            UpdateNotifyIcon();
            Program.BackupManager.BackupTaskExecuted += _backupManager_BackupTaskExecuted;

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
        private void _timer_Tick(object sender, EventArgs e)
        {
            Program.BackupManager.Execute();
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
        /// タスクトレイの通知アイコン更新
        /// </summary>
        private void UpdateNotifyIcon()
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

        /// <summary>
        /// メニュー：　終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _timer.Stop();
                _timer.Dispose();

                _notifyIcon.Visible = false;

                // 結果ダイアログが表示されている場合は閉じる
                if (_formResult != null)
                {
                    Program.BackupManager.BackupTaskExecuted -= _backupManager_BackupTaskExecuted;
                    _formResult.Close();
                    _formResult = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex, "終了処理でエラーが発生しました.");
            }
            finally
            {
                Close();
                Application.Exit();
            }
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
        private void forcedBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.BackupManager.Execute(true);
        }
        /// <summary>
        /// メニュー：　このアプリについて
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new FormAbout();
            form.ShowDialog();
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

        
    }
}
