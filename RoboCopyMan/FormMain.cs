#pragma warning disable IDE1006 // �����X�^�C��

using System.Diagnostics;

namespace RoboCopyMan
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// ���ʃ_�C�A���O
        /// </summary>
        private FormResult? _formResult;
        /// <summary>
        /// About �_�C�A���O
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
            // �����͌Ă΂�Ȃ�
        }

        /// <summary>
        /// �o�b�N�A�b�v�����s���邽�߂̃|�[�����O����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void _timer_Tick(object sender, EventArgs e)
        {
            if (Program.BackupManager.IsExecuting)
            {
                Debug.WriteLine("�o�b�N�A�b�v���s���̂��߃|�[�����O�����ł̃o�b�N�A�b�v���X�L�b�v���܂�.");
                return;
            }

            try
            {
                await Program.BackupManager.ExecuteAsync();
            }
            catch (Exception ex)
            {
                SerilogWrapper.Error(ex, "�|�[�����O�����ł̃o�b�N�A�b�v�Ɏ��s���܂���.");
            }
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
        /// �o�b�N�A�b�v�J�n���̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _backupManager_BeginBackupEventHandler(object sender, EventArgs e)
        {
            // �o�b�N�A�b�v���s���̓��j���[�𖳌���
            _contextMenuStrip.Enabled = false;

            UpdateNotifyIcon();
        }
        /// <summary>
        /// �o�b�N�A�b�v�I�����̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _backupManager_EndBackupEventHandler(object sender, EventArgs e)
        {
            // �o�b�N�A�b�v���I�������烁�j���[��L����
            _contextMenuStrip.Enabled = true;

            UpdateNotifyIcon();
        }

        /// <summary>
        /// �^�X�N�g���C�̒ʒm�A�C�R���X�V
        /// </summary>
        private void UpdateNotifyIcon()
        {
            // �o�b�N�A�b�v���s���̏ꍇ
            if (Program.BackupManager.IsExecuting)
            {
                _notifyIcon.Icon = Properties.Resources.gosenzo_other;
                _notifyIcon.Text = "RoboCopyMan (���:�o�b�N�A�b�v��)";
            }
            else
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
        }

        /// <summary>
        /// �A�v���P�[�V�������I������
        /// </summary>
        /// <param name="restart">true: �ċN������</param>
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
                //    Debug.WriteLine("�o�b�N�A�b�v���̂��ߏI����ҋ@");
                //}

                // ���ʃ_�C�A���O���\������Ă���ꍇ�͕���
                if (_formResult != null)
                {
                    Program.BackupManager.BackupTaskExecuted -= _backupManager_BackupTaskExecuted;
                    _formResult.Close();
                    _formResult = null;
                }

                // About �_�C�A���O���\������Ă���ꍇ�͕���
                if (_formAbout != null)
                {
                    _formAbout.Close();
                    _formAbout = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.APPFULLNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                SerilogWrapper.Error(ex, "�I�������ŃG���[���������܂���.");
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
        /// ���j���[�F�@�I��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppExit(false);
        }
        /// <summary>
        /// ���j���[�F�@�ċN��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("�ċN�����܂����H", Program.APPFULLNAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            AppExit(true);
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
        private async void forcedBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.BackupManager.IsExecuting)
            {
                Debug.WriteLine("�o�b�N�A�b�v���s���̂��ߋ����o�b�N�A�b�v�i�蓮�o�b�N�A�b�v�j���X�L�b�v���܂�.");
                return;
            }

            try
            {
                await Program.BackupManager.ExecuteAsync(true);
            }
            catch (Exception ex)
            {
                SerilogWrapper.Error(ex, "�����o�b�N�A�b�v�i�蓮�o�b�N�A�b�v�j�Ɏ��s���܂���.");
            }
        }
        /// <summary>
        /// ���j���[�F�@���̃A�v���ɂ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ���ɕ\������Ă���ꍇ�̓t�H�[�J�X��߂�
            if (_formAbout != null)
            {
                _formAbout.WindowState = FormWindowState.Normal; // �ŏ�������Ă���ꍇ���l��
                _formAbout.Activate();
                return;
            }

            _formAbout = new FormAbout();
            _formAbout.FormClosing += _formAbout_FormClosing;
            _formAbout.ShowDialog();
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
            // �o�b�N�A�b�v���ł���ꍇ�͉������Ȃ�
            if (Program.BackupManager.IsExecuting)
                return;

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
        /// <summary>
        /// About �_�C�A���O������ꂽ���̌�n��
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
