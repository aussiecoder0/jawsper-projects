using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TabManager.TabFiles.GuitarPro
{
    public class GuitarProDocument : TabFile
    {
        public GuitarProDocument(Stream s) : base(s) { }

        private string m_strversion;
        private int m_version_major;
        private int m_version_minor;

        protected override void Parse()
        {
            m_FileType = m_strversion = s.ReadStringWithLength();
            DetectVersion();
            m_FileType = string.Format("Guitar Pro v{0}.{1:00}", m_version_major, m_version_minor);

            s.Position = 0x1F;

            m_Title = s.GPReadString();
            m_SubTitle = s.GPReadString();
            m_Artist = s.GPReadString();
            m_Album = s.GPReadString();
        }

        private void DetectVersion()
        {
            var rx = new Regex("[v|L](?<major>[0-9])\\.(?<minor>[0-9][0-9])$");
            var match = rx.Match(m_strversion);
            if (match.Success)
            {
                m_version_major = int.Parse(match.Groups["major"].Value);
                m_version_minor = int.Parse(match.Groups["minor"].Value);
            }
            else
            {
                m_version_major = 0;
                m_version_minor = 0;
            }
        }

        public static bool IsType(Stream s)
        {
            s.Position = 0;
            return s.ReadStringWithLength().StartsWith("FICHIER GUITAR PRO");
        }
    }
}
