
using Microsoft.Extensions.Configuration;
using Serilog;

namespace RoboCopyMan
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのフルネーム
        /// </summary>
        private const string APPFULLNAME = "RoboCopyMan";

        /// <summary>
        /// バックアップマネージャ
        /// </summary>
        private static BackupManager? _backupManager = null;
        /// <summary>
        /// バックアップマネージャ
        /// </summary>
        public static BackupManager BackupManager
        {
            get => _backupManager ?? throw new NullReferenceException("バックアップマネージャが初期化されていません.");
        }


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
                    MessageBox.Show($"ロガーの初期化に失敗しました. [{ex.Message}]");
                    return;
                }

                Log.Information("アプリケーションを開始しました.");

                // バックアップ設定の読み込み
                try
                {
                    var settings = BackupManager.LoadSettings("settings");
                    _backupManager = new(settings);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "バックアップ設定の読み込みに失敗しました.");
                    return;
                }

                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                _ = new FormMain();
                Application.Run();

                Log.Information("アプリケーションを終了しました.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "アプリケーションはエラーで終了しました.");
            }
            finally
            {
                Log.CloseAndFlush();
                mutex.ReleaseMutex();
            }
        }
    }
}