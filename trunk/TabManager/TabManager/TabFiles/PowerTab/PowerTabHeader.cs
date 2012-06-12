using System;
using System.IO;

namespace TabManager.TabFiles.PowerTab
{
    public class PowerTabHeader
    {
        internal ushort m_version;
        internal byte m_fileType;

        internal SongData m_songData = null;
        internal LessonData m_lessonData = null;


        internal const uint POWERTABFILE_MARKER = 0x62617470U;

        internal const ushort FILEVERSION_1_0 = 1;
        internal const ushort FILEVERSION_1_0_2 = 2;
        internal const ushort FILEVERSION_1_5 = 3;
        internal const ushort FILEVERSION_1_7 = 4;
        internal const ushort FILEVERSION_CURRENT = FILEVERSION_1_7;

        internal const byte FILETYPE_SONG = 0;
        internal const byte FILETYPE_LESSON = 1;

        internal const byte RELEASETYPE_PUBLIC_AUDIO = 0;
        internal const byte RELEASETYPE_PUBLIC_VIDEO = 1;
        internal const byte RELEASETYPE_BOOTLEG = 2;
        internal const byte RELEASETYPE_NOTRELEASED = 3;

        internal const byte NUM_AUTHORTYPES = 2;
        internal const byte AUTHORTYPE_AUTHORKNOWN = 0;
        internal const byte AUTHORTYPE_TRADITIONAL = 1;

        internal bool ParseHeader(Stream s)
        {
            var marker = s.LE_ReadUInt32();
            if (marker != POWERTABFILE_MARKER)
            {
                Console.WriteLine("Marker error: {0:X}", marker);
                return false;
            }

            m_version = s.LE_ReadUInt16();

            switch (m_version)
            {
                case FILEVERSION_1_0:
                case FILEVERSION_1_0_2:
                    throw new NotImplementedException("Need to implement 1.0 reader");
                case FILEVERSION_1_5:
                    throw new NotImplementedException("Need to implement 1.5 reader");
                case FILEVERSION_1_7:
                    return ParseHeader_1_7(s);
                default:
                    return false;
            }
        }

        private bool ParseHeader_1_7(Stream s)
        {
            m_fileType = s.LE_ReadUInt8();

            switch (m_fileType)
            {
                case FILETYPE_SONG:
                    m_songData = new SongData();

                    m_songData.contentType = s.LE_ReadUInt8();
                    m_songData.title = s.ReadStringWithLength();
                    m_songData.artist = s.ReadStringWithLength();
                    m_songData.releaseType = s.LE_ReadUInt8();
                    switch (m_songData.releaseType)
                    {
                        case RELEASETYPE_PUBLIC_AUDIO:
                            m_songData.audioData = new SongData.AudioData();
                            m_songData.audioData.type = s.LE_ReadUInt8();
                            m_songData.audioData.title = s.ReadStringWithLength();
                            m_songData.audioData.year = s.LE_ReadUInt16();
                            m_songData.audioData.live = s.LE_ReadUInt8();
                            break;
                        case RELEASETYPE_PUBLIC_VIDEO:
                            m_songData.videoData = new SongData.VideoData();
                            m_songData.videoData.title = s.ReadStringWithLength();
                            m_songData.videoData.live = s.LE_ReadUInt8();
                            break;
                        case RELEASETYPE_BOOTLEG:
                            m_songData.bootlegData = new SongData.BootlegData();
                            m_songData.bootlegData.title = s.ReadStringWithLength();
                            m_songData.bootlegData.month = s.LE_ReadUInt16();
                            m_songData.bootlegData.day = s.LE_ReadUInt16();
                            m_songData.bootlegData.year = s.LE_ReadUInt16();
                            break;
                    }

                    m_songData.authorType = s.LE_ReadUInt8();
                    if (m_songData.authorType == AUTHORTYPE_AUTHORKNOWN)
                    {
                        m_songData.authorData = new SongData.AuthorData();
                        m_songData.authorData.composer = s.ReadStringWithLength();
                        m_songData.authorData.lyricist = s.ReadStringWithLength();
                    }

                    m_songData.arranger = s.ReadStringWithLength();

                    m_songData.guitarScoreTranscriber = s.ReadStringWithLength();
                    m_songData.bassScoreTranscriber = s.ReadStringWithLength();
                    m_songData.copyright = s.ReadStringWithLength();
                    m_songData.lyrics = s.ReadStringWithLength();
                    m_songData.guitarScoreNotes = s.ReadStringWithLength();
                    m_songData.bassScoreNotes = s.ReadStringWithLength();

                    break;
                case FILETYPE_LESSON:
                    m_lessonData = new LessonData();

                    m_lessonData.title = s.ReadStringWithLength();
                    m_lessonData.subtitle = s.ReadStringWithLength();
                    m_lessonData.musicStyle = s.LE_ReadUInt16();
                    m_lessonData.level = s.LE_ReadUInt8();
                    m_lessonData.author = s.ReadStringWithLength();
                    m_lessonData.notes = s.ReadStringWithLength();
                    m_lessonData.copyright = s.ReadStringWithLength();

                    break;
            }
            return true;
        }

        internal class SongData
        {
            internal byte contentType;
            internal string title;
            internal string artist;

            internal byte releaseType;
            internal AudioData audioData = null;
            internal VideoData videoData = null;
            internal BootlegData bootlegData = null;

            internal byte authorType;
            internal AuthorData authorData = null;

            internal string arranger;

            internal string guitarScoreTranscriber;
            internal string bassScoreTranscriber;

            internal string copyright;

            internal string lyrics;

            internal string guitarScoreNotes;
            internal string bassScoreNotes;

            internal class AudioData
            {
                internal byte type;
                internal string title;
                internal ushort year;
                internal byte live;
            }
            internal class VideoData
            {
                internal string title;
                internal byte live;
            }
            internal class BootlegData
            {
                internal string title;
                internal ushort month;
                internal ushort day;
                internal ushort year;

                internal DateTime GetDateTime()
                {
                    return new DateTime(year, month, day);
                }
            }
            internal class AuthorData
            {
                internal string composer;
                internal string lyricist;
            }

        }
        internal class LessonData
        {
            internal string title;
            internal string subtitle;
            internal ushort musicStyle;
            internal byte level;
            internal string author;
            internal string notes;
            internal string copyright;
        }
    }
}