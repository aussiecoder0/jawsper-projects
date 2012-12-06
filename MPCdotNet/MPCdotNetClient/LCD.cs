using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogitechLCD;
using System.Windows.Forms;

namespace MPCdotNet.Client
{
    public class LCD : LCDDisplay
    {
        private const int ID_PROGRESS_BAR = -1;

        private const int ID_ARTIST = 0;
        private const int ID_PL_DISP = 1;
        private const int ID_TITLE = 2;
        private const int ID_ALBUM = 3;
        private const int ID_STATUS = 4;
        private const int ID_EMAIL = 5;
        private const int ID_TIME_POS = 6;
        private const int ID_CLOCK = 7;
        private const int ID_TIME_LENGTH = 8;

        private Font m_MainFont = new Font7x5();
        private Font m_TimeFont = new Font7x5Time();

        public LCD(string friendlyName, bool autoStartable)
            : base(friendlyName, autoStartable)
        {
            if (Device == null) return;
            Surface.Clear();

            int y = 0;

            AddText(ID_ARTIST, 0, y, Surface.Width);
            AddTextTime(ID_PL_DISP, Surface.Width, y, -53);
            y += 8;

            AddText(ID_TITLE, 0, y, Surface.Width);
            y += 8;

            AddText(ID_ALBUM, 0, y, Surface.Width);
            y += 8;

            m_Drawables.Add(ID_PROGRESS_BAR, new DrawableProgressBar(26, y, Surface.Width - 27, y + 6));
            AddTextTime(ID_TIME_POS, 0, y, 25);
            AddTextTime(ID_TIME_LENGTH, Surface.Width, y, -25);
            y += 7;

            y = Surface.Height - 1 - 5 - 1;
            AddText(ID_STATUS, 0, y, 40);
            AddTextTime(ID_CLOCK, (Surface.Width / 2) - (39 / 2), y, 39);
            AddText(ID_EMAIL, Surface.Width, y, -70);



            /*y = Surface.Height - 7;
            AddText(666, 0, y, Surface.Width);

            SetText(666, "Hello C#!");*/
            Draw();
            Device.SetAsLCDForegroundApp(true);
        }

        private void AddText(int id, int x, int y, int max_w, byte color = Surface.PIXEL_ON)
        {
            m_Drawables.Add(id, new DrawableText(x, y, max_w, m_MainFont, color));
        }
        private void AddTextTime(int id, int x, int y, int max_w, byte color = Surface.PIXEL_ON)
        {
            m_Drawables.Add(id, new DrawableText(x, y, max_w, m_TimeFont, color));
        }

        public string Title { set { SetText(ID_TITLE, value);} }
        public string Artist { set { SetText(ID_ARTIST, value); } }
        public string Album { set { SetText(ID_ALBUM, value); } }
        public int TrackTime
        {
            set
            {
                SetText(ID_TIME_POS, value > 0 ? string.Format("{0:00}:{1:00}", value / 60, value % 60) : "");
            }
        }
        public int TrackLength
        {
            set
            {
                SetText(ID_TIME_LENGTH, value > 0 ? string.Format("{0:00}:{1:00}", value / 60, value % 60) : "");
            }
        }
        private int playlist_pos = -1, playlist_length = -1;
        public int PlaylistPosition { set { playlist_pos = value; UpdatePlaylist(); } }
        public int PlaylistLength { set { playlist_length = value; UpdatePlaylist(); } }
        private void UpdatePlaylist()
        {
            if (playlist_pos < 0 || playlist_length < 0)
            {
                SetText(ID_PL_DISP, "" );
            }
            else
            {
                var pl_len = string.Format("{0:000}", playlist_length);
                SetText(ID_PL_DISP, string.Format("{0:" + new string('0', pl_len.Length) + "}/{1}", playlist_pos, pl_len));
            }
        }

        public void SetProgress(int val, int min, int max)
        {
            if (m_Drawables.ContainsKey(ID_PROGRESS_BAR))
            {
                var progress = m_Drawables[ID_PROGRESS_BAR] as DrawableProgressBar;
                progress.Set(val, min, max);
            }
        }

        public void SetPlaybackState(PlaybackState state)
        {
            switch (state)
            {
                case PlaybackState.Playing:
                    SetText(ID_STATUS, "Playing");
                    break;
                case PlaybackState.Paused:
                    SetText(ID_STATUS, "Paused");
                    break;
                case PlaybackState.Stopped:
                    SetText(ID_STATUS, "Stopped");
                    break;

            }
        }

        public override void OnConfigure()
        {
            base.OnConfigure();
            MessageBox.Show("Nothing to do here");
        }

        public override void Update()
        {
            SetText(ID_CLOCK, DateTime.Now.ToString("HH:mm:ss"));
            base.Update();
        }
    }
}
