using System;
using System.IO;

namespace TabManager.TabFiles.PowerTab
{
    public class PowerTabDocument : TabFile
    {
        private PowerTabHeader m_header;

        public PowerTabHeader Header { get { return m_header; } }

        public PowerTabDocument(TabStream s) : base(s) { }

        protected override void Parse()
        {
            m_header = new PowerTabHeader();
            m_header.ParseHeader(s);
            switch (m_header.m_version)
            {
                case PowerTabHeader.FILEVERSION_1_0:
                case PowerTabHeader.FILEVERSION_1_0_2:
                    m_FileType = "PowerTab v1.0.x";
                    throw new NotImplementedException(m_FileType);
                case PowerTabHeader.FILEVERSION_1_5:
                    m_FileType = "PowerTab v1.5";
                    throw new NotImplementedException(m_FileType);
                case PowerTabHeader.FILEVERSION_1_7:
                    m_FileType = "PowerTab v1.7";
                    ParseFormat_1_7(s);
                    break;

            }

            if (m_header.m_songData != null)
            {
                m_Title = m_header.m_songData.title;
                m_SubTitle = "";
                m_Artist = m_header.m_songData.artist;
                if (m_header.m_songData.audioData != null)
                {
                    m_Album = m_header.m_songData.audioData.title;
                }
            }
        }

        private void ParseFormat_1_7(Stream s)
        {
        }

        public static bool IsType(TabStream s)
        {
            s.Position = 0;
            return s.LE_ReadUInt32() == PowerTabHeader.POWERTABFILE_MARKER;
        }
    }
}