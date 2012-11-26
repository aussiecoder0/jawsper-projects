using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPCdotNet
{
    public class Output
    {
        private int m_ID;
        private string m_Name;
        private bool m_Enabled;
        public Output(int id, string name, bool enabled)
        {
            m_ID = id;
            m_Name = name;
            m_Enabled = enabled;
        }

        public override string ToString()
        {
            return String.Format("Output {{ id: {0}; name: \"{1}\"; enabled: {2} }}", m_ID, m_Name, m_Enabled);
        }
    }
}
