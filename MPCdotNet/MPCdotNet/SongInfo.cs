using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPCdotNet
{
    partial class MPC
    {
        public class SongInfo
        {
            public SongInfo()
            {

            }
            public SongInfo(List<KeyValuePair<string, string>> data)
            {
                Pos = -1;
                ID = -1;
                foreach (var item in data)
                {
                    if (item.Key == "file") File = item.Value;
                    else if (item.Key == "Time") Time = int.Parse(item.Value);
                    else if (item.Key == "Album") Album = item.Value;
                    else if (item.Key == "Artist") Artist = item.Value;
                    else if (item.Key == "Title") Title = item.Value;
                    else if (item.Key == "Track") Track = item.Value;
                    else if (item.Key == "Pos") Pos = int.Parse(item.Value);
                    else if (item.Key == "Id") ID = int.Parse(item.Value);
                    //else Console.WriteLine("? SongInfo[{0}] = {1}", item);
                }
            }

            public string File { get; private set; }
            public int Time { get; private set; }
            public string Album { get; private set; }
            public string Artist { get; private set; }
            public string Title { get; private set; }
            public string Track { get; private set; }
            /// <summary>
            /// Playlist position
            /// </summary>
            public int Pos { get; private set; }
            /// <summary>
            /// Unique song id
            /// </summary>
            public int ID { get; private set; }
        }
    }
}
