
using Microsoft.Extensions.Configuration;
using Serilog;

namespace RoboCopyMan
{
    internal static class Program
    {
        /// <summary>
        /// �A�v���P�[�V�����̃t���l�[��
        /// </summary>
        private const string APPFULLNAME = "RoboCopyMan";

        /// <summary>
        /// �o�b�N�A�b�v�}�l�[�W��
        /// </summary>
        private static BackupManager? _backupManager = null;
        /// <summary>
        /// �o�b�N�A�b�v�}�l�[�W��
        /// </summary>
        public static BackupManager BackupManager
        {
            get => _backupManager ?? throw new NullReferenceException("�o�b�N�A�b�v�}�l�[�W��������������Ă��܂���.");
        }


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ���d�N����h��
            var mutex = new Mutex(false, APPFULLNAME);
            bool hasHandle;
            try
            {
                hasHandle = mutex.WaitOne(0, false);
            }
            catch (AbandonedMutexException)
            {
                //�ʂ̃A�v���P�[�V�������~���[�e�b�N�X��������Ȃ��ŏI��������
                hasHandle = true;
            }

            if (hasHandle == false)
            {
                //MessageBox.Show("���d�N���͂ł��܂���B");
                return;
            }

            try
            {
                // ���K�[�̏���
                try
                {
                    var configuration = new ConfigurationBuilder().AddJsonFile("serilog_setting.json").Build();
                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"���K�[�̏������Ɏ��s���܂���. [{ex.Message}]");
                    return;
                }

                Log.Information("�A�v���P�[�V�������J�n���܂���.");

                // �o�b�N�A�b�v�ݒ�̓ǂݍ���
                try
                {
                    var settings = BackupManager.LoadSettings("settings");
                    _backupManager = new(settings);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "�o�b�N�A�b�v�ݒ�̓ǂݍ��݂Ɏ��s���܂���.");
                    return;
                }

                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                _ = new FormMain();
                Application.Run();

                Log.Information("�A�v���P�[�V�������I�����܂���.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "�A�v���P�[�V�����̓G���[�ŏI�����܂���.");
            }
            finally
            {
                Log.CloseAndFlush();
                mutex.ReleaseMutex();
            }
        }
    }
}