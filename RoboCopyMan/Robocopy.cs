using System.Diagnostics;

namespace RoboCopyMan
{
    /// <summary>
    /// robocopy ラッパー
    /// </summary>
    internal class Robocopy
    {
        /// <summary>
        /// バックアップ設定
        /// </summary>
        public BackupSetting Setting { get; private init; }

        /// <summary>
        /// ログファイル名 (任意)
        /// </summary>
        public string? LogFilename { get; private init; } = null;
        /// <summary>
        /// 実行コマンド
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// robocopy 実行時の標準出力
        /// </summary>
        public string StdOutput { get; private set; } = string.Empty;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setting">バックアップ設定</param>
        public Robocopy(BackupSetting setting)
        {
            Setting = new(setting);

            Command = Setting.MakeCommand(DateTime.Now, out var logFilename, out var _);
            LogFilename = logFilename;
        }

        /// <summary>
        /// バックアップ(robocopy)を実行する
        /// </summary>
        /// <returns>標準出力</returns>
        /// <exception cref="Exception"></exception>
        public string Execute()
        {
            StdOutput = string.Empty;

            ProcessStartInfo psInfo = new()
            {
                FileName = "cmd",
                Arguments = "/c " + Command,
                CreateNoWindow = true,  // コンソール開かない。
                UseShellExecute = false,  // シェル機能使用しない。
                RedirectStandardOutput = true,  // 標準出力をリダイレクト。
            };

            using Process process = Process.Start(psInfo) ?? throw new Exception();

            // 標準出力を全て取得。
            StdOutput = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Close();

            Debug.WriteLine(StdOutput);
            return StdOutput;
        }
    }
}
