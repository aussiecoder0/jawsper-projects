using System;
using System.ComponentModel;
using System.Windows.Forms;
using LogitechLCD;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace MPCdotNet.Client
{
    public partial class MainWindow : Form
    {
        private MPC mpc;

        public MainWindow(MPC mpc)
        {
            InitializeComponent();

            this.mpc = mpc;

            panelRawCommand1.MPC = mpc;

            notifyIcon1.Text = this.Text;

            playlistView.DataSource = mpc.CurrentPlaylist;

            FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void trackBarWithTimeLabels1_DragComplete(object sender, EventArgs e)
        {
            mpc.SeekCurrent(trackBarWithTimeLabels1.Value);
        }

        private void playlistView_DragOver(object sender, DragEventArgs e)
        {

        }

    }
}
