using System;

namespace LogitechLCD
{
    public class Surface
    {
        public const int PIXEL_ON = 128;
        public const int PIXEL_OFF = 0;

        byte[] m_Buffer;
        int m_Width, m_Height, m_Pitch;
        public Surface(int width, int height, int bpp)
        {
            m_Width = width;
            m_Height = height;
            m_Pitch = m_Width;
            m_Buffer = new byte[m_Width * m_Height * bpp];
        }

        public int Width { get { return m_Width; } }
        public int Height { get { return m_Height; } }

        public byte[] Data { get { return m_Buffer; } }

        public void Clear(byte c = PIXEL_OFF)
        {
            for (int i = 0; i < m_Buffer.Length; i++)
                m_Buffer[i] = c;
        }
        public void SetPixel(int x, int y, byte c = PIXEL_ON)
        {
            m_Buffer[m_Width * y + x] = c;
        }

        public void Line(int x1, int y1, int x2, int y2, byte c = PIXEL_ON)
        {
            if ((x1 < 0) || (y1 < 0) || (x1 >= m_Width) || (y1 >= m_Height) ||
                (x2 < 0) || (y2 < 0) || (x2 >= m_Width) || (y2 >= m_Height))
            {
                return;
            }
            int b = x2 - x1;
            int h = y2 - y1;
            int l = Math.Abs(b);
            if (Math.Abs(h) > l) l = Math.Abs(h);
            int dx = b / l;
            int dy = h / l;
            for (int i = 0; i <= l; i++)
            {
                m_Buffer[x1 + y1 * m_Pitch] = c;
                x1 += dx;
                y1 += dy;
            }
        }

        public void Box(int x1, int y1, int w, int h, byte c = PIXEL_ON)
        {
            Line(x1, y1, x1 + w, y1, c); // top
            Line(x1, y1 + h, x1 + w, y1 + h, c); // bottom;
            Line(x1, y1, x1, y1 + h, c); // left
            Line(x1 + w, y1, x1 + w, y1 + h, c); // right
        }

        public void BoxAbs(int x1, int y1, int x2, int y2, byte c = PIXEL_ON)
        {
            Line(x1, y1, x2, y1, c); // top
            Line(x1, y2, x2, y2, c); // bottom;
            Line(x1, y1, x1, y2, c); // left
            Line(x2, y1, x2, y2, c); // right
        }

        public void Bar(int x1, int y1, int w, int h, byte c = PIXEL_ON)
        {
            for (int y = y1; y <= y1 + h; y++)
            {
                Line(x1, y, x1 + w, y, c);
            }
        }
        public void BarAbs( int x1, int y1, int x2, int y2, byte c = PIXEL_ON )
        {
	        for( int y = y1; y <= y2; y++ )
	        {
		        Line( x1, y, x2, y, c );
	        }
        }

        public void Print(string a_String, int x1, int y1, Font a_Font, byte colour = PIXEL_ON, int max_x = -1)
        {
            if (max_x >= m_Width || max_x < 0) max_x = m_Width - 1;
            int t = x1 + y1 * m_Pitch;
            int xpos = x1;
            for (int i = 0, n = a_String.Length; i < n; ++i)
            {
                int a = t;
                int h, v, cw, ch;
                string c = a_Font.GetChar(a_String[i], out cw, out ch);
                int cpos = 0;
                for (v = 0; v < ch; v++)
                {
                    for (h = 0; h < cw; h++)
                    {
                        if (c[cpos++] == 'o' && xpos + h <= max_x)
                        {
                            m_Buffer[a + h] = colour;//, *(a + h + LCD_P) = 0;
                        }
                    }
                    a += m_Pitch;
                }
                t += cw + 1;
                xpos += cw + 1;
            }
        }
    }
}
