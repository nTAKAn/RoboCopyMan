using System.Diagnostics;

#pragma warning disable IDE1006 // 命名スタイル

namespace RoboCopyMan
{
    public partial class FormAbout : Form
    {
        private static readonly string _GITHUB_URL = "https://github.com/nTAKAn/RoboCopyMan";
        public FormAbout()
        {
            InitializeComponent();
        }

        private void _linkLabel_github_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo pi = new()
                {
                    FileName = _GITHUB_URL,
                    UseShellExecute = true, // 重要
                };
                Process.Start(pi);
            }
            catch (Exception ex)
            {
                SerilogWrapper.Error(ex, "リンク先を開けませんでした.");
            }
        }
    }
}
