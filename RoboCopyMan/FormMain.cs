namespace RoboCopyMan
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            this.ShowInTaskbar = false;
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();

            _notifyIcon.Visible = false;

            Close();
            Application.Exit();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Program.BackupManager.Execute();
        }
    }
}
