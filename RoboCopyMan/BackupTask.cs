using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboCopyMan
{
    internal class BackupTask
    {
        private bool _initial = true;

        public BackupSetting Setting { get; private init; }

        public DateTime NextTriggerTime { get; set; }

        public bool IsInitial { get { return _initial; } private set { _initial = value; } }
        public Exception? LastException { get; set; } = null;

        public bool IsTimeToBackup
        {
            get
            {
                if (IsInitial)
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
            NextTriggerTime = DateTime.Now.AddMinutes(Setting.IntervalMinutes);
        }

        public bool Execute(bool forced = false, bool updateNextTrigger = true)
        {
            try
            {
                if (!IsInitial && !forced)
                {
                    if (!IsTimeToBackup)
                        return false;
                }

                IsInitial = false;

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
