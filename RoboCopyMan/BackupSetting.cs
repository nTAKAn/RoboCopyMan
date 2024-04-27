using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomlyn.Model;
using Tomlyn;

namespace RoboCopyMan
{
    public class BackupSetting
    {
        public static readonly string DEFAULT_XD_FILES = "\"System Volume Information\" \"$RECYCLE.BIN\"";
        public static readonly string DEFAULT_OPTION = "/MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE";


        public string SrcDir { get; set; }
        public string DstDir { get; set; }
        public string LogDir { get; set; }
        public string XdFiles { get; private init; } = DEFAULT_XD_FILES;
        public string Option { get; private init; } = DEFAULT_OPTION;
        public long IntervalMinutes { get; set; }
        public long DelayMinutes { get; set; }

        public BackupSetting()
        {
            SrcDir = string.Empty;
            DstDir = string.Empty;
            LogDir = string.Empty;
            IntervalMinutes = -1;
            DelayMinutes = -1;
        }

        public BackupSetting(string srcDir, string dstDir, string logDir, long intervalMin, long delayMin)
        {
            SrcDir = srcDir;
            DstDir = dstDir;
            LogDir = logDir;
            IntervalMinutes = intervalMin;
            DelayMinutes = delayMin;
        }

        public BackupSetting(BackupSetting src)
        {
            SrcDir = src.SrcDir;
            DstDir = src.DstDir;
            LogDir = src.LogDir;
            XdFiles = src.XdFiles;
            Option = src.Option;
            IntervalMinutes = src.IntervalMinutes;
            DelayMinutes = src.DelayMinutes;
        }

        public static BackupSetting Load(string path)
        {
            var tomlString = File.ReadAllText(path);
            var table = Toml.ToModel(tomlString);

            var srcDir = (string)table["srcDir"];
            var dstDir = (string)table["dstDir"];
            var logDir = (string)table["logDir"];

            string xdFiles = table.ContainsKey("xdFiles") ? (string)table["xdFiles"] : DEFAULT_XD_FILES;
            string option = table.ContainsKey("option") ? (string)table["option"] : DEFAULT_OPTION;
            
            var intervalMin = (long)table["intervalMinutes"];
            var delayMin = (long)table["delayMinutes"];

            return new BackupSetting(srcDir, dstDir, logDir, intervalMin, delayMin)
            {
                XdFiles = xdFiles,
                Option = option,
            };
        }
    }
}
