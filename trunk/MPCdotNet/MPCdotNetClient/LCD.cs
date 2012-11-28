using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogitechLCD;

namespace MPCdotNet.Client
{
    public class LCD : LCDDisplay
    {
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

        public LCD(string name)
            : base(name, IntPtr.Zero)
        {
            Surface.Clear();

            Surface.BoxAbs(0, Surface.Height - 1 - 5, Surface.Width - 1, Surface.Height - 1);

            AddText(ID_ARTIST, 0, 0, Surface.Width);
            AddTextTime(ID_PL_DISP, Surface.Width, 0, -53);

            AddText(ID_TITLE, 0, 8, Surface.Width);

            AddText(ID_ALBUM, 0, 16, Surface.Width);

            AddText(ID_STATUS, 0, 23, 80);
            AddText(ID_EMAIL, Surface.Width, 23, -70);

            int time_y = Surface.Height - 1 - 5 - 7;
            AddTextTime(ID_TIME_POS, 0, time_y, 25);
            AddTextTime(ID_CLOCK, (Surface.Width / 2) - (39 / 2), time_y, 39 );
            AddTextTime(ID_TIME_LENGTH, Surface.Width, time_y, -25);
            Title = "Hello C#!";
            Draw();
        }

        private void AddText(int id, int w, int h, int max_w)
        {
            m_TextMap.Add(id, new DrawableText(w, h, max_w, m_MainFont));
        }
        private void AddTextTime(int id, int w, int h, int max_w)
        {
            m_TextMap.Add(id, new DrawableText(w, h, max_w, m_TimeFont));
        }

        public string Title { set { m_TextMap[ID_TITLE].Text = value; } }
        public string Artist { set { m_TextMap[ID_ARTIST].Text = value; } }
        public string Album { set { m_TextMap[ID_ALBUM].Text = value; } }
        public int TrackTime
        {
            set
            {
                m_TextMap[ID_TIME_POS].Text = value > 0 ? string.Format("{0:00}:{1:00}", value / 60, value % 60) : "";
            }
        }
        public int TrackLength
        {
            set
            {
                m_TextMap[ID_TIME_LENGTH].Text = value > 0 ? string.Format("{0:00}:{1:00}", value / 60, value % 60) : "";
            }
        }
        private int playlist_pos = -1, playlist_length = -1;
        public int PlaylistPosition { set { playlist_pos = value; UpdatePlaylist(); } }
        public int PlaylistLength { set { playlist_length = value; UpdatePlaylist(); } }
        private void UpdatePlaylist()
        {
            if (playlist_pos < 0 || playlist_length < 0)
            {
                m_TextMap[ID_PL_DISP].Text = "";
            }
            else
            {
                var pl_len = string.Format("{0:000}", playlist_length);
                m_TextMap[ID_PL_DISP].Text = string.Format("{0:" + new string('0', pl_len.Length) + "}/{1}", playlist_pos, pl_len);
            }
        }

        public void SetProgress(int val, int min, int max)
        {
            Surface.BarAbs(2, Surface.Height - 1 - 3, Surface.Width - 3, Surface.Height - 3, Surface.PIXEL_OFF);
            if (val > 0 && max - min > 0)
            {
                int x2 = (int)(((double)(val - min) / (double)(max - min)) * (double)(Surface.Width - 5) + 2.0);
                if (x2 > 2) Surface.BarAbs(2, Surface.Height - 1 - 3, x2, Surface.Height - 3);
            }
        }

        public void SetPlaybackState(PlaybackState state)
        {
            switch (state)
            {
                case PlaybackState.Playing:
                    m_TextMap[ID_STATUS].Text = "Playing";
                    break;
                case PlaybackState.Paused:
                    m_TextMap[ID_STATUS].Text = "Paused";
                    break;
                case PlaybackState.Stopped:
                    m_TextMap[ID_STATUS].Text = "Stopped";
                    break;

            }
        }

        public override void Update()
        {
            m_TextMap[ID_CLOCK].Text = DateTime.Now.ToString("HH:mm:ss");
            base.Update();
        }
    }
}
