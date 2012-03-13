#include "stdafx.h"
#include <stdint.h>

using namespace std;

#pragma pack(2)
typedef struct {
  unsigned char magic[2];
  uint32_t filesz;
  uint16_t creator1;
  uint16_t creator2;
  uint32_t bmp_offset;
} bmpfile_header;

void write_bmp_header( ofstream &f, size_t width, size_t height )
{
	bmpfile_header hdr = { 'B', 'M' , 0 };
	hdr.filesz = 14 + 40 + 2 + ( width * height / 8 );
	hdr.bmp_offset = 14 + 40 + 2;
	f.write( reinterpret_cast<char*>(&hdr), sizeof( hdr ) );

	BITMAPINFOHEADER dib = { 0 };
	dib.biSize = 40;
	dib.biWidth = width;
	dib.biHeight = -((int)height);
	dib.biPlanes = 1;
	dib.biBitCount = 1;
	dib.biCompression = 0;
	dib.biSizeImage = width * height;


	f.write( reinterpret_cast<char*>(&dib), sizeof(dib) );
	
	// B G R A
	#pragma warning( disable : 4309 ) // truncated value
	char color_table[] = 
	{
		0, 0, 0, 255,
		0, 0, 255, 255
	};
	#pragma warning( default : 4309 ) // truncated value
	f.write( color_table, sizeof( color_table ) );
}

void output_bmp( ofstream &f, unsigned char* buff, size_t width, size_t height )
{
	write_bmp_header( f, width, height );

	char count = 0;
	int b = 0;
	for( size_t y = 0; y < height; y++ )
	{
		for( size_t x = 0; x < width; x++ )
		{
			if( buff[y * width + x] == PIXEL_ON ) b |= 1;
			count++;
			if( count == 8 )
			{
				f.put( (char) b );
				count = 0;
				b = 0;
			}
			else b <<= 1;
		}
		/*int pad = (width /  8) % 4;
		while( pad-- > 0 )
		{
			f.put( 0 );
		}*/
	}
}