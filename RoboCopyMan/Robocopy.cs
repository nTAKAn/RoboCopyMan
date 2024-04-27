using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboCopyMan
{
    internal class Robocopy
    {
        public BackupSetting Setting { get; private init; }
        public string LogFile { get; private init; }
        public string LogFilePath { get; private init; }
        public string Command { get; private set; }


        public Robocopy(BackupSetting setting)
        {
            Setting = new (setting);

            LogFile = Path.GetFileName(Setting.DstDir);
            LogFilePath = $"{Setting.LogDir}\\{LogFile}-" + DateTime.Now.ToString("yyyyMMdd-HHMMdd") + ".txt";         
            Command = $"robocopy {Setting.SrcDir} {Setting.DstDir} /LOG:{LogFilePath} /XD {Setting.XdFiles} {Setting.Option}";
        }

        public string Execute()
        {
            ProcessStartInfo psInfo = new()
            {
                FileName = "cmd",
                Arguments = "/c " + Command,
                CreateNoWindow = true,  //コンソール開かない。
                UseShellExecute = false,  //シェル機能使用しない。
                RedirectStandardOutput = true,  //標準出力をリダイレクト。
            };

            using Process process = Process.Start(psInfo) ?? throw new Exception();

            //標準出力を全て取得。
            string res = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Close();

            Debug.WriteLine(res);

            return res;
        }
    }
}
