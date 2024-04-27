#pragma warning disable IDE1006 // 命名スタイル

namespace RoboCopyMan
{
    public partial class FormMain : Form
    {
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


        void _backupManager_BackupTaskExecuted(object sender, EventArgs e)
        {
            UpdateNotifyIcon();
        }

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


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();

            _notifyIcon.Visible = false;

            if (_formResult != null)
            {
                _formResult.Close();
                _formResult.Dispose();
                _formResult = null;
            }

            Close();
            Application.Exit();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Program.BackupManager.Execute();
        }

        private void showResultDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowResultDialog();
        }

        private void forcedBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.BackupManager.Execute(true);
        }

        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowResultDialog();
        }

        private void ShowResultDialog()
        {
            if (_formResult != null)
            {
                _formResult.TopMost = true;
                _formResult.TopMost = false;
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
