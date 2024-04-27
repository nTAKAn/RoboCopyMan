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
        public List<BackupTask> Tasks { get; private init; }

        /// <summary>
        /// エラーが発生しているかを調べる
        /// </summary>
        public bool IsErrorOccured
        {
            get
            {
                foreach (var task in Tasks)
                {
                    if (task.LastException != null)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 直近の次回バックアップ時間
        /// </summary>
        public DateTime NextBackupTime
        {
            get
            {
                DateTime next = DateTime.MaxValue;
                foreach (var task in Tasks)
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
        public int TaskCount { get => Tasks.Count; }



        public BackupManager(List<BackupSetting> settings)
        {
            Tasks = [];
            foreach (var setting in settings)
            {
                Tasks.Add(new(setting));
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

        public void Execute()
        {
            foreach (var task in Tasks)
            {
                try
                {
                    if (task.IsTimeToBackup)
                    {
                        if (task.Execute())
                        {
                            Log.Information($"Backup task executed. {task.Setting.SrcDir} -> {task.Setting.DstDir}");
                            Log.Information($"Next trigger time: {task.NextTriggerTime}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"An error occurred while executing backup task. {task.Setting.SrcDir} -> {task.Setting.DstDir}");
                }
            }
        }
    }
}
