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
        /// ログファイルの出力先ディレクトリ
        /// </summary>
        public string LogDir { get; set; }
        /// <summary>
        /// ログファイル名に使用する日付フォーマット (C#の日付フォーマットに準拠)
        /// </summary>
        public string LogDatetimeFmt { get; set; }

        /// <summary>
        /// 除外するファイル・ディレクトリ
        /// </summary>
        public string XdFiles { get; set; }
        /// <summary>
        /// robocopy のオプション
        /// </summary>
        public string Option { get; set; }

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
            LogDir = string.Empty;
            LogDatetimeFmt = string.Empty;

            Option = string.Empty;
            XdFiles = string.Empty;

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
            LogDir = src.LogDir;
            LogDatetimeFmt = src.LogDatetimeFmt;

            XdFiles = src.XdFiles;
            Option = src.Option;

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
            var logDir = (string)table["logDir"];
            var logDatetimeFmt = (string)table["logDatetimeFmt"];

            string xdFiles = (string)table["xdFiles"];
            string option = (string)table["option"];

            var intervalMin = (long)table["intervalMinutes"];
            var delayMin = (long)table["delayMinutes"];

            return new BackupSetting()
            {
                Title = title,
                SrcDir = srcDir,
                DstDir = dstDir,
                LogDir = logDir,
                LogDatetimeFmt = logDatetimeFmt,

                XdFiles = xdFiles,
                Option = option,

                IntervalMinutes = intervalMin,
                DelayMinutes = delayMin,
            };
        }
    }
}
