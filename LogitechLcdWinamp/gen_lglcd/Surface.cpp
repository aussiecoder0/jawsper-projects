#include "stdafx.h"
#include "Surface.h"
#include <math.h>

Surface::Surface()
{
	m_bmp = new lgLcdBitmap160x43x1;
	m_bmp->hdr.Format = LGLCD_BMP_FORMAT_160x43x1;
	
	Clear();

	m_Font = new Font7x5();
}

Surface::~Surface(void)
{
	delete m_Font;
	free( m_bmp );
}

void Surface::SetPixel( int x, int y, Pixel val )
{
	m_bmp->pixels[ LCD_W * y + x ] = val;
}

void Surface::Print( wchar_t* a_String, int x1, int y1, Font* a_Font, Pixel colour, int max_x )
{
	if( a_Font == 0 ) a_Font = m_Font;
    Pixel* t = m_bmp->pixels + x1 + y1 * LCD_P;
	int xpos = x1;
    for ( int i = 0, n = (int)wcslen( a_String ); i < n; ++i )
    {
        Pixel* a = t;
        int h, v, cw, ch;
        wchar_t* c = a_Font->GetChar( a_String[i], &cw, &ch );
		for ( v = 0; v < ch; v++ )
		{
			for ( h = 0; h < cw && xpos + h < max_x; h++ )
			{
				if (*c++ == _T('o'))
				{
					*(a + h) = colour;//, *(a + h + LCD_P) = 0;
				}
			}
			a += LCD_P;
		}
        t += cw + 1;
		xpos += cw + 1;
    }
}

void Surface::Line( int x1, int y1, int x2, int y2, Pixel c)
{
	if ((x1 < 0) || (y1 < 0) || (x1 >= LCD_W) || (y1 >= LCD_H) ||
			(x2 < 0) || (y2 < 0) || (x2 >= LCD_W) || (y2 >= LCD_H))
	{
			return;
	}
	int b = x2 - x1;
	int h = y2 - y1;
	int l = abs( b );
	if (abs ( h ) > l) l = abs( h );
	int dx = b / l;
	int dy = h / l;
	for ( int i = 0; i <= l; i++ )
	{
		*(m_bmp->pixels + x1 + y1 * LCD_P) = c;
		x1 += dx, y1 += dy;
	}
}

void Surface::Line( float x1, float y1, float x2, float y2, Pixel c )
{
    if ((x1 < 0) || (y1 < 0) || (x1 >= LCD_W) || (y1 >= LCD_H) ||
            (x2 < 0) || (y2 < 0) || (x2 >= LCD_W) || (y2 >= LCD_H))
    {
            return;
    }
    float b = x2 - x1;
    float h = y2 - y1;
    float l = fabsf( b );
    if (fabsf ( h ) > l) l = fabsf( h );
    int il = (int)l;
    float dx = b / (float)l;
    float dy = h / (float)l;
    for ( int i = 0; i <= il; i++ )
    {
		*(m_bmp->pixels + (int)x1 + (int)y1 * LCD_P) = c;
        x1 += dx, y1 += dy;
    }
}

void Surface::Box( int x1, int y1, int w, int h, Pixel c )
{
	Line( x1, y1, x1 + w, y1, c ); // top
	Line( x1, y1 + h, x1 + w, y1 + h, c ); // bottom;
	Line( x1, y1, x1, y1 + h, c ); // left
	Line( x1 + w, y1, x1 + w, y1 + h, c ); // right
}

void Surface::BoxAbs( int x1, int y1, int x2, int y2, Pixel c )
{
	Line( x1, y1, x2, y1, c ); // top
	Line( x1, y2, x2, y2, c ); // bottom;
	Line( x1, y1, x1, y2, c ); // left
	Line( x2, y1, x2, y2, c ); // right
}

void Surface::Bar( int x1, int y1, int w, int h, Pixel c )
{
	for( int y = y1; y <= y1 + h; y++ )
	{
		Line( x1, y, x1 + w, y, c );
	}
}
void Surface::BarAbs( int x1, int y1, int x2, int y2, Pixel c )
{
	for( int y = y1; y <= y2; y++ )
	{
		Line( x1, y, x2, y, c );
	}
}