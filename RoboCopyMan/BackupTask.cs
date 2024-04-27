using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboCopyMan
{
    internal class BackupTask
    {
        private bool _initialBackup = true;

        public BackupSetting Setting { get; private init; }
        public DateTime NextTriggerTime { get; set; }
        public bool IsInitialBackup { get { return _initialBackup; } private set { _initialBackup = value; } }
        public Exception? LastException { get; set; } = null;
        public DateTime LastBackupTime { get; set; } = DateTime.MinValue;

        public bool IsTimeToBackup
        {
            get
            {
                if (IsInitialBackup)
                    return true;

                return DateTime.Now >= NextTriggerTime;
            }

        }


        public BackupTask(BackupSetting setting)
        {
            Setting = new(setting);
            UpdateNextTriggerTime();
        }

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
