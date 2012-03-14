#pragma once

#include "Fonts.h"

typedef unsigned char Pixel;

/*#define LCD_SIZE (160*43*1)
#define LCD_W (160)
#define LCD_H (43)
#define LCD_P (160)*/

#define PIXEL_ON (128)
#define PIXEL_OFF (0)

class Surface
{
	int m_Width, m_Height, m_Pitch;
    Pixel* m_Buffer;
	lgLcdBitmap160x43x1* m_bmp;
public:
	Surface();
	Surface( int a_Width, int a_Height );
	~Surface();

	lgLcdBitmapHeader* Get( ) { return m_bmp == 0 ? 0 : &(m_bmp->hdr); };
	
	void Clear( Pixel colour = PIXEL_OFF ) { memset( m_Buffer, colour, m_Width * m_Height ); }
	void SetPixel(int,int,Pixel);
	void Print(  wchar_t* a_String, int x1, int y1, Font* a_Font, Pixel colour = PIXEL_ON, int max_x = -1 );

    void Line( int x1, int y1, int x2, int y2, Pixel c = PIXEL_ON );
    void Line( float x1, float y1, float x2, float y2, Pixel c = PIXEL_ON );

	void Box( int x1, int y1, int w, int h, Pixel c = PIXEL_ON );
	void BoxAbs( int x1, int y1, int x2, int y2, Pixel c = PIXEL_ON );

	void Bar( int x1, int y1, int w, int h, Pixel c = PIXEL_ON );
	void BarAbs( int x1, int y1, int x2, int y2, Pixel c = PIXEL_ON );

	void Plot( int x, int y, Pixel c = PIXEL_ON );

	void Save( const wchar_t* file );

	Pixel* GetBuffer() { return m_Buffer; }
	void CopyTo( Surface* a_Dst, int a_X, int a_Y, Pixel* a_AlphaColour = 0 );

	int GetWidth() { return m_Width; }
	int GetHeight() { return m_Height; }
	int GetPitch() { return m_Pitch; }
};