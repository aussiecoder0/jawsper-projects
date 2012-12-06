using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace MPCdotNet
{
    partial class MPC
    {
        public class Status
        {
            public Status() { }
            public Status(List<KeyValuePair<string, string>> data)
            {
                foreach (var item in data) Set(item.Key, item.Value);
            }
            private void Set(string key, string value)
            {
                if (key == "volume")
                {
                    Volume = int.Parse(value);
                }
                else if (key == "repeat")
                {
                    Repeat = value == "1";
                }
                else if (key == "random")
                {
                    Random = value == "1";
                }
                else if (key == "single")
                {
                    Single = value == "1";
                }
                else if (key == "consume")
                {
                    Consume = value == "1";
                }
                else if (key == "playlist")
                {
                    PlaylistVersion = int.Parse(value);
                }
                else if (key == "playlistlength")
                {
                    PlaylistLength = int.Parse(value);
                }
                else if (key == "state")
                {
                    if (value == "play") PlaybackState = PlaybackState.Playing;
                    if (value == "pause") PlaybackState = PlaybackState.Paused;
                    if (value == "stop") PlaybackState = PlaybackState.Stopped;
                }
                else if (key == "song")
                {
                    Song = int.Parse(value);
                }
                else if (key == "songid")
                {
                    SongID = int.Parse(value);
                }
                else if (key == "nextsong")
                {
                    NextSong = int.Parse(value);
                }
                else if (key == "nextsongid")
                {
                    NextSongID = int.Parse(value);
                }
                else if (key == "time")
                {
                    var raw = Regex.Match(value, @"^(\d+):(\d+)$");
                    if(raw.Success)
                        Time = new SongTime() { Current = int.Parse(raw.Groups[1].Value), Total = int.Parse(raw.Groups[2].Value) };
                }
                else if (key == "elapsed")
                {
                    Elapsed = float.Parse(value);
                }
                else if (key == "bitrate")
                {
                    Bitrate = int.Parse(value);
                }
                else if (key == "xfade")
                {
                    CrossFade = float.Parse(value);
                }
                else if (key == "mixrampdb")
                {

                }
                else if (key == "mixrampdelay")
                {

                }
                else if (key == "audio")
                {
                    var raw = Regex.Match(value, @"^(\d+):(\d+):(\d+)$");
                    if( raw.Success)
                        Audio = new int[] { int.Parse(raw.Groups[1].Value), int.Parse(raw.Groups[2].Value), int.Parse(raw.Groups[3].Value) };
                }
                else if (key == "updating_db")
                {
                    UpdatingDB = int.Parse(value);
                }
                else if (key == "error")
                {
                    Error = value;
                }
                else
                {
                    Debug.WriteLine(string.Format("Unimplemented status item: \"{0}\" = \"{1}\"", key, value));
                }

            }

            public int Volume { get; private set; }
            public bool Repeat { get; private set; }
            public bool Random { get; private set; }
            public bool Single { get; private set; }
            public bool Consume { get; private set; }
            public int PlaylistVersion { get; private set; }
            public int PlaylistLength { get; private set; }
            public PlaybackState PlaybackState { get; private set; }
            /// <summary>
            /// The playlist position of the current song
            /// </summary>
            public int Song { get; private set; }
            /// <summary>
            /// The global song id of the current song
            /// </summary>
            public int SongID { get; private set; }
            public int NextSong { get; private set; }
            public int NextSongID { get; private set; }
            public SongTime Time { get; private set; }
            public float Elapsed { get; private set; }
            public int Bitrate { get; private set; }
            public float CrossFade { get; private set; }
            public int[] Audio { get; private set; }
            public int UpdatingDB { get; private set; }
            public string Error { get; private set; }
        }
    }
}
