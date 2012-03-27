#include "stdafx.h"
#include "Surface.h"
#include <math.h>

// default constructor: 160x43 for lcd
Surface::Surface() : m_Width( 160 ), m_Height( 43 ), m_Pitch( 160 )
{
	m_bmp = new lgLcdBitmap160x43x1;
	m_bmp->hdr.Format = LGLCD_BMP_FORMAT_160x43x1;
	m_Buffer = m_bmp->pixels;
	Clear();
}

Surface::Surface( int a_Width, int a_Height ) :
	m_Width( a_Width ), m_Height( a_Height ), m_Pitch( a_Width )
{
	if( m_Width == 160 && m_Height == 43 )
	{
		m_bmp = new lgLcdBitmap160x43x1;
		m_bmp->hdr.Format = LGLCD_BMP_FORMAT_160x43x1;
		m_Buffer = m_bmp->pixels;
	}
	else
	{
		m_bmp = 0;
		m_Buffer = new Pixel[ m_Width * m_Height ];
	}
	
	Clear();
}

Surface::~Surface(void)
{
	if( m_bmp != 0 )
	{
		delete m_bmp;
	}
	else
	{
		delete[] m_Buffer;
	}
	m_bmp = 0;
	m_Buffer = 0;
}

void Surface::SetPixel( int x, int y, Pixel val )
{
	m_Buffer[ m_Width * y + x ] = val;
}

void Surface::Print( wchar_t* a_String, int x1, int y1, Font* a_Font, Pixel colour, int max_x )
{
	if( max_x >= m_Width || max_x < 0 ) max_x = m_Width - 1;
    Pixel* t = m_Buffer + x1 + y1 * m_Pitch;
	int xpos = x1;
    for ( int i = 0, n = (int)wcslen( a_String ); i < n; ++i )
    {
        Pixel* a = t;
        int h, v, cw, ch;
        wchar_t* c = a_Font->GetChar( a_String[i], cw, ch );
		for ( v = 0; v < ch; v++ )
		{
			for ( h = 0; h < cw; h++ )
			{
				if (*c++ == _T('o') && xpos + h <= max_x)
				{
					*(a + h) = colour;//, *(a + h + LCD_P) = 0;
				}
			}
			a += m_Pitch;
		}
        t += cw + 1;
		xpos += cw + 1;
    }
}

void Surface::Line( int x1, int y1, int x2, int y2, Pixel c)
{
	if ((x1 < 0) || (y1 < 0) || (x1 >= m_Width) || (y1 >= m_Height) ||
			(x2 < 0) || (y2 < 0) || (x2 >= m_Width) || (y2 >= m_Height))
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
		*(m_Buffer + x1 + y1 * m_Pitch) = c;
		x1 += dx, y1 += dy;
	}
}

void Surface::Line( float x1, float y1, float x2, float y2, Pixel c )
{
    if ((x1 < 0) || (y1 < 0) || (x1 >= m_Width) || (y1 >= m_Height) ||
            (x2 < 0) || (y2 < 0) || (x2 >= m_Width) || (y2 >= m_Height))
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
		*(m_Buffer + (int)x1 + (int)y1 * m_Pitch) = c;
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

void Surface::Plot( int x, int y, Pixel c )
{
	m_Buffer[ y * m_Pitch + x ] = c;
}

void output_tga( ofstream &, unsigned char* buff, size_t width, size_t height );
void output_bmp( ofstream &, unsigned char* buff, size_t width, size_t height );

void Surface::Save( const wchar_t* a_File )
{
	/*ofstream f( a_File, ios::out | ios::trunc | ios::binary );

	if( f.is_open() )
	{
		output_tga( f, m_bmp->pixels, m_Width, m_Height );
	}
	else
	{
		MessageBox( 0, _T("no open"), _T(""), MB_OK );
	}

	f.close();*/
}

void Surface::CopyTo( Surface* a_Dst, int a_X, int a_Y, Pixel* a_AlphaColour )
{
    Pixel* dst = a_Dst->GetBuffer();
	Pixel* src = m_Buffer;
    if ((src) && (dst))
    {
        int srcwidth = m_Width;
        int srcheight = m_Height;
        int srcpitch = m_Pitch;
        int dstwidth = a_Dst->GetWidth();
        int dstheight = a_Dst->GetHeight();
        int dstpitch = a_Dst->GetPitch();
        if ((srcwidth + a_X) > dstwidth) srcwidth = dstwidth - a_X;
        if ((srcheight + a_Y) > dstheight) srcheight = dstheight - a_Y;
        if (a_X < 0) src -= a_X, srcwidth += a_X, a_X =0;
        if (a_Y < 0) src -= a_Y * srcpitch, srcheight += a_Y, a_Y = 0;
        if ((srcwidth > 0) && (srcheight > 0))
        {
            dst += a_X + dstpitch * a_Y;
			if( a_AlphaColour != 0 )
			{
				for ( int y = 0; y < srcheight; y++ )
                {
                    for( int x = 0; x < srcwidth; x++ )
                    {
                        if(src[x] != *a_AlphaColour)
							memcpy( dst + x, src + x, sizeof(Pixel) );
                    }
					dst += dstpitch;
					src += srcpitch;
                }
			}
			else
			{
				for ( int y = 0; y < srcheight; y++ )
				{
					memcpy(dst, src, sizeof(Pixel) * srcwidth);
					dst += dstpitch;
					src += srcpitch;
				}
			}
        }
    }
}