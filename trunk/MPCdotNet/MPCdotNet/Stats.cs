using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPCdotNet
{
    partial class MPC
    {
        public class Stats
        {
            public Stats(List<KeyValuePair<string,string>> data)
            {
                foreach (var item in data) Set(item.Key, item.Value);
            }
            private void Set(string key, string value)
            {
                if (key == "artists")
                {
                    Artists = int.Parse(value);
                }
                else if (key == "albums")
                {
                    Albums = int.Parse(value);
                }
                else if (key == "songs")
                {
                    Songs = int.Parse(value);
                }
                else if (key == "uptime")
                {
                    Uptime = TimeSpan.FromSeconds(int.Parse(value));
                }
                else if (key == "playtime")
                {
                    Playtime = int.Parse(value);
                }
                else if (key == "db_playtime")
                {
                    DBPlayTime = int.Parse(value);
                }
                else if (key == "db_update")
                {
                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    DBUpdate = epoch.AddSeconds(int.Parse(value)).ToLocalTime();
                }
            }

            public int Artists { get; private set; }
            public int Albums { get; private set; }
            public int Songs { get; private set; }
            public TimeSpan Uptime { get; private set; }
            public int Playtime { get; private set; }
            public int DBPlayTime { get; private set; }
            public DateTime DBUpdate { get; private set; }
        }
    }
}
