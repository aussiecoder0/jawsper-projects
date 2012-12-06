using System.Drawing;
namespace LogitechLCD
{
    public class DrawableProgressBar : Drawable
    {
        int m_Minimum, m_Value, m_Maximum;
        int m_BarOffset = 2;

        private Point m_Position2;

        public DrawableProgressBar(int a_X1, int a_Y1, int a_X2, int a_Y2, int a_BarOffset = 2)
            : base(new Point(a_X1, a_Y1))
        {
            m_Position2 = new Point(a_X2, a_Y2);
            m_BarOffset = a_BarOffset;
        }

        public override bool Draw(Surface a_Surface)
        {
            if (m_Changed)
            {
               // m_Changed = false;

                a_Surface.BoxAbs(m_Position.X, m_Position.Y, m_Position2.X, m_Position2.Y);
                a_Surface.BarAbs(m_Position.X + m_BarOffset, m_Position.Y + m_BarOffset, m_Position2.X - m_BarOffset, m_Position2.Y - m_BarOffset, Surface.PIXEL_OFF);

                if (m_Value > 0 && m_Maximum - m_Minimum > 0)
                {
                    int bar_x2 = ConvertRange(m_Value, m_Minimum, m_Maximum, m_Position.X + m_BarOffset, m_Position2.X - m_BarOffset);
                    if (bar_x2 > m_Position.X + m_BarOffset)
                    {
                        a_Surface.BarAbs(m_Position.X + m_BarOffset, m_Position.Y + m_BarOffset, bar_x2, m_Position2.Y - m_BarOffset);
                    }
                }
                return true;
            }
            return false;
        }

        public void Set(int val, int min, int max)
        {
            if (m_Value != val || m_Minimum != min || m_Maximum != max)
            {
                m_Changed = true;
                m_Value = val;
                m_Minimum = min; 
                m_Maximum = max;
            }
        }

        public static int ConvertRange(
            int value, // value to convert
            int originalStart, int originalEnd, // original range
            int newStart, int newEnd ) // desired range
        {
            double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
            return (int)(newStart + ((value - originalStart) * scale));
        }
    }
}