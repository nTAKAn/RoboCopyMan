#pragma warning disable IDE1006 // �����X�^�C��

namespace RoboCopyMan
{
    public partial class FormMain : Form
    {
        private FormResult? _formResult;

        public FormMain()
        {
            ShowInTaskbar = false;
            InitializeComponent();

            _timer.Start();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // �����͌Ă΂�Ȃ�
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

            if (Program.BackupManager.IsErrorOccured)
            {
                _notifyIcon.Icon = Properties.Resources.gosenzo_error;
                _notifyIcon.Text = "RoboCopyMan (���:Error)" +
                    $"\n����o�b�N�A�b�v {Program.BackupManager.NextBackupTime:HH:mm:ss}";
            }
            else
            {
                _notifyIcon.Icon = Properties.Resources.gosenzo_ok;
                _notifyIcon.Text = "RoboCopyMan (���:OK)" +
                    $"\n����o�b�N�A�b�v {Program.BackupManager.NextBackupTime:HH:mm:ss}";
            }
        }

        private void showResultDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowResultDialog();
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
        /// ���ʃ_�C�A���O������ꂽ���̌�n��
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
