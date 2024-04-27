using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

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
                ProcessStartInfo pi = new ProcessStartInfo()
                {
                    FileName = _GITHUB_URL,
                    UseShellExecute = true, // 重要
                };
                Process.Start(pi);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
