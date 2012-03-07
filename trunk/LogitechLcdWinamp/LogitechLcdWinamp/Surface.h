#pragma once

#include "Fonts.h"

typedef byte Pixel;

#define LCD_SIZE (160*43*1)
#define LCD_W (160)
#define LCD_H (43)
#define LCD_P (160)

#define PIXEL_ON (128)
#define PIXEL_OFF (0)

class Surface
{
	lgLcdBitmap160x43x1* m_bmp;

	Font* m_Font;
public:
	Surface(void);
	~Surface(void);

	lgLcdBitmapHeader* Get( ) { return &(m_bmp->hdr); };

	void Clear( Pixel colour = PIXEL_OFF ) { memset( m_bmp->pixels, colour, LCD_SIZE ); }
	void SetPixel(int,int,Pixel);
	void Print( char* str, int x, int y, Pixel colour = PIXEL_ON ) { Print( m_Font, str, x, y, colour ); }
	void Print( Font*, char* , int, int, Pixel colour = PIXEL_ON );

    void Line( int x1, int y1, int x2, int y2, Pixel c = PIXEL_ON );
    void Line( float x1, float y1, float x2, float y2, Pixel c = PIXEL_ON );
	void Box( int x1, int y1, int w, int h, Pixel c = PIXEL_ON );
	void BoxAbs( int x1, int y1, int x2, int y2, Pixel c = PIXEL_ON );

	void BarAbs( int x1, int y1, int x2, int y2, Pixel c = PIXEL_ON );
};