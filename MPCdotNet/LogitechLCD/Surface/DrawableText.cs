using System.Drawing;
namespace LogitechLCD
{
    public class DrawableText : Drawable
    {
        private int m_Width, m_MaxWidth;
        private string m_Str;
        Font m_Font;
        int prev_len;
        private byte color = Surface.PIXEL_ON;

        public DrawableText(int a_X, int a_Y, int a_Width, Font a_Font, byte color = Surface.PIXEL_ON)
            : base(new Point(a_X, a_Y))
        {
            m_Width = a_Width;
            m_MaxWidth = a_Width;
            m_Font = a_Font;
            m_Str = "";
            prev_len = -1;
            this.color = color;
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

        public override bool Draw(Surface a_Surface)
        {
            if (m_Changed)
            {
                //m_Changed = false;

                int text_x = m_Position.X;
                int text_width;
                int prev_text_width = prev_len;
                int bar_x = m_Position.X;

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
                    text_x = m_Position.X - text_width;
                    bar_x = m_Position.X - bar_width;
                }
                else
                {
                    if (text_width > m_MaxWidth) text_width = m_MaxWidth;
                    if (bar_width > m_MaxWidth) bar_width = m_MaxWidth;
                }

                a_Surface.Bar(bar_x, m_Position.Y, bar_width - 1, 6, color == Surface.PIXEL_ON ? Surface.PIXEL_OFF : Surface.PIXEL_ON);
                a_Surface.Print(m_Str, text_x, m_Position.Y, m_Font, color, text_x + text_width);

                return true;
            }

            return false;
        }
    }
}