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
        /// robocopy 実行時の標準エラー出力
        /// </summary>
        public string StdError { get; private set; } = string.Empty;


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
        /// <returns>終了コード</returns>
        /// <exception cref="Exception"></exception>
        public int Execute()
        {
            var exitCode = Helper.ExecuteCommand(Command, out var stdOutput, out var srdError);
            StdOutput = stdOutput;
            StdError = srdError;
            return exitCode;
        }
    }
}
