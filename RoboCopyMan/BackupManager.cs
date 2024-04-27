using Serilog;

namespace RoboCopyMan
{
    /// <summary>
    /// バックアップマネージャ
    /// </summary>
    internal class BackupManager
    {
        /// <summary>
        /// バックアップタスクリスト
        /// </summary>
        public List<BackupTask> BackupTasks { get; private init; }

        /// <summary>
        /// エラーが発生しているかを調べる
        /// </summary>
        public bool IsErrorOccured
        {
            get
            {
                foreach (var task in BackupTasks)
                {
                    if (task.LastException != null)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 次回バックアップ時間
        /// </summary>
        public DateTime NextBackupTime
        {
            get
            {
                DateTime next = DateTime.MaxValue;
                foreach (var task in BackupTasks)
                {
                    if (task.NextTriggerTime < next)
                        next = task.NextTriggerTime;
                }

                return next;
            }
        }

        /// <summary>
        /// バックアップタスクの数
        /// </summary>
        public int TaskCount { get => BackupTasks.Count; }

        /// <summary>
        /// バックアップタスクが実行されたときに発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void BackupTaskExecutedEventHandler(object sender, EventArgs e);
        /// <summary>
        /// バックアップタスクが実行されたときに発生するイベント
        /// </summary>
        public event BackupTaskExecutedEventHandler? BackupTaskExecuted;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settings">バックアップ設定のリスト</param>
        public BackupManager(List<BackupSetting> settings)
        {
            BackupTasks = [];
            foreach (var setting in settings)
            {
                BackupTasks.Add(new(setting));
            }
        }

        /// <summary>
        /// 指定したディレクトリからバックアップ設定ファイルを読み込む
        /// </summary>
        /// <param name="baseDirPath">バックアップ設定ファイルが格納されたディレクトリパス</param>
        /// <returns></returns>
        public static List<BackupSetting> LoadSettings(string baseDirPath)
        {
            List<BackupSetting> settings = [];
            var files = Directory.GetFiles(baseDirPath);

            foreach (var file in files)
            {
                if (Path.GetExtension(file) != ".toml")
                    continue;

                try
                {
                    var setting = BackupSetting.Load(file);
                    settings.Add(setting);
                }
                catch (Exception ex)
                {
                    // 読み込めない場合でもログを残して継続する
                    Log.Error(ex, $"設定ファイルの読み込み中に例外が発生しました. {file}");
                }
            }

            return settings;
        }

        /// <summary>
        /// バックアップを実行する
        /// </summary>
        /// <param name="forced">true: 現在時刻に関わらず強制的にバックアップする, false 通常バックアップ</param>
        public void Execute(bool forced = false)
        {
            bool backupExecuted = false;

            foreach (var task in BackupTasks)
            {
                try
                {
                    if (task.Execute(forced))
                    {
                        Log.Information($"{task.Setting.Title}: バックアップを実行しました. {task.Setting.SrcDir} -> {task.Setting.DstDir}");
                        Log.Information($"{task.Setting.Title}: 次回バックアップ時間: {task.NextTriggerTime}");
                        backupExecuted = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{task.Setting.Title}: バックアップ中に例外が発生しました. {task.Setting.SrcDir} -> {task.Setting.DstDir}");
                }


            }

            if (backupExecuted)
                BackupTaskExecuted?.Invoke(this, EventArgs.Empty);
        }
    }
}
