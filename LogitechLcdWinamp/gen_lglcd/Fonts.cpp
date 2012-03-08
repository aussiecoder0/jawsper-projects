#include "Stdafx.h"
#include "Fonts.h"

FontChar::FontChar(wchar_t ac, wchar_t* ad, int max_height)
{
	c = ac;
	size_t l = wcslen( ad );
	w = l / max_height;
	if( w > 0 )
	{
		d = new wchar_t[l];
		memcpy( d, ad, l * sizeof(wchar_t) );
	}
	else d = 0;
}

FontChar::~FontChar()
{
	if( d != 0 ) delete[] d;
}

Font::Font() : m_MaxWidth(5)
{
	m_Font = new FontMap();
}

void Font::Init( int h )
{
	m_MaxHeight = h;
	InitCharset();
}

Font::~Font()
{
	for( FontMap::iterator iter = m_Font->begin(); iter != m_Font->end(); iter++ )
	{
		delete iter->second;
	}
	delete m_Font;
}

wchar_t* Font::GetChar( wchar_t c, int* w, int* h )
{
	if( c > 255 ) c = _T(' ');
	wchar_t p = s_Transl[c];
	FontChar* chr = (*m_Font)[p];
	*w = chr->w;
	*h = m_MaxHeight;
	return chr->d;
}

void Font::SetChar( wchar_t p, wchar_t* c )
{
	FontChar* el = new FontChar( p, c, m_MaxHeight );
	m_Font->insert( make_pair( p, el ) );
	s_Transl[ p ] = p;
}

int Font::MeasureWidth( wchar_t* str )
{
	int total = 0;
	for( int i = 0, n = wcslen( str ); i < n; ++i )
	{
		int w, h;
		GetChar( str[i], &w, &h );
		total += w;
		total++; // add spacer
	}
	total--; // remove final spacer
	return total;
}

/********** Font7x5 **********/

