#include "Stdafx.h"
#include "Fonts.h"



Font::Font() : m_MaxWidth(5)
{
}

void Font::Init( int l, int h )
{
	m_Length = l;
	m_MaxHeight = h;
	InitCharset();
}

Font::~Font()
{
	for( int i = 0; i < 256; i++ )
		delete[] m_Font[i];
}

char* Font::GetChar( char c, int* w, int* h )
{
	char p = s_Transl[c];
	*w = m_CharWidth[p];
	*h = m_MaxHeight;
	return m_Font[p];
}

void Font::SetChar( char p, char* c )
{
	int l = strlen( c );
	m_CharWidth[p] = l / m_MaxHeight;
	m_Font[p] = new char[l];
	memcpy( m_Font[p], c, l );
	s_Transl[ p ] = p;
}

/********** Font7x5 **********/

void Font7x5::InitCharset()
{
	for(int i = 0; i < 256; i++ ) s_Transl[i] = 0;

	SetChar( 0,   "o:o"   ":o:"   "o:o"   ":o:"   "o:o"   ":o:"   "o:o"   );
	SetChar( 1,   ":"     ":"     ":"     ":"     ":"     ":"     ":"     );
	SetChar( 2,   "::"    "::"    "::"    "::"    "::"    "::"    "::"    );
	SetChar( 3,   ":::"   ":::"   ":::"   ":::"   ":::"   ":::"   ":::"   );
	SetChar( 4,   "::::"  "::::"  "::::"  "::::"  "::::"  "::::"  "::::"  );
	SetChar( 5,   ":::::" ":::::" ":::::" ":::::" ":::::" ":::::" ":::::" );

	SetChar( ' ', "::"    "::"    "::"    "::"    "::"    "::"    "::"    );
	SetChar( '!', ":"     "o"     "o"     "o"     ":"     "o"     ":"     );
	SetChar( '\"',":::"   "o:o"   "o:o"   ":::"   ":::"   ":::"   ":::"   );
	SetChar( '#', ":::::" ":o:o:" "ooooo" ":o:o:" "ooooo" ":o:o:" ":::::" );
	SetChar( '$', ":::::" ":oooo" "o:o::" ":ooo:" "::o:o" "oooo:" ":::::" );
	SetChar( '%', ":::::" ":o:o:" "ooooo" ":o:o:" "ooooo" ":o:o:" ":::::" );
	SetChar( '&', ":::::" ":oo::" ":o:::" ":oo::" "o::o:" ":oo:o" ":::::" );
	SetChar( '\'',":"     "o"     "o"     ":"     ":"     ":"     ":"     );
	SetChar( '(', "::"    ":o"    "o:"    "o:"    "o:"    ":o"    "::"    );
	SetChar( ')', "::"    "o:"    ":o"    ":o"    ":o"    "o:"    "::"    );
	SetChar( '*', ":::::" "o:o:o" ":ooo:" "ooooo" ":ooo:" "o:o:o" ":::::" );
	SetChar( '+', ":::"   ":::"   ":o:"   "ooo"   ":o:"   ":::"   ":::"   );
	SetChar( ',', "::"    "::"    "::"    "::"    ":o"    "o:"    "::"    );
	SetChar( '-', ":::"   ":::"   ":::"   "ooo"   ":::"   ":::"   ":::"   );
	SetChar( '.', ":"     ":"     ":"     ":"     ":"     "o"     ":"     );
	SetChar( '/', ":::"   "::o"   "::o"   ":o:"   "o::"   "o::"   ":::"   );
	SetChar( '1', ":::"   ":o:"   "oo:"   ":o:"   ":o:"   "ooo"   ":::"   );
	SetChar( '2', "::::"  ":oo:"  "o::o"  "::o:"  ":o::"  "oooo"  "::::"  );
	SetChar( '3', ":::"   "oo:"   "::o"   ":oo"   "::o"   "oo:"   ":::"   );
	SetChar( '4', "::::"  "::o:"  ":oo:"  "o:o:"  "oooo"  "::o:"  "::::"  );
	SetChar( '5', ":::"   "ooo"   "o::"   "oo:"   "::o"   "oo:"   ":::"   );
	SetChar( '6', ":::"   ":oo"   "o::"   "ooo"   "o:o"   ":o:"   ":::"   );
	SetChar( '7', ":::"   "ooo"   "::o"   ":o:"   ":o:"   ":o:"   ":::"   );
	SetChar( '8', "::::"  ":oo:"  "o::o"  ":oo:"  "o::o"  ":oo:"  "::::"  );
	SetChar( '9', "::::"  ":oo:"  "o::o"  ":ooo"  ":::o"  "ooo:"  "::::"  );
	SetChar( '0', "::::"  ":oo:"  "o::o"  "o::o"  "o::o"  ":oo:"  "::::"  );
	SetChar( ':', ":"     ":"     "o"     ":"     "o"     ":"     ":"     );
	SetChar( ';', "::"    "::"    ":o"    "::"    ":o"    "o:"    "::"    );
	SetChar( '<', ":::"   "::o"   ":o:"   "o::"   ":o:"   "::o"   ":::"   );
	SetChar( '=', ":::"   ":::"   "ooo"   ":::"   "ooo"   ":::"   ":::"   );
	SetChar( '>', ":::"   "o::"   ":o:"   "::o"   ":o:"   "o::"   ":::"   );
	SetChar( '?', "::::"  ":oo:"  "o::o"  "::o:"  "::::"  "::o:"  "::::"  );

	SetChar( '@', ":::::" ":ooo:" "o:::o" "o:ooo" "o:o:o" "::ooo" ":::::" );
	SetChar( 'A', "::::"  ":oo:"  "o::o"  "oooo"  "o::o"  "o::o"  "::::"  );
	SetChar( 'B', "::::"  "ooo:"  "o::o"  "ooo:"  "o::o"  "ooo:"  "::::"  );
	SetChar( 'C', ":::"   ":oo"   "o::"   "o::"   "o::"   ":oo"   ":::"   );
	SetChar( 'D', "::::"  "ooo:"  "o::o"  "o::o"  "o::o"  "ooo:"  "::::"  );
	SetChar( 'E', ":::"   "ooo"   "o::"   "oo:"   "o::"   "ooo"   ":::"   );
	SetChar( 'F', ":::"   "ooo"   "o::"   "oo:"   "o::"   "o::"   ":::"   );
	SetChar( 'G', "::::"  ":ooo"  "o:::"  "o:oo"  "o::o"  ":oo:"  "::::"  );
	SetChar( 'H', "::::"  "o::o"  "o::o"  "oooo"  "o::o"  "o::o"  "::::"  );
	SetChar( 'I', ":::"   "ooo"   ":o:"   ":o:"   ":o:"   "ooo"   ":::"   );
	SetChar( 'J', ":::"   "::o"   "::o"   "::o"   "::o"   "oo:"   ":::"   );
	SetChar( 'K', "::::"  "o::o"  "o:o:"  "oo::"  "o:o:"  "o::o"  "::::"  );
	SetChar( 'L', ":::"   "o::"   "o::"   "o::"   "o::"   "ooo"   ":::"   );
	SetChar( 'M', ":::::" "oo:o:" "o:o:o" "o:o:o" "o:::o" "o:::o" ":::::" );
	SetChar( 'N', ":::::" "oo::o" "oo::o" "o:o:o" "o::oo" "o::oo" ":::::" );
	SetChar( 'O', "::::"  ":oo:"  "o::o"  "o::o"  "o::o"  ":oo:"  "::::"  );
	SetChar( 'P', "::::"  "ooo:"  "o::o"  "ooo:"  "o:::"  "o:::"  "::::"  );
	SetChar( 'Q', "::::"  ":oo:"  "o::o"  "o::o"  "o:oo"  ":ooo"  "::::"  );
	SetChar( 'R', "::::"  "ooo:"  "o::o"  "ooo:"  "o:o:"  "o::o"  "::::"  );
	SetChar( 'S', "::::"  ":ooo"  "o:::"  ":oo:"  ":::o"  "ooo:"  "::::"  );
	SetChar( 'T', ":::"   "ooo"   ":o:"   ":o:"   ":o:"   ":o:"   ":::"   );
	SetChar( 'U', "::::"  "o::o"  "o::o"  "o::o"  "o::o"  ":ooo"  "::::"  );
	SetChar( 'V', ":::::" "o:::o" "o:::o" ":o:o:" ":o:o:" "::o::" ":::::" );
	SetChar( 'W', ":::::" "o:::o" "o:::o" "o:o:o" "o:o:o" ":o:o:" ":::::" );
	SetChar( 'X', ":::::" "o:::o" ":o:o:" "::o::" ":o:o:" "o:::o" ":::::" );
	SetChar( 'Y', "::::"  "o::o"  "o::o"  ":ooo"  ":::o"  ":oo:"  "::::"  );
	SetChar( 'Z', ":::::" "ooooo" ":::o:" "::o::" ":o:::" "ooooo" ":::::" );
	SetChar( '[', "::"    "oo"    "o:"    "o:"    "o:"    "oo"    "::"    );
	SetChar( '\\',":::"   "o::"   "o::"   ":o:"   "::o"   "::o"   ":::"   );
	SetChar( ']', "::"    "oo"    ":o"    ":o"    ":o"    "oo"    "::"    );
	SetChar( '^', ":::"   ":o:"   "o:o"   ":::"   ":::"   ":::"   ":::"   );
	SetChar( '_', ":::"   ":::"   ":::"   ":::"   ":::"   "ooo"   ":::"   );
	
	SetChar( '`', "::"    "o:"    ":o"    "::"    "::"    "::"    "::"    );
	SetChar( 'a', ":::"   ":::"   ":::"   ":::"   ":::"   ":::"   ":::"   );
	SetChar( 'a', ":::"   ":::"   "oo:"   "::o"   "o:o"   ":oo"   ":::"   );
	SetChar( 'b', "::::"  "o:::"  "ooo:"  "o::o"  "o::o"  "ooo:"  "::::"  );
	SetChar( 'c', ":::"   ":::"   ":oo"   "o::"   "o::"   ":oo"   ":::"   );
	SetChar( 'd', "::::"  ":::o"  ":ooo"  "o::o"  "o::o"  ":ooo"  "::::"  );
	SetChar( 'e', ":::"   ":::"   ":o:"   "o:o"   "o::"   ":oo"   ":::"   );
	SetChar( 'f', "::"    ":o"    "o:"    "oo"    "o:"    "o:"    "::"    );
	SetChar( 'g', "::::"  "::::"  ":ooo"  "o::o"  ":ooo"  ":::o"  "ooo:"  );
	SetChar( 'h', ":::"   "o::"   "o::"   "oo:"   "o:o"   "o:o"   ":::"   );
	SetChar( 'i', ":"     "o"     ":"     "o"     "o"     "o"     ":"     );
	SetChar( 'j', ":::"   "::o"   ":::"   "::o"   "::o"   "oo:"   ":::"   );
	SetChar( 'k', ":::"   "o::"   "o:o"   "oo:"   "o:o"   "o:o"   ":::"   );
	SetChar( 'l', ":"     "o"     "o"     "o"     "o"     "o"     ":"     );
	SetChar( 'm', ":::::" ":::::" "oooo:" "o:o:o" "o:o:o" "o:o:o" ":::::" );
	SetChar( 'n', ":::"   ":::"   "oo:"   "o:o"   "o:o"   "o:o"   ":::"   );
	SetChar( 'o', "::::"  "::::"  ":oo:"  "o::o"  "o::o"  ":oo:"  "::::"  );
	SetChar( 'p', "::::"  "::::"  "ooo:"  "o::o"  "ooo:"  "o:::"  "::::"  );
	SetChar( 'q', "::::"  "::::"  ":ooo"  "o::o"  ":ooo"  ":::o"  "::::"  );
	SetChar( 'r', "::"    "::"    "oo"    "o:"    "o:"    "o:"    "::"    );
	SetChar( 's', ":::"   ":::"   ":oo"   "o::"   "::o"   "oo:"   ":::"   );
	SetChar( 't', "::"    "::"    "o:"    "oo"    "o:"    "o:"    ":o"    );
	SetChar( 'u', ":::"   ":::"   "o:o"   "o:o"   "o:o"   ":oo"   ":::"   );
	SetChar( 'v', ":::"   ":::"   "o:o"   "o:o"   ":o:"   ":o:"   ":::"   );
	SetChar( 'w', ":::::" ":::::" "o:o:o" "o:o:o" ":o:o:" ":o:o:" ":::::" );
	SetChar( 'x', ":::"   ":::"   "o:o"   ":o:"   ":o:"   "o:o"   ":::"   );
	SetChar( 'y', ":::"   ":::"   "o:o"   "o:o"   ":o:"   ":o:"   "o::"   );
	SetChar( 'z', ":::"   ":::"   "ooo"   "::o"   "o::"   "ooo"   ":::"   );
	SetChar( '{', ":::"   ":oo"   ":o:"   "oo:"   ":o:"   ":oo"   ":::"   );
	SetChar( '|', ":"     "o"     "o"     "o"     "o"     "o"     ":"     );
	SetChar( '}', ":::"   "oo:"   ":o:"   ":oo"   ":o:"   "oo:"   ":::"   );
	SetChar( '~', ":::"   ":::"   ":::"   ":::"   ":::"   ":::"   ":::"   );
}