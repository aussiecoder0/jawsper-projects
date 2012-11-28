namespace LogitechLCD
{
    public class DrawableText
    {
        private bool m_Changed = false;
        private int m_X, m_Y, m_Width, m_MaxWidth;
        private string m_Str;
        Font m_Font;
        int prev_len;

        public DrawableText(int a_X, int a_Y, int a_Width, Font a_Font)
        {
            m_X = a_X;
            m_Y = a_Y;
            m_Width = a_Width;
            m_MaxWidth = a_Width;
            m_Changed = false;
            m_Font = a_Font;
            m_Str = "";
            prev_len = -1;
        }

        public string Text
        {
            set
            {
                if (m_Str != value)
                {
                    m_Str = value != null ? value : "";
                    m_Changed = true;
                }
            }
        }

        public bool Draw(Surface a_Surface)
        {
            if (m_Changed)
            {
                m_Changed = false;

                int text_x = m_X;
                int text_width;
                int prev_text_width = prev_len;
                int bar_x = m_X;

                prev_len = text_width = m_Font.MeasureWidth(m_Str);
                if (prev_text_width < 0)
                {
                    prev_text_width = text_width;
                }

                int bar_width = prev_text_width;

                // MaxWidth < 0 means right aligned, X will be the rightmost position
                if (m_MaxWidth < 0)
                {
                    if (text_width > 0 - m_MaxWidth)
                        text_width = 0 - m_MaxWidth;
                    text_x = m_X - text_width;
                    bar_x = m_X - bar_width;
                }
                else
                {
                    if (text_width > m_MaxWidth) text_width = m_MaxWidth;
                    if (bar_width > m_MaxWidth) bar_width = m_MaxWidth;
                }
                a_Surface.Bar(bar_x, m_Y, bar_width - 1, 6, Surface.PIXEL_OFF);
                a_Surface.Print(m_Str, text_x, m_Y, m_Font, Surface.PIXEL_ON, text_x + text_width);

                return true;
            }

            return false;
        }
    }
}