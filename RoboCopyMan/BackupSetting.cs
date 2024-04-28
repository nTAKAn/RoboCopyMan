using System.Diagnostics;
using Tomlyn;

namespace RoboCopyMan
{
    /// <summary>
    /// バックアップ設定
    /// </summary>
    public class BackupSetting
    {
        /// <summary>
        /// バックアップ設定のタイトル
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// バックアップ元ディレクトリ
        /// </summary>
        public string SrcDir { get; set; }
        /// <summary>
        /// バックアップ先ディレクトリ
        /// </summary>
        public string DstDir { get; set; }
        /// <summary>
        /// robocopy のオプション
        /// </summary>
        public string Option { get; set; }

        /// <summary>
        /// ログファイルの出力先ディレクトリ (任意)
        /// </summary>
        public string? LogDir { get; set; }
        /// <summary>
        /// ログファイル名 (任意)
        /// </summary>
        public string? LogFilename { get; set; }
        /// <summary>
        /// ログファイル名に付加する日付フォーマット (任意) (C#の日付フォーマットに準拠)
        /// </summary>
        public string? LogDatetimeFmt { get; set; }
        /// <summary>
        /// 除外するファイル・ディレクトリ (任意)
        /// </summary>
        public string? XdFiles { get; set; }

        /// <summary>
        /// バックアップ間隔（分）
        /// </summary>
        public long IntervalMinutes { get; set; }
        /// <summary>
        /// 初回実行を遅らせるディレイ（分）
        /// </summary>
        public long DelayMinutes { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BackupSetting()
        {
            Title = string.Empty;
            SrcDir = string.Empty;
            DstDir = string.Empty;
            Option = string.Empty;

            LogDir = null;
            LogFilename = null;
            LogDatetimeFmt = null;
            XdFiles = null;

            IntervalMinutes = -1;
            DelayMinutes = -1;
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="src">コピー元</param>
        public BackupSetting(BackupSetting src)
        {
            Title = src.Title;
            SrcDir = src.SrcDir;
            DstDir = src.DstDir;
            Option = src.Option;

            LogDir = src.LogDir;
            LogFilename = src.LogFilename;
            LogDatetimeFmt = src.LogDatetimeFmt;
            XdFiles = src.XdFiles;

            IntervalMinutes = src.IntervalMinutes;
            DelayMinutes = src.DelayMinutes;
        }

        /// <summary>
        /// TOMLファイルから設定を読み込む
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>読み込んだ設定ファイル</returns>
        public static BackupSetting Load(string path)
        {
            var tomlString = File.ReadAllText(path);
            var table = Toml.ToModel(tomlString);

            var title = (string)table["title"];
            var srcDir = (string)table["srcDir"];
            var dstDir = (string)table["dstDir"];
            string option = (string)table["option"];

            string? logDir = null;
            string? logFilename = null;
            string? logDatetimeFmt = null;
            string? xdFiles = null;
            if (table.ContainsKey("logDir"))
            {
                logDir = (string)table["logDir"];
                Debug.WriteLine($"Enable logDir: {logDir}");
            }
            if (table.ContainsKey("logFilename"))
            {
                logFilename = (string)table["logFilename"];
                Debug.WriteLine($"Enable logFilename: {logFilename}");
            }
            if (table.ContainsKey("logDatetimeFmt"))
            {
                logDatetimeFmt = (string)table["logDatetimeFmt"];
                Debug.WriteLine($"Enable logDatetimeFmt: {logDatetimeFmt}");
            }
            if (table.ContainsKey("xdFiles"))
            {
                xdFiles = (string)table["xdFiles"];
                Debug.WriteLine($"Enable xdFiles: {xdFiles}");
            }

            var intervalMin = (long)table["intervalMinutes"];
            var delayMin = (long)table["delayMinutes"];

            return new BackupSetting()
            {
                Title = title,
                SrcDir = srcDir,
                DstDir = dstDir,
                Option = option,

                LogDir = logDir,
                LogFilename = logFilename,
                LogDatetimeFmt = logDatetimeFmt,
                XdFiles = xdFiles,

                IntervalMinutes = intervalMin,
                DelayMinutes = delayMin,
            };
        }
    }
}
