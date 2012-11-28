using System.Collections.Generic;
namespace LogitechLCD
{
    public class FontChar
    {
        public char m_Char;
        public string m_Data;
        public int m_Width;

        internal FontChar(char a_Char, string a_Data, int a_MaxHeight)
        {
            m_Char = a_Char;
            int len = a_Data.Length;
            if (len > 0)
            {
                m_Width = len / a_MaxHeight;
                m_Data = a_Data;
            }
            else
            {
                m_Width = 0; m_Data = null;
            }
        }
    }
    public abstract class Font
    {
        protected int m_MaxWidth;
        protected int m_MaxHeight;
        protected List<FontChar> m_FontMap;
        protected Dictionary<char, char> m_FontRemap;

        public Font()
        {
            m_MaxWidth = 5;
            m_FontMap = new List<FontChar>();
            m_FontRemap = new Dictionary<char, char>();
        }

        protected void Init(int h)
        {
            m_MaxHeight = h;
            InitCharset();
        }
        protected abstract void InitCharset();
        protected FontChar FindChar(char c)
        {
            foreach (var fwc in m_FontMap)
                if (fwc.m_Char == c)
                    return fwc;
            return null;
        }
        protected void SetChar(char c, string data)
        {
            m_FontMap.Add(new FontChar(c, data, m_MaxHeight));
        }
        protected void SetChar(char from, char to)
        {
            m_FontRemap.Add(from, to);
        }

        public string GetChar(char c, out int w, out int h)
        {
            FontChar chr = FindChar(c);
            if (chr == null)
            {
                if (m_FontRemap.ContainsKey(c))
                {
                    c = m_FontRemap[c];
                    chr = FindChar(c);
                    if (chr == null)
                    {
                        chr = m_FontMap[0];
                    }
                }
                else
                {
                    chr = m_FontMap[0];
                }
            }
            w = chr.m_Width;
            h = m_MaxHeight;
            return chr.m_Data;
        }

        public int MeasureWidth(string str)
        {
            int total = 0;
            for (int i = 0, n = str.Length; i < n; ++i)
            {
                int w, h;
                GetChar(str[i], out w, out h);
                total += w;
                total++; // add spacer
            }
            total--; // remove final spacer
            return total;
        }
    }
}