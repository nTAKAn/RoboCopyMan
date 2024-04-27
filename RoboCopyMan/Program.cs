
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
            // 多重起動を防ぐ
            var mutex = new Mutex(false, APPFULLNAME);
            bool hasHandle;
            try
            {
                hasHandle = mutex.WaitOne(0, false);
            }
            catch (AbandonedMutexException)
            {
                //別のアプリケーションがミューテックスを解放しないで終了した時
                hasHandle = true;
            }

            if (hasHandle == false)
            {
                //MessageBox.Show("多重起動はできません。");
                return;
            }

            try
            {
                // ロガーの準備
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

                // バックアップ設定の読み込み
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