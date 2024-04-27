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

        public BackupManager(List<BackupSetting> settings)
        {
            Tasks = new();
            foreach (var setting in settings)
            {
                Tasks.Add(new(setting));
            }
        }

        public static List<BackupSetting> LoadSettings(string baseDirPath)
        {
            List<BackupSetting> settings = new();
            var files = Directory.GetFiles(baseDirPath);

            foreach (var file in files)
            {
                if (Path.GetExtension(file) != ".toml")
                {
                    continue;
                }

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
