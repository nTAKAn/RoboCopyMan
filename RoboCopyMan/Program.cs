
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;

namespace RoboCopyMan
{
    internal static class Program
    {
        private const string APPFULLNAME = "RoboCopyMan";

        public static BackupManager BackupManager {  get; private set; }

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
                    MessageBox.Show($"An error occurred while setting up the logger. [{ex.Message}]");
                    return;
                }

                Log.Information("Application is starting.");

                // �o�b�N�A�b�v�ݒ�̓ǂݍ���
                try
                {
                    BackupManager = new(BackupManager.LoadSettings("settings"));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred while loading backup settings.");
                    return;
                }

                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                new FormMain();
                Application.Run();

                Log.Information("Application is closing.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while running the application.");
            }
            finally
            {
                Log.CloseAndFlush();
                mutex.ReleaseMutex();
            }
        }
    }
}