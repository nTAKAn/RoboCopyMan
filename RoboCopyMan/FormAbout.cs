using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                System.Diagnostics.Process.Start(_GITHUB_URL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
