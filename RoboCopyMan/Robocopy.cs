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
        /// robocopy 実行時の終了コード
        /// </summary>
        public int ExitCode { get; private set; } = 0;
        /// <summary>
        /// robocopy 実行が成功したかどうか
        /// </summary>
        public bool IsSuccessful { get => CheckExitCode(ExitCode); }
        /// <summary>
        /// robocopy 実行が成功したかどうか調べる
        /// </summary>
        /// <param name="exitCode">終了コード</param>
        /// <returns></returns>
        public static bool CheckExitCode(int exitCode)
        {
            return exitCode < 8;
        }

        //public static string GetErrorMessage(int exitCode)
        //{

        //    return exitCode switch
        //    {
        //        0 => "コピーする必要がないため、何も実施しなかった. ログの「スキップ(Skipped)」としてカウント.",
        //        1 => "ファイルのコピーが成功した. フォルダーのコピーは含まれません.",
        //        2 => "コピー先にのみ存在するファイル、フォルダが確認された. ログの「Extras」としてカウント.",
        //        4 => "同じ名前で別の種類のファイルが存在した(Mismatched). ログの「不一致(Mismatch)」としてカウント.",
        //        8 => "コピーに失敗した. ログの「失敗(FAILED)」としてカウント.",
        //        16 => "致命的エラー。全く処理できなかったなど。.",
        //        _ => "Unknown error.",
        //    };
        //}

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
            var exitCode = Helper.ExecuteCommand(Command, out var stdOutput, out var stdError);
            StdOutput = stdOutput;
            StdError = stdError;
            ExitCode = exitCode;
            return exitCode;
        }
    }
}
