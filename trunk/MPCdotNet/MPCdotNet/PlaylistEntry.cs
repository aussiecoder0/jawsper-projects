using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MPCdotNet
{
    public class PlaylistEntry
    {
        public PlaylistEntry(List<KeyValuePair<string, string>> data)
        {
            foreach (var item in data)
            {
                if (item.Key == "Title") Title = item.Value;
                else if (item.Key == "Artist") Artist = item.Value;
                else if (item.Key == "Album") Album = item.Value;
                else if (item.Key == "Id") ID = int.Parse(item.Value);
                else if (item.Key == "Pos") Pos = int.Parse(item.Value);
                //else Console.WriteLine(string.Format("? PlaylistEntry[{0}] = {1}", item));
            }
        }

        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string Album { get; private set; }
        public int ID { get; private set; }
        public int Pos { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Artist, Title);
        }
    }
}
