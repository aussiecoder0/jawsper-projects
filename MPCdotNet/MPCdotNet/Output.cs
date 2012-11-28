using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPCdotNet
{
    partial class MPC
    {
        public class Output
        {
            public Output(int id, string name, bool enabled)
            {
                ID = id;
                Name = name;
                Enabled = enabled;
            }

            public int ID { get; private set; }
            public string Name { get; private set; }
            public bool Enabled { get; private set; }

            public override string ToString()
            {
                return String.Format("Output {{ id: {0}; name: \"{1}\"; enabled: {2} }}", ID, Name, Enabled);
            }
        }
    }
}
