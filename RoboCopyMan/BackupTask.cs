namespace RoboCopyMan
{
    /// <summary>
    /// バックアップタスク
    /// </summary>
    internal class BackupTask
    {
        /// <summary>
        /// 初回フラグ
        /// </summary>
        private bool _initialBackup = true;

        /// <summary>
        /// バックアップ設定を取得する
        /// </summary>
        public BackupSetting Setting { get; private init; }
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
        /// コンストラクタ
        /// </summary>
        /// <param name="setting">バックアップ設定</param>
        public BackupTask(BackupSetting setting)
        {
            Setting = new(setting);
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
            if (!IsTimeToBackup && !forced)
                return false;

            try
            {
                LastBackupTime = DateTime.Now;

                if (updateNextTrigger)
                    UpdateNextTriggerTime();

                Robocopy robocopy = new(Setting);
                robocopy.Execute();

                return true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                throw;
            }
        }
    }
}
