#include "stdafx.h"

using namespace std;

#pragma pack(1)
typedef struct {
   char  idlength;
   char  colourmaptype;
   char  datatypecode;
   short int colourmaporigin;
   short int colourmaplength;
   char  colourmapdepth;
   short int x_origin;
   short int y_origin;
   short width;
   short height;
   char  bitsperpixel;
   char  imagedescriptor;
} tga_header;

void tga_write_header( ofstream &f, size_t width, size_t height )
{
	tga_header th =
	{
		0,
		0,
		2,
		0, 0, 0, // colourmap
		0, // X origin
		0, // Y origin
		(short)(width & 0xFFFF), // W
		(short)(height & 0xFFFF), // H
		24, // 24bit
		0
	};
	f.write( reinterpret_cast<char*>(&th), sizeof( th ) );
}

void output_tga( ofstream &f, unsigned char* buff, size_t width, size_t height )
{
	tga_write_header( f, width, height );

	// B G R
	#pragma warning( suppress : 4309 ) // truncated value
	char color_on[3] = { 0, 0, 255 };
	char color_off[3] = { 0, 0, 0 };
	
	//int rle_counter = 0;
	//Pixel p = 0, prev_pixel = 0;
	for( long y = height - 1; y >= 0; y-- )
	{
		for( size_t x = 0; x < width; x++ )
		{
			/*p = buff[y * width + x];
			if( rle_counter > 0 )
			{
				if( rle_counter < 127 && p == prev_pixel ) rle_counter++;
				else
				{
					f.put( (char)(( rle_counter - 1) | 0x80 ) );
					f.write( p == PIXEL_ON ? color_on : color_off, 3 );
					rle_counter = 0;
				}
			} else rle_counter++;
			prev_pixel = p;*/
			f.write( buff[y * width + x] == PIXEL_ON ? color_on : color_off, 3 );
		}
		/*if( rle_counter > 0 )
		{
			f.put( (char)(( rle_counter - 1 ) | 0x80 ) );
			f.write( p == PIXEL_ON ? color_on : color_off, 3 );
			rle_counter = 0;
		}*/
	}
}