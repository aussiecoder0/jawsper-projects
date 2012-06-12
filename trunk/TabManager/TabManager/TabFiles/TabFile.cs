using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TabManager.TabFiles.PowerTab;
using TabManager.TabFiles.GuitarPro;

namespace TabManager.TabFiles
{
    public abstract class TabFile
    {
        public static readonly string TestFileFolder = @"..\..\..\testfiles\";

        protected FileInfo m_File;
        protected string m_FileType;
        protected string m_Title;
        protected string m_SubTitle;
        protected string m_Artist;
        protected string m_Album;

        public FileInfo File { get { return m_File; } protected set { m_File = value; } }
        public string FileType { get { return m_FileType; } }
        public string Title { get { return m_Title; } }
        public string SubTitle { get { return m_SubTitle; } }
        public string Artist { get { return m_Artist; } }
        public string Album { get { return m_Album; } }

        protected Stream s;
        protected TabFile(Stream s)
        {
            this.s = s;
            this.s.Position = 0;
            Parse();
        }

        protected abstract void Parse();

        public static TabFile OpenTab(FileInfo filename)
        {
            TabFile f = null;
            using (var fs = new FileStream(filename.FullName, FileMode.Open))
            {
                if (PowerTabDocument.IsType(fs))
                {
                    f = new PowerTabDocument(fs);
                }
                else if (GuitarProDocument.IsType(fs))
                {
                    f = new GuitarProDocument(fs);
                }
            }
            if (f != null) f.File = filename;
            return f;
        }
    }
}
