using System.Data;
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
        /// ログファイル名のプレフィックス (任意)
        /// </summary>
        public string? LogFilePrefix { get; set; }
        /// <summary>
        /// ログファイル名に付加する日付フォーマット (任意) (C#の日付フォーマットに準拠)
        /// </summary>
        public string? LogDatetimeFmt { get; set; }
        /// <summary>
        /// 除外するディレクトリ (任意)
        /// </summary>
        public string? XdDirs { get; set; }
        /// <summary>
        /// 除外するファイル (任意)
        /// </summary>
        public string? XfFiles { get; set; }
        /// <summary>
        /// テストモード
        /// </summary>
        public bool TestMode { get; set; }
        /// <summary>
        /// 強制バックアップを無効化
        /// </summary>
        public bool DisableForcedBackup { get; set; }

        /// <summary>
        /// バックアップ前に実行するコマンド
        /// </summary>
        public string? Precoomand { get; set; }
        /// <summary>
        /// バックアップ後に実行するコマンド
        /// </summary>
        public string? Postcommand { get; set; }

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
            LogFilePrefix = null;
            LogDatetimeFmt = null;
            XdDirs = null;
            XfFiles = null;
            TestMode = false;
            DisableForcedBackup = false;

            Precoomand = null;
            Postcommand = null;

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
            LogFilePrefix = src.LogFilePrefix;
            LogDatetimeFmt = src.LogDatetimeFmt;
            XdDirs = src.XdDirs;
            XfFiles = src.XfFiles;
            TestMode = src.TestMode;
            DisableForcedBackup = src.DisableForcedBackup;

            Precoomand = src.Precoomand;
            Postcommand = src.Postcommand;

            IntervalMinutes = src.IntervalMinutes;
            DelayMinutes = src.DelayMinutes;
        }

        /// <summary>
        /// robocopy のコマンドを生成する
        /// </summary>
        /// <param name="logTime">ログファイルに使用する時刻</param>
        /// <param name="logFilePath">生成したログファイルパス</param>
        /// <param name="logFilename">生成したログファイル名</param>
        /// <returns></returns>
        public string MakeCommand(DateTime logTime, out string? logFilePath, out string? logFilename)
        {
            logFilePath = null;
            logFilename = null;
            string options = string.Empty;

            if (TestMode)
                options += "/L ";

            options += Option;

            if ((LogDir is not null) && (LogFilePrefix is not null) && (LogDatetimeFmt is not null))
            {
                logFilename = $"{LogFilePrefix}{logTime.ToString(LogDatetimeFmt)}.txt";
                logFilePath = Path.Join(LogDir, logFilename);

                var logOption = $" /LOG:{logFilePath}";
                options += logOption;
            }

            if (XdDirs is not null)
            {
                var xdFiles = $" /XD {XdDirs}";
                options += xdFiles;
            }
            if (XfFiles is not null)
            {
                var xfFiles = $" /XF {XfFiles}";
                options += xfFiles;
            }

            var command = $"robocopy {SrcDir} {DstDir} {options}";
            return command;
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
            string? logFilePrefix = null;
            string? logDatetimeFmt = null;
            string? xdDirs = null;
            string? xfFiles = null;
            if (table.ContainsKey("logDir"))
            {
                logDir = (string)table["logDir"];
                Debug.WriteLine($"Enable logDir: {logDir}");
            }
            if (table.ContainsKey("logFilePrefix"))
            {
                logFilePrefix = (string)table["logFilePrefix"];
                Debug.WriteLine($"Enable logFilename: {logFilePrefix}");
            }
            if (table.ContainsKey("logDatetimeFmt"))
            {
                logDatetimeFmt = (string)table["logDatetimeFmt"];
                Debug.WriteLine($"Enable logDatetimeFmt: {logDatetimeFmt}");
            }
            if (table.ContainsKey("xdDirs"))
            {
                xdDirs = (string)table["xdDirs"];
                if (string.IsNullOrWhiteSpace(xdDirs))
                    xdDirs = null;
                else
                    Debug.WriteLine($"Enable xdDirs: {xdDirs}");
            }
            if (table.ContainsKey("xfFiles"))
            {
                xfFiles = (string)table["xfFiles"];
                if (string.IsNullOrWhiteSpace(xfFiles))
                    xfFiles = null;
                else
                    Debug.WriteLine($"Enable xfFiles: {xfFiles}");
            }
            bool testMode = false;
            if (table.ContainsKey("testMode"))
            {
                testMode = (bool)table["testMode"];
                Debug.WriteLine($"Enable testMode: {testMode}");
            }
            bool disableForcedBackup = false;
            if (table.ContainsKey("disableForcedBackup"))
            {
                disableForcedBackup = (bool)table["disableForcedBackup"];
                Debug.WriteLine($"Enable disableForcedBackup: {disableForcedBackup}");
            }

            string? precommand = null;
            string? postcommand = null;
            if (table.ContainsKey("precommand"))
            {
                precommand = (string)table["precommand"];
                Debug.WriteLine($"Enable precommand: {precommand}");
            }
            if (table.ContainsKey("postcommand"))
            {
                postcommand = (string)table["postcommand"];
                Debug.WriteLine($"Enable postcommand: {postcommand}");
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
                LogFilePrefix = logFilePrefix,
                LogDatetimeFmt = logDatetimeFmt,
                XdDirs = xdDirs,
                XfFiles = xfFiles,
                TestMode = testMode,
                DisableForcedBackup = disableForcedBackup,

                Precoomand = precommand,
                Postcommand = postcommand,

                IntervalMinutes = intervalMin,
                DelayMinutes = delayMin,
            };
        }
    }
}
