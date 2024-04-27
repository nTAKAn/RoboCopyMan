#pragma warning disable IDE1006 // �����X�^�C��

using Serilog;

namespace RoboCopyMan
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// ���ʃ_�C�A���O
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
            // �����͌Ă΂�Ȃ�
        }

        /// <summary>
        /// �o�b�N�A�b�v�����s���邽�߂̃|�[�����O����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, EventArgs e)
        {
            Program.BackupManager.Execute();
        }

        /// <summary>
        /// �o�b�N�A�b�v�^�X�N�����s���ꂽ�Ƃ��̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backupManager_BackupTaskExecuted(object sender, EventArgs e)
        {
            UpdateNotifyIcon();
        }

        /// <summary>
        /// �^�X�N�g���C�̒ʒm�A�C�R���X�V
        /// </summary>
        private void UpdateNotifyIcon()
        {
            if (Program.BackupManager.IsErrorOccured)
            {
                _notifyIcon.Icon = Properties.Resources.gosenzo_error;
                _notifyIcon.Text = "RoboCopyMan (���:�ُ�)" +
                    $"\n����o�b�N�A�b�v {Program.BackupManager.NextBackupTime:HH:mm:ss}";
            }
            else
            {
                _notifyIcon.Icon = Properties.Resources.gosenzo_ok;
                _notifyIcon.Text = "RoboCopyMan (���:����)" +
                    $"\n����o�b�N�A�b�v {Program.BackupManager.NextBackupTime:HH:mm:ss}";
            }
        }

        /// <summary>
        /// ���j���[�F�@�I��
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

                // ���ʃ_�C�A���O���\������Ă���ꍇ�͕���
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
                Log.Error(ex, "�I�������ŃG���[���������܂���.");
            }
            finally
            {
                Close();
                Application.Exit();
            }
        }

        /// <summary>
        /// ���j���[�F�@���ʃ_�C�A���O��\��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showResultDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowResultDialog();
        }
        /// <summary>
        /// ���j���[�F�@�����o�b�N�A�b�v�i�蓮�o�b�N�A�b�v�j
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forcedBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.BackupManager.Execute(true);
        }
        /// <summary>
        /// ���j���[�F�@���̃A�v���ɂ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new FormAbout();
            form.ShowDialog();
        }

        /// <summary>
        /// �^�X�N�g���C�A�C�R�����_�u���N���b�N���ꂽ�Ƃ��̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowResultDialog();
        }

        /// <summary>
        /// ���ʃ_�C�A���O��\��
        /// </summary>
        private void ShowResultDialog()
        {
            // ���ɕ\������Ă���ꍇ�̓t�H�[�J�X��߂�
            if (_formResult != null)
            {
                _formResult.WindowState = FormWindowState.Normal; // �ŏ�������Ă���ꍇ���l��
                _formResult.Activate();
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
