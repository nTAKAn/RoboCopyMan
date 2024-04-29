using Serilog;
using System.Diagnostics;

namespace RoboCopyMan
{
    /// <summary>
    /// バックアップマネージャ
    /// </summary>
    internal class BackupManager
    {
        /// <summary>
        /// サンプル設定ファイル名
        /// </summary>
        private static readonly string _SAMPLE_SETTING_FILE = "sample.toml";

        /// <summary>
        /// 非同期バックアップ防止用のセマフォ
        /// </summary>
        static readonly SemaphoreSlim _semaphore = new(1, 1);


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

                    if (!task.IsSuccessful)
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
        /// 非同期バックアップ中かを調べる
        /// </summary>
#pragma warning disable CA1822 // メンバーを static に設定します ← static にするとわかりにくいと思うので抑制
        public bool IsExecuting { get => _semaphore.CurrentCount == 0; }
#pragma warning restore CA1822 // メンバーを static に設定します
        /// <summary>
        /// バックアップ時間を超過しているバックアップタスクが存在するかを調べる
        /// </summary>
        public bool IsTimeToBackupTasks { get => BackupTasks.Any(task => task.IsTimeToBackup); }
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
        /// バックアップ開始時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void BeginBackupEventHandler(object sender, EventArgs e);
        /// <summary>
        /// バックアップ開始時に発生するイベント
        /// </summary>
        public event BeginBackupEventHandler? BeginBackup;
        /// <summary>
        /// バックアップ終了時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EndBackupEventHandler(object sender, EventArgs e);
        /// <summary>
        /// バックアップ終了時に発生するイベント
        /// </summary>
        public event EndBackupEventHandler? EndBackup;

        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settings">(バックアップ設定, 設定ファイルのパス) を要素とするリスト</param>
        public BackupManager(List<(BackupSetting, string)> settings)
        {
            BackupTasks = [];
            foreach (var setting in settings)
            {
                var (backupSetting, filepath) = setting;
                BackupTasks.Add(new(backupSetting, filepath));
            }
        }

        /// <summary>
        /// 指定したディレクトリからバックアップ設定ファイルを読み込む
        /// </summary>
        /// <param name="baseDirPath">バックアップ設定ファイルが格納されたディレクトリパス</param>
        /// <returns>(読み込んだバックアップ設定, 設定ファイルのパス) を要素とするリスト</returns>
        public static List<(BackupSetting, string)> LoadSettings(string baseDirPath)
        {
            List<(BackupSetting, string)> settings = [];
            var filepaths = Directory.GetFiles(baseDirPath);

            foreach (var filepath in filepaths)
            {
                if (Path.GetExtension(filepath) != ".toml")
                    continue;

                if (Path.GetFileName(filepath) == _SAMPLE_SETTING_FILE)
                    continue;

                try
                {
                    var backupSetting = BackupSetting.Load(filepath);
                    settings.Add((backupSetting, filepath));
                }
                catch (Exception ex)
                {
                    // 読み込めない場合でもログを残して継続する
                    SerilogWrapper.Error(ex, $"設定ファイルの読み込み中に例外が発生しました. {filepath}");
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
                        SerilogWrapper.Information($"{task.Setting.Title}: バックアップを実行しました. {task.Setting.SrcDir} -> {task.Setting.DstDir}");
                        SerilogWrapper.Information($"{task.Setting.Title}: 次回バックアップ時間: {task.NextTriggerTime}");
                        backupExecuted = true;
                    }
                }
                catch (Exception ex)
                {
                    SerilogWrapper.Error(ex, $"{task.Setting.Title}: バックアップ中に例外が発生しました. {task.Setting.SrcDir} -> {task.Setting.DstDir}");
                }
            }

            if (backupExecuted)
                BackupTaskExecuted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 非同期バックアップを実行する
        /// </summary>
        /// <param name="forced">true: 現在時刻に関わらず強制的にバックアップする, false 通常バックアップ</param>
        /// <returns></returns>
        public async Task ExecuteAsync(bool forced = false)
        {
            if (!_semaphore.Wait(0))
                throw new BackupIsAlreadyRunningException();

            // バックアップ開始イベントを発生させる
            bool backupExecuted = false;
            if (forced || IsTimeToBackupTasks)
            {
                BeginBackup?.Invoke(this, EventArgs.Empty);
                backupExecuted = true;
            }
            
            try
            {
                // HACK: キャンセルトークンを渡す処理を検討する
                await Task.Run(() => Execute(forced));
            }
            finally
            {
                _semaphore.Release();

                // バックアップ終了イベントを発生させる
                if (backupExecuted)
                    EndBackup?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// バックアップが既に実行中の場合に発生する例外
    /// </summary>
    public class BackupIsAlreadyRunningException : Exception
    {
        public BackupIsAlreadyRunningException() : base("バックアップは既に実行中です.") { }
    }
}
