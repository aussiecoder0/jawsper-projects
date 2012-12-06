using System.Windows.Forms;
using System;
using System.Drawing;
namespace MPCdotNet.Client
{
    partial class MainWindow
    {
        public void mpc_onConnectionStateChange(object sender, EventArgs e)
        {
            var mpc = sender as MPC;

            menuConnect.Enabled = !mpc.Connected;
            menuDisconnect.Enabled = mpc.Connected;

            menuControl.Enabled
                = menuServer.Enabled
                = menuItemNext.Enabled
                = menuItemPrevious.Enabled
                = menuItemPlay.Enabled
                = menuItemStop.Enabled
                = mpc.Connected;

            menuOutputs.DropDownItems.Clear();
            if (mpc.Connected)
            {
                foreach (var output in mpc.Outputs)
                {
                    var item = menuOutputs.DropDownItems.Add(output.Name, null, output_menuItem_Click) as ToolStripMenuItem;
                    item.Checked = output.Enabled;
                    item.CheckOnClick = true;
                    item.Tag = output.ID;
                }
            }
            else
            {
                trackBarWithTimeLabels1.Value = 0;
                trackBarWithTimeLabels1.Maximum = 0;
                mpc_OnTrackChange(sender, e);
            }
        }

        public void mpc_OnUpdate(object sender, EventArgs e)
        {
            var mpc = sender as MPC;

            muteToolStripMenuItem.Checked = mpc.CurrentStatus.Volume == 0;
            repeatModeToolStripMenuItem.Checked = mpc.CurrentStatus.Repeat;
            randomModeToolStripMenuItem.Checked = mpc.CurrentStatus.Random;
            singleModeToolStripMenuItem.Checked = mpc.CurrentStatus.Single;
            consumeModeToolStripMenuItem.Checked = mpc.CurrentStatus.Consume;

            if (mpc.CurrentStatus.PlaybackState == PlaybackState.Playing || mpc.CurrentStatus.PlaybackState == PlaybackState.Paused)
            {
                trackBarWithTimeLabels1.Maximum = mpc.CurrentStatus.Time.Total;
                trackBarWithTimeLabels1.Value = mpc.CurrentStatus.Time.Current;
            }
            else
            {
                trackBarWithTimeLabels1.Value = 0;
            }
        }
        public void mpc_OnTrackChange(object sender, EventArgs e)
        {
            var mpc = sender as MPC;
            playlistView.Invalidate();

            trackInfo1.Track = mpc.CurrentSong.Title;
            trackInfo1.Artist = mpc.CurrentSong.Artist;
            tabPanelNowPlaying.Title = mpc.CurrentSong.Title;
            tabPanelNowPlaying.Artist = mpc.CurrentSong.Artist;
            tabPanelNowPlaying.Album = mpc.CurrentSong.Album;
        }

        public void mpc_OnPlaylistChange(object sender, EventArgs e)
        {
            playlistView.Invalidate();
        }

        private void playlistView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < mpc.CurrentPlaylist.Count)
            {
                if (mpc.CurrentPlaylist[e.RowIndex].ID == mpc.CurrentSong.ID)
                {
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
            }
        }
    }
}