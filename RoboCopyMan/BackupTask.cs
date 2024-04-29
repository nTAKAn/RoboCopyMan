using System.Diagnostics;

namespace RoboCopyMan
{
    /// <summary>
    /// バックアップタスク
    /// </summary>
    internal class BackupTask
    {
        /// <summary>
        /// 非同期バックアップ防止用のセマフォ
        /// </summary>
        static readonly SemaphoreSlim _semaphore = new(1, 1);
        /// <summary>
        /// 初回フラグ
        /// </summary>
        private bool _initialBackup = true;

        /// <summary>
        /// バックアップ設定を取得する
        /// </summary>
        public BackupSetting Setting { get; private init; }
        /// <summary>
        /// 設定ファイル読み込み時のファイルパスを取得する
        /// </summary>
        public string Filepath { get; private init; }

        /// <summary>
        /// 次回バックアップ時間を取得、設定する
        /// </summary>
        public DateTime NextTriggerTime { get; set; }
        /// <summary>
        /// 初回バックアップかどうかを取得する
        /// </summary>
        public bool IsInitialBackup { get { return _initialBackup; } private set { _initialBackup = value; } }
        /// <summary>
        /// 最後に実行したバックアップ時の例外を取得する（例外が発生していない場合は null）
        /// </summary>
        public Exception? LastException { get; private set; } = null;
        /// <summary>
        /// 最後にバックアップを実行した時間を取得する
        /// </summary>
        public DateTime LastBackupTime { get; private set; } = DateTime.MinValue;

        /// <summary>
        /// 非同期バックアップ中かを調べる
        /// </summary>
#pragma warning disable CA1822 // メンバーを static に設定します ← static にするとわかりにくいと思うので抑制
        public bool IsExecuting { get => _semaphore.CurrentCount == 0; }
#pragma warning restore CA1822 // メンバーを static に設定します
        /// <summary>
        /// バックアップ時間かどうかを調べる
        /// </summary>
        public bool IsTimeToBackup
        {
            get
            {
                if (IsInitialBackup)
                    return true;

                return DateTime.Now >= NextTriggerTime;
            }
        }

        /// <summary>
        /// robocopy 実行時の標準出力を取得する
        /// </summary>
        public string StdOutput { get; private set; } = string.Empty;
        /// <summary>
        /// robocopy 実行時の標準エラーを取得する
        /// </summary>
        public string StdError { get; private set; } = string.Empty;
        /// <summary>
        /// robocopy 実行時の終了コードを取得する
        /// </summary>
        public int ExitCode { get; private set; } = 0;
        /// <summary>
        /// robocopy 実行が成功したかどうか
        /// </summary>
        public bool IsSuccessful { get => Robocopy.CheckExitCode(ExitCode); }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setting">バックアップ設定</param>
        /// <param name="filepath">設定ファイル読み込み時のファイルパス</param>
        public BackupTask(BackupSetting backupSetting, string filepath)
        {
            Setting = new(backupSetting);
            Filepath = filepath;
            UpdateNextTriggerTime();
        }

        /// <summary>
        /// 次回バックアップ時間を更新する
        /// </summary>
        public void UpdateNextTriggerTime()
        {
            if (IsInitialBackup)
            {
                NextTriggerTime = DateTime.Now.AddMinutes(Setting.DelayMinutes);
                IsInitialBackup = false;
            }
            else
                NextTriggerTime = DateTime.Now.AddMinutes(Setting.IntervalMinutes);
        }

        /// <summary>
        /// バックアップを実行する(現在時刻が次回バックアップ未満である場合はバックアップされない)
        /// </summary>
        /// <param name="forced">true: 現在時刻に関わらず強制的にバックアップする, false 通常バックアップ</param>
        /// <param name="updateNextTrigger">true: 次回バックアップ時間を更新する, false 更新しない</param>
        /// <returns>true: バックアップが実行された, false: バックアップは実行されなかった</returns>
        public bool Execute(bool forced = false, bool updateNextTrigger = true)
        {
            // バックアップ時間を超過していない？
            if (!IsTimeToBackup)
            {
                // 強制バックアップ？
                if (forced)
                {
                    // 強制バックアップが無効？
                    if (Setting.DisableForcedBackup)
                    {
                        Debug.WriteLine($"{Setting.Title}: 強制バックアップが 無効 に設定されているため強制バックアップを実行しませんでした.");
                        return false;
                    }
                }
                else
                    return false;
            }

            _semaphore.Wait(); // 非同期ではないのでフラグ代わり

            try
            {
                LastBackupTime = DateTime.Now;

                if (updateNextTrigger)
                    UpdateNextTriggerTime();

                Robocopy robocopy = new(Setting);
                ExitCode = robocopy.Execute();
                StdOutput = robocopy.StdOutput;
                StdError = robocopy.StdError;

                return true;
            }
            catch (Exception ex)
            {
                StdOutput = string.Empty;
                LastException = ex;
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
