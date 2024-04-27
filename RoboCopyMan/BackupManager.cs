using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboCopyMan
{
    internal class BackupManager
    {
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


        public delegate void BackupTaskExecutedEventHandler(object sender, EventArgs e);
        public event BackupTaskExecutedEventHandler? BackupTaskExecuted;


        public BackupManager(List<BackupSetting> settings)
        {
            BackupTasks = [];
            foreach (var setting in settings)
            {
                BackupTasks.Add(new(setting));
            }
        }

        public static List<BackupSetting> LoadSettings(string baseDirPath)
        {
            List<BackupSetting> settings = [];
            var files = Directory.GetFiles(baseDirPath);

            foreach (var file in files)
            {
                if (Path.GetExtension(file) != ".toml")
                    continue;

                var setting = BackupSetting.Load(file);
                settings.Add(setting);
            }

            return settings;
        }

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