void Font7x5::InitCharset()
{
	for(int i = 0; i < 256; i++ ) s_Transl[i] = 0;


	SetChar( 0,   L""      L""      L""      L""      L""      L""      L""      );
	SetChar( 1,   L":"     L":"     L":"     L":"     L":"     L":"     L":"     );
	SetChar( 2,   L"::"    L"::"    L"::"    L"::"    L"::"    L"::"    L"::"    );
	SetChar( 3,   L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   );
	SetChar( 4,   L"::::"  L"::::"  L"::::"  L"::::"  L"::::"  L"::::"  L"::::"  );
	SetChar( 5,   L":::::" L":::::" L":::::" L":::::" L":::::" L":::::" L":::::" );

	SetChar( ' ', L"::"    L"::"    L"::"    L"::"    L"::"    L"::"    L"::"    );
	SetChar( '!', L":"     L"o"     L"o"     L"o"     L":"     L"o"     L":"     );
	SetChar( '\"',L":::"   L"o:o"   L"o:o"   L":::"   L":::"   L":::"   L":::"   );
	SetChar( '#', L":::::" L":o:o:" L"ooooo" L":o:o:" L"ooooo" L":o:o:" L":::::" );
	SetChar( '$', L":::::" L":oooo" L"o:o::" L":ooo:" L"::o:o" L"oooo:" L":::::" );
	SetChar( '%', L":::::" L":o:o:" L"ooooo" L":o:o:" L"ooooo" L":o:o:" L":::::" );
	SetChar( '&', L":::::" L":oo::" L":o:::" L":oo::" L"o::o:" L":oo:o" L":::::" );
	SetChar( '\'',L":"     L"o"     L"o"     L":"     L":"     L":"     L":"     );
	SetChar( '(', L"::"    L":o"    L"o:"    L"o:"    L"o:"    L":o"    L"::"    );
	SetChar( ')', L"::"    L"o:"    L":o"    L":o"    L":o"    L"o:"    L"::"    );
	SetChar( '*', L":::::" L"o:o:o" L":ooo:" L"ooooo" L":ooo:" L"o:o:o" L":::::" );
	SetChar( '+', L":::"   L":::"   L":o:"   L"ooo"   L":o:"   L":::"   L":::"   );
	SetChar( ',', L"::"    L"::"    L"::"    L"::"    L":o"    L"o:"    L"::"    );
	SetChar( '-', L":::"   L":::"   L":::"   L"ooo"   L":::"   L":::"   L":::"   );
	SetChar( '.', L":"     L":"     L":"     L":"     L":"     L"o"     L":"     );
	SetChar( '/', L":::"   L"::o"   L"::o"   L":o:"   L"o::"   L"o::"   L":::"   );
	SetChar( '1', L":::"   L":o:"   L"oo:"   L":o:"   L":o:"   L"ooo"   L":::"   );
	SetChar( '2', L"::::"  L":oo:"  L"o::o"  L"::o:"  L":o::"  L"oooo"  L"::::"  );
	SetChar( '3', L":::"   L"oo:"   L"::o"   L":oo"   L"::o"   L"oo:"   L":::"   );
	SetChar( '4', L"::::"  L"::o:"  L":oo:"  L"o:o:"  L"oooo"  L"::o:"  L"::::"  );
	SetChar( '5', L":::"   L"ooo"   L"o::"   L"oo:"   L"::o"   L"oo:"   L":::"   );
	SetChar( '6', L":::"   L":oo"   L"o::"   L"ooo"   L"o:o"   L":o:"   L":::"   );
	SetChar( '7', L":::"   L"ooo"   L"::o"   L":o:"   L":o:"   L":o:"   L":::"   );
	SetChar( '8', L"::::"  L":oo:"  L"o::o"  L":oo:"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( '9', L"::::"  L":oo:"  L"o::o"  L":ooo"  L":::o"  L"ooo:"  L"::::"  );
	SetChar( '0', L"::::"  L":oo:"  L"o::o"  L"o::o"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( ':', L":"     L":"     L"o"     L":"     L"o"     L":"     L":"     );
	SetChar( ';', L"::"    L"::"    L":o"    L"::"    L":o"    L"o:"    L"::"    );
	SetChar( '<', L":::"   L"::o"   L":o:"   L"o::"   L":o:"   L"::o"   L":::"   );
	SetChar( '=', L":::"   L":::"   L"ooo"   L":::"   L"ooo"   L":::"   L":::"   );
	SetChar( '>', L":::"   L"o::"   L":o:"   L"::o"   L":o:"   L"o::"   L":::"   );
	SetChar( '?', L"::::"  L":oo:"  L"o::o"  L"::o:"  L"::::"  L"::o:"  L"::::"  );

	SetChar( '@', L":::::" L":ooo:" L"o:::o" L"o:ooo" L"o:o:o" L"::ooo" L":::::" );
	SetChar( 'A', L"::::"  L":oo:"  L"o::o"  L"oooo"  L"o::o"  L"o::o"  L"::::"  );
	SetChar( 'B', L"::::"  L"ooo:"  L"o::o"  L"ooo:"  L"o::o"  L"ooo:"  L"::::"  );
	SetChar( 'C', L":::"   L":oo"   L"o::"   L"o::"   L"o::"   L":oo"   L":::"   );
	SetChar( 'D', L"::::"  L"ooo:"  L"o::o"  L"o::o"  L"o::o"  L"ooo:"  L"::::"  );
	SetChar( 'E', L":::"   L"ooo"   L"o::"   L"oo:"   L"o::"   L"ooo"   L":::"   );
	SetChar( 'F', L":::"   L"ooo"   L"o::"   L"oo:"   L"o::"   L"o::"   L":::"   );
	SetChar( 'G', L"::::"  L":ooo"  L"o:::"  L"o:oo"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( 'H', L"::::"  L"o::o"  L"o::o"  L"oooo"  L"o::o"  L"o::o"  L"::::"  );
	SetChar( 'I', L":::"   L"ooo"   L":o:"   L":o:"   L":o:"   L"ooo"   L":::"   );
	SetChar( 'J', L":::"   L"::o"   L"::o"   L"::o"   L"::o"   L"oo:"   L":::"   );
	SetChar( 'K', L"::::"  L"o::o"  L"o:o:"  L"oo::"  L"o:o:"  L"o::o"  L"::::"  );
	SetChar( 'L', L":::"   L"o::"   L"o::"   L"o::"   L"o::"   L"ooo"   L":::"   );
	SetChar( 'M', L":::::" L"oo:o:" L"o:o:o" L"o:o:o" L"o:::o" L"o:::o" L":::::" );
	SetChar( 'N', L":::::" L"oo::o" L"oo::o" L"o:o:o" L"o::oo" L"o::oo" L":::::" );
	SetChar( 'O', L"::::"  L":oo:"  L"o::o"  L"o::o"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( 'P', L"::::"  L"ooo:"  L"o::o"  L"ooo:"  L"o:::"  L"o:::"  L"::::"  );
	SetChar( 'Q', L"::::"  L":oo:"  L"o::o"  L"o::o"  L"o:oo"  L":ooo"  L"::::"  );
	SetChar( 'R', L"::::"  L"ooo:"  L"o::o"  L"ooo:"  L"o:o:"  L"o::o"  L"::::"  );
	SetChar( 'S', L"::::"  L":ooo"  L"o:::"  L":oo:"  L":::o"  L"ooo:"  L"::::"  );
	SetChar( 'T', L":::"   L"ooo"   L":o:"   L":o:"   L":o:"   L":o:"   L":::"   );
	SetChar( 'U', L"::::"  L"o::o"  L"o::o"  L"o::o"  L"o::o"  L":ooo"  L"::::"  );
	SetChar( 'V', L":::::" L"o:::o" L"o:::o" L":o:o:" L":o:o:" L"::o::" L":::::" );
	SetChar( 'W', L":::::" L"o:::o" L"o:::o" L"o:o:o" L"o:o:o" L":o:o:" L":::::" );
	SetChar( 'X', L":::::" L"o:::o" L":o:o:" L"::o::" L":o:o:" L"o:::o" L":::::" );
	SetChar( 'Y', L"::::"  L"o::o"  L"o::o"  L":ooo"  L":::o"  L":oo:"  L"::::"  );
	SetChar( 'Z', L":::::" L"ooooo" L":::o:" L"::o::" L":o:::" L"ooooo" L":::::" );
	SetChar( '[', L"::"    L"oo"    L"o:"    L"o:"    L"o:"    L"oo"    L"::"    );
	SetChar( '\\',L":::"   L"o::"   L"o::"   L":o:"   L"::o"   L"::o"   L":::"   );
	SetChar( ']', L"::"    L"oo"    L":o"    L":o"    L":o"    L"oo"    L"::"    );
	SetChar( '^', L":::"   L":o:"   L"o:o"   L":::"   L":::"   L":::"   L":::"   );
	SetChar( '_', L":::"   L":::"   L":::"   L":::"   L":::"   L"ooo"   L":::"   );
	
	SetChar( '`', L"::"    L"o:"    L":o"    L"::"    L"::"    L"::"    L"::"    );
	SetChar( 'a', L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   );
	SetChar( 'a', L":::"   L":::"   L"oo:"   L"::o"   L"o:o"   L":oo"   L":::"   );
	SetChar( 'b', L"::::"  L"o:::"  L"ooo:"  L"o::o"  L"o::o"  L"ooo:"  L"::::"  );
	SetChar( 'c', L":::"   L":::"   L":oo"   L"o::"   L"o::"   L":oo"   L":::"   );
	SetChar( 'd', L"::::"  L":::o"  L":ooo"  L"o::o"  L"o::o"  L":ooo"  L"::::"  );
	SetChar( 'e', L":::"   L":::"   L":o:"   L"o:o"   L"o::"   L":oo"   L":::"   );
	SetChar( 'f', L"::"    L":o"    L"o:"    L"oo"    L"o:"    L"o:"    L"::"    );
	SetChar( 'g', L"::::"  L"::::"  L":ooo"  L"o::o"  L":ooo"  L":::o"  L"ooo:"  );
	SetChar( 'h', L":::"   L"o::"   L"o::"   L"oo:"   L"o:o"   L"o:o"   L":::"   );
	SetChar( 'i', L":"     L"o"     L":"     L"o"     L"o"     L"o"     L":"     );
	SetChar( 'j', L":::"   L"::o"   L":::"   L"::o"   L"::o"   L"oo:"   L":::"   );
	SetChar( 'k', L":::"   L"o::"   L"o:o"   L"oo:"   L"o:o"   L"o:o"   L":::"   );
	SetChar( 'l', L":"     L"o"     L"o"     L"o"     L"o"     L"o"     L":"     );
	SetChar( 'm', L":::::" L":::::" L"oooo:" L"o:o:o" L"o:o:o" L"o:o:o" L":::::" );
	SetChar( 'n', L":::"   L":::"   L"oo:"   L"o:o"   L"o:o"   L"o:o"   L":::"   );
	SetChar( 'o', L"::::"  L"::::"  L":oo:"  L"o::o"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( 'p', L"::::"  L"::::"  L"ooo:"  L"o::o"  L"ooo:"  L"o:::"  L"::::"  );
	SetChar( 'q', L"::::"  L"::::"  L":ooo"  L"o::o"  L":ooo"  L":::o"  L"::::"  );
	SetChar( 'r', L"::"    L"::"    L"oo"    L"o:"    L"o:"    L"o:"    L"::"    );
	SetChar( 's', L":::"   L":::"   L":oo"   L"o::"   L"::o"   L"oo:"   L":::"   );
	SetChar( 't', L"::"    L"o:"    L"oo"    L"o:"    L"o:"    L":o"    L"::"    );
	SetChar( 'u', L":::"   L":::"   L"o:o"   L"o:o"   L"o:o"   L":oo"   L":::"   );
	SetChar( 'v', L":::"   L":::"   L"o:o"   L"o:o"   L":o:"   L":o:"   L":::"   );
	SetChar( 'w', L":::::" L":::::" L"o:o:o" L"o:o:o" L":o:o:" L":o:o:" L":::::" );
	SetChar( 'x', L":::"   L":::"   L"o:o"   L":o:"   L":o:"   L"o:o"   L":::"   );
	SetChar( 'y', L":::"   L":::"   L"o:o"   L"o:o"   L":o:"   L":o:"   L"o::"   );
	SetChar( 'z', L":::"   L":::"   L"ooo"   L"::o"   L"o::"   L"ooo"   L":::"   );
	SetChar( '{', L":::"   L":oo"   L":o:"   L"oo:"   L":o:"   L":oo"   L":::"   );
	SetChar( '|', L":"     L"o"     L"o"     L"o"     L"o"     L"o"     L":"     );
	SetChar( '}', L":::"   L"oo:"   L":o:"   L":oo"   L":o:"   L"oo:"   L":::"   );
	SetChar( '~', L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   );
}

void Font7x5Time::InitCharset()
{
	SetChar( ':', L":"     L":"     L"o"     L":"     L"o"     L":"     L":"     );
	SetChar( '1', L":::::" L"::o::" L":oo::" L"::o::" L"::o::" L":ooo:" L":::::" );
	SetChar( '2', L":::::" L":ooo:" L"o:::o" L"::oo:" L":o:::" L"ooooo" L":::::"  );
	SetChar( '3', L":::::" L"oooo:" L"::::o" L"::oo:" L"::::o" L"oooo:" L":::::"  );
	SetChar( '4', L":::::" L"o::::" L"o::o:" L"ooooo" L":::o:" L":::o:" L":::::"  );
	SetChar( '5', L":::::" L"ooooo" L"o::::" L"oooo:" L"::::o" L"oooo:" L":::::"  );
	SetChar( '6', L":::::" L":oooo" L"o::::" L"oooo:" L"o:::o" L":ooo:" L":::::"  );
	SetChar( '7', L":::::" L"ooooo" L"::::o" L":::o:" L"::o::" L"::o::" L":::::"  );
	SetChar( '8', L":::::" L":ooo:" L"o:::o" L":ooo:" L"o:::o" L":ooo:" L":::::"  );
	SetChar( '9', L":::::" L":ooo:" L"o:::o" L":oooo" L"::::o" L":ooo:" L":::::"  );
	SetChar( '0', L":::::" L":ooo:" L"o::oo" L"o:o:o" L"oo::o" L":ooo:" L":::::"  );
}