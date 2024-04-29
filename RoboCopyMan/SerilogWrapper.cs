using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics;
using System.Windows.Forms;

namespace RoboCopyMan
{
    /// <summary>
    /// Serilog ラッパー
    /// (Serilog のログの有効/無効を容易に切り替えるためにラッパクラスを作成)
    /// </summary>
    internal static class SerilogWrapper
    {
        /// <summary>
        /// デバッグモード
        /// </summary>
        public static bool EnableDebug { get; private set; } = false;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="debug">true: デバッグモード, false: 通常の Serilog を使用するモード</param>
        /// <param name="configFilePath">Serilog の設定ファイル</param>
        public static void Initialize(bool debug, string configFilePath)
        {
            EnableDebug = debug;

            if (debug)
            {
                Debug.WriteLine($"SerilogWrapper: Debug mode.");
            }
            else
            {
                try
                {
                    var configuration = new ConfigurationBuilder().AddJsonFile(configFilePath).Build();
                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ロガーの初期化に失敗しました. \n[{ex.Message}]", Program.APPFULLNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        /// <summary>
        /// 通常情報ログ
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public static void Information(string message)
        {
            if (EnableDebug)
                Debug.WriteLine($"SerilogWrapper - Information: {message}");
            else
                Log.Information(message);
        }
        /// <summary>
        /// エラーログ
        /// </summary>
        /// <param name="ex">例外</param>
        /// <param name="message">ログメッセージ</param>
        public static void Error(Exception ex, string message)
        {
            if (EnableDebug)
                Debug.WriteLine($"SerilogWrapper - Error: {message} [{ex.Message}]");
            else
                Log.Error(message);
        }
        /// <summary>
        /// エラーログ
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public static void Error(string message)
        {
            if (EnableDebug)
                Debug.WriteLine($"SerilogWrapper - Error: {message}");
            else
                Log.Error(message);
        }
        /// <summary>
        /// 警告ログ
        /// </summary>
        /// <param name="ex">例外</param>
        /// <param name="message">ログメッセージ</param>
        public static void Warning(Exception ex, string message)
        {
            if (EnableDebug)
                Debug.WriteLine($"SerilogWrapper - Warning: {message} [{ex.Message}]");
            else
                Log.Warning(message);
        }
        /// <summary>
        /// 警告ログ
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public static void Warning(string message)
        {
            if (EnableDebug)
                Debug.WriteLine($"SerilogWrapper - Warning: {message}");
            else
                Log.Warning(message);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public static void CloseAndFlush()
        {
            if (!EnableDebug)
                Log.CloseAndFlush();
        }
    }
}
