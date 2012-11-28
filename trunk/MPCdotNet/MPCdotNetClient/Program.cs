using System;
using System.Windows.Forms;

namespace MPCdotNet.Client
{
    public partial class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new Program();
        }

        private KeyboardListener m_KeyboardListener;
        private MPC mpc;
        private LCD lcd;
        private Timer timer;

        private MainWindow window;

        public Program()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 100;

            m_KeyboardListener = new KeyboardListener(Keys.MediaPlayPause, Keys.MediaStop, Keys.MediaPreviousTrack, Keys.MediaNextTrack);
            m_KeyboardListener.KeyDown += m_KeyboardListener_KeyDown;

            mpc = new MPC(Properties.Settings.Default.Server1);

            lcd = new LCD("MPCdotNet");
            lcd.SetPlaybackState(PlaybackState.Stopped);
            lcd.Artist = "";
            lcd.Title = "";
            lcd.Album = "";
            lcd.TrackTime = 0;
            lcd.TrackLength = 0;

            window = new MainWindow(mpc);
            mpc.OnConnectionStateChanged += window.mpc_onConnectionStateChange;


            if (Properties.Settings.Default.AutoConnect)
            {
                mpc.Open();
            }
            timer.Start();

            Application.Run(window);

            mpc.Close();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mpc.Connected)
            {
                if (mpc.Update())
                {
                    window.Invoke((MethodInvoker)delegate() { window.mpc_OnUpdate(mpc, null); });
                    if (mpc.PreviousStatus == null || mpc.PreviousStatus.SongID != mpc.CurrentStatus.SongID)
                    {
                        mpc.UpdateCurrentSongInfo();

                        lcd.Artist = mpc.CurrentSong.Artist;
                        lcd.Title = mpc.CurrentSong.Title;
                        lcd.Album = mpc.CurrentSong.Album;
                        window.Invoke((MethodInvoker)delegate()
                        {
                            window.mpc_OnTrackChange(mpc, null);
                        });
                    }
                    if (mpc.PreviousStatus == null || mpc.PreviousStatus.PlaylistVersion != mpc.CurrentStatus.PlaylistVersion)
                    {
                        mpc.UpdateCurrentPlaylist();
                        window.Invoke((MethodInvoker)delegate()
                        {
                            window.mpc_OnPlaylistChange(mpc, null);
                        });
                    }
                    if (mpc.CurrentStatus.PlaybackState == PlaybackState.Stopped)
                    {
                        lcd.PlaylistPosition = -1;
                        lcd.PlaylistLength = -1;
                        lcd.TrackTime = -1;
                        lcd.TrackLength = -1;
                        lcd.SetProgress(0, 0, 0);
                    }
                    else
                    {
                        lcd.PlaylistPosition = mpc.CurrentStatus.Song;
                        lcd.PlaylistLength = mpc.CurrentStatus.PlaylistLength;
                        lcd.TrackTime = mpc.CurrentStatus.Time[0];
                        lcd.TrackLength = mpc.CurrentStatus.Time[1];
                        lcd.SetProgress((int)mpc.CurrentStatus.Elapsed, 0, mpc.CurrentStatus.Time[1] * 1000);
                    }
                }
                lcd.SetPlaybackState(mpc.CurrentStatus.PlaybackState);
            }

            lcd.Update();
        }

        void m_KeyboardListener_KeyDown(object sender, RawKeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.MediaPlayPause:
                    mpc.SendPlaybackCommand(PlaybackCommand.PlayPause);
                    break;
                case Keys.MediaPreviousTrack:
                    mpc.SendPlaybackCommand(PlaybackCommand.PreviousTrack);
                    break;
                case Keys.MediaNextTrack:
                    mpc.SendPlaybackCommand(PlaybackCommand.NextTrack);
                    break;
                case Keys.MediaStop:
                    mpc.SendPlaybackCommand(PlaybackCommand.Stop);
                    break;
            }
        }
    }

    public static class EventExtensions
    {
        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            if (handler != null) handler(sender, args);
        }
    }

    public enum MPCEventType
    {
        Connection, Status, PlaylistChange
    }
    public class MPCEvent
    {
        public MPC MPC { get; set; }
        public MPCEventType Event { get; set; }
        public object Data { get; set; }
    }
}
