using System.Diagnostics;

namespace RoboCopyMan
{
    /// <summary>
    /// robocopy ラッパー
    /// </summary>
    internal class Robocopy
    {
        /// <summary>
        /// バックアップ設定
        /// </summary>
        public BackupSetting Setting { get; private init; }

        /// <summary>
        /// ログファイル名
        /// </summary>
        public string LogFile { get; private init; }
        /// <summary>
        /// ログファイルパス
        /// </summary>
        public string LogFilePath { get; private init; }
        /// <summary>
        /// 実行コマンド
        /// </summary>
        public string Command { get; private set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setting">バックアップ設定</param>
        public Robocopy(BackupSetting setting)
        {
            Setting = new(setting);

            LogFile = Path.GetFileName(Setting.DstDir);
            LogFilePath = $"{Setting.LogDir}\\{LogFile}-" + DateTime.Now.ToString(setting.LogDatetimeFmt) + ".txt";
            Command = $"robocopy {Setting.SrcDir} {Setting.DstDir} /LOG:{LogFilePath} /XD {Setting.XdFiles} {Setting.Option}";
        }

        /// <summary>
        /// バックアップ(robocopy)を実行する
        /// </summary>
        /// <returns>標準出力</returns>
        /// <exception cref="Exception"></exception>
        public string Execute()
        {
            ProcessStartInfo psInfo = new()
            {
                FileName = "cmd",
                Arguments = "/c " + Command,
                CreateNoWindow = true,  // コンソール開かない。
                UseShellExecute = false,  // シェル機能使用しない。
                RedirectStandardOutput = true,  // 標準出力をリダイレクト。
            };

            using Process process = Process.Start(psInfo) ?? throw new Exception();

            // 標準出力を全て取得。
            string res = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Close();

            Debug.WriteLine(res);

            return res;
        }
    }
}
