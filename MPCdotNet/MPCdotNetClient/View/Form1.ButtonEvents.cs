using System;
using System.Windows.Forms;
namespace MPCdotNet.Client
{
    partial class MainWindow
    {
        private void toolStripButtonPlay_Click(object sender, EventArgs e)
        {
            mpc.SendPlaybackCommand(PlaybackCommand.PlayPause);
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            mpc.SendPlaybackCommand(PlaybackCommand.Stop);
        }

        private void toolStripButtonPrevious_Click(object sender, EventArgs e)
        {
            mpc.SendPlaybackCommand(PlaybackCommand.PreviousTrack);
        }

        private void toolStripButtonNext_Click(object sender, EventArgs e)
        {
            mpc.SendPlaybackCommand(PlaybackCommand.NextTrack);
        }

        private void menuConnect_Click(object sender, EventArgs e)
        {
            mpc.Open();
        }

        private void menuDisconnect_Click(object sender, EventArgs e)
        {
            mpc.Close();
        }

        private void repeatModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mpc.SetRepeat(repeatModeToolStripMenuItem.Checked);
        }

        private void randomModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mpc.SetRandom(randomModeToolStripMenuItem.Checked);
        }

        private void singleModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mpc.SetSingle(singleModeToolStripMenuItem.Checked);
        }

        private void consumeModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mpc.SetConsume(consumeModeToolStripMenuItem.Checked);
        }

        private void output_menuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            var id = (int)item.Tag;
            var state = item.Checked;
            mpc.SetOutput(id, state);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible) this.Hide();
            else this.Show();
        }
    }
}