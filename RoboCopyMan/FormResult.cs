#pragma warning disable IDE1006 // 命名スタイル

namespace RoboCopyMan
{
    public partial class FormResult : Form
    {

        public FormResult()
        {
            InitializeComponent();
        }

        private void FormResult_Load(object sender, EventArgs e)
        {
            UpdateResults();
            _timer.Start();
        }

        private void FormResult_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FormResult_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void UpdateResults()
        {
            _listView.Items.Clear();

            var tasks = Program.BackupManager.Tasks;
            foreach (var task in tasks)
            {
                var item = new ListViewItem(task.Setting.Title);
                item.SubItems.Add(task.LastBackupTime.ToString("yyyy/MM/dd HH:mm:ss"));
                item.SubItems.Add(task.NextTriggerTime.ToString("yyyy/MM/dd HH:mm:ss"));
                item.SubItems.Add(task.LastException?.Message ?? "成功しました");
                _listView.Items.Add(item);
            }
        }

        private void _listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            UpdateResults();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        
    }
}
