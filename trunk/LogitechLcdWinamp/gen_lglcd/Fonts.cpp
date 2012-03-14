#include "Stdafx.h"
#include "Fonts.h"

FontChar::FontChar(wchar_t a_Char, wchar_t* a_Data, size_t max_height)
{
	m_Char = a_Char;
	size_t len = wcslen( a_Data );
	if( len > 0 )
	{
		m_Width = len / max_height;
	//if( m_Width > 0 )
	//{
		m_Data = new wchar_t[ len ];
		memcpy( m_Data, a_Data, len * sizeof(wchar_t) );
	}
	else m_Width = 0, m_Data = 0;
}

FontChar::~FontChar()
{
	if( m_Data != 0 ) delete[] m_Data;
}


Font::Font() : m_MaxWidth(5)
{
}

void Font::Init( int h )
{
	m_MaxHeight = h;
	InitCharset();
}

Font::~Font()
{
	for( FontMap::iterator iter = m_FontMap.begin(); iter != m_FontMap.end(); iter++ )
	{
		delete *iter;
	}
}

FontChar* FindChar( FontMap& map, const wchar_t c )
{
	for( FontMap::iterator it = map.begin(); it != map.end(); ++it )
	{
		if( *it != 0 && (*it)->m_Char == c ) return *it;
	}
	return 0;
}

wchar_t* Font::GetChar( const wchar_t c, int& w, int& h )
{	
	FontChar* chr = FindChar( m_FontMap, c );

	if( c == 0 )
	{
		FontRemap::const_iterator re_it = m_FontRemap.find( c );
		if( re_it != m_FontRemap.end() )
		{
			chr = FindChar( m_FontMap, re_it->second );
			if( chr == 0 )
				chr = *(m_FontMap.begin());
		}
		else
		{
			chr = *(m_FontMap.begin());
		}
	}
	w = chr->m_Width;
	h = m_MaxHeight;
	return chr->m_Data;
}

void Font::SetChar( wchar_t p, wchar_t* c )
{
	FontChar* el = new FontChar( p, c, m_MaxHeight );
	m_FontMap.push_back( el );
}

void Font::SetChar( wchar_t from, wchar_t to )
{
	m_FontRemap.insert( make_pair( from, to ) );
}

int Font::MeasureWidth( wchar_t* str )
{
	int total = 0;
	for( int i = 0, n = wcslen( str ); i < n; ++i )
	{
		int w, h;
		GetChar( str[i], w, h );
		total += w;
		total++; // add spacer
	}
	total--; // remove final spacer
	return total;
}

/********** Font7x5 **********/

void Font7x5::InitCharset()
{
	SetChar( 0,   L""      L""      L""      L""      L""      L""      L""      );
	SetChar( 1,   L":"     L":"     L":"     L":"     L":"     L":"     L":"     );
	SetChar( 2,   L"::"    L"::"    L"::"    L"::"    L"::"    L"::"    L"::"    );
	SetChar( 3,   L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   );
	SetChar( 4,   L"::::"  L"::::"  L"::::"  L"::::"  L"::::"  L"::::"  L"::::"  );
	SetChar( 5,   L":::::" L":::::" L":::::" L":::::" L":::::" L":::::" L":::::" );

	SetChar( L' ', L"::"    L"::"    L"::"    L"::"    L"::"    L"::"    L"::"    );
	SetChar( L'!', L":"     L"o"     L"o"     L"o"     L":"     L"o"     L":"     );
	SetChar( L'\"',L":::"   L"o:o"   L"o:o"   L":::"   L":::"   L":::"   L":::"   );
	SetChar( L'#', L":::::" L":o:o:" L"ooooo" L":o:o:" L"ooooo" L":o:o:" L":::::" );
	SetChar( L'$', L":::::" L":oooo" L"o:o::" L":ooo:" L"::o:o" L"oooo:" L":::::" );
	SetChar( L'%', L":::::" L":o:o:" L"ooooo" L":o:o:" L"ooooo" L":o:o:" L":::::" );
	SetChar( L'&', L":::::" L":oo::" L":o:::" L":oo::" L"o::o:" L":oo:o" L":::::" );
	SetChar( L'\'',L":"     L"o"     L"o"     L":"     L":"     L":"     L":"     );
	SetChar( L'(', L"::"    L":o"    L"o:"    L"o:"    L"o:"    L":o"    L"::"    );
	SetChar( L')', L"::"    L"o:"    L":o"    L":o"    L":o"    L"o:"    L"::"    );
	SetChar( L'*', L":::::" L"o:o:o" L":ooo:" L"ooooo" L":ooo:" L"o:o:o" L":::::" );
	SetChar( L'+', L":::"   L":::"   L":o:"   L"ooo"   L":o:"   L":::"   L":::"   );
	SetChar( L',', L"::"    L"::"    L"::"    L"::"    L":o"    L"o:"    L"::"    );
	SetChar( L'-', L":::"   L":::"   L":::"   L"ooo"   L":::"   L":::"   L":::"   );
	SetChar( L'.', L":"     L":"     L":"     L":"     L":"     L"o"     L":"     );
	SetChar( L'/', L":::"   L"::o"   L"::o"   L":o:"   L"o::"   L"o::"   L":::"   );
	SetChar( L'1', L":::"   L":o:"   L"oo:"   L":o:"   L":o:"   L"ooo"   L":::"   );
	SetChar( L'2', L"::::"  L":oo:"  L"o::o"  L"::o:"  L":o::"  L"oooo"  L"::::"  );
	SetChar( L'3', L":::"   L"oo:"   L"::o"   L":oo"   L"::o"   L"oo:"   L":::"   );
	SetChar( L'4', L"::::"  L"::o:"  L":oo:"  L"o:o:"  L"oooo"  L"::o:"  L"::::"  );
	SetChar( L'5', L":::"   L"ooo"   L"o::"   L"oo:"   L"::o"   L"oo:"   L":::"   );
	SetChar( L'6', L":::"   L":oo"   L"o::"   L"ooo"   L"o:o"   L":o:"   L":::"   );
	SetChar( L'7', L":::"   L"ooo"   L"::o"   L":o:"   L":o:"   L":o:"   L":::"   );
	SetChar( L'8', L"::::"  L":oo:"  L"o::o"  L":oo:"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( L'9', L"::::"  L":oo:"  L"o::o"  L":ooo"  L":::o"  L"ooo:"  L"::::"  );
	SetChar( L'0', L"::::"  L":oo:"  L"o::o"  L"o::o"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( L':', L":"     L":"     L"o"     L":"     L"o"     L":"     L":"     );
	SetChar( L';', L"::"    L"::"    L":o"    L"::"    L":o"    L"o:"    L"::"    );
	SetChar( L'<', L":::"   L"::o"   L":o:"   L"o::"   L":o:"   L"::o"   L":::"   );
	SetChar( L'=', L":::"   L":::"   L"ooo"   L":::"   L"ooo"   L":::"   L":::"   );
	SetChar( L'>', L":::"   L"o::"   L":o:"   L"::o"   L":o:"   L"o::"   L":::"   );
	SetChar( L'?', L"::::"  L":oo:"  L"o::o"  L"::o:"  L"::::"  L"::o:"  L"::::"  );

	SetChar( L'@', L":::::" L":ooo:" L"o:::o" L"o:ooo" L"o:o:o" L"::ooo" L":::::" );
	SetChar( L'A', L"::::"  L":oo:"  L"o::o"  L"oooo"  L"o::o"  L"o::o"  L"::::"  );
	SetChar( L'B', L"::::"  L"ooo:"  L"o::o"  L"ooo:"  L"o::o"  L"ooo:"  L"::::"  );
	SetChar( L'C', L":::"   L":oo"   L"o::"   L"o::"   L"o::"   L":oo"   L":::"   );
	SetChar( L'D', L"::::"  L"ooo:"  L"o::o"  L"o::o"  L"o::o"  L"ooo:"  L"::::"  );
	SetChar( L'E', L":::"   L"ooo"   L"o::"   L"oo:"   L"o::"   L"ooo"   L":::"   );
	SetChar( L'F', L":::"   L"ooo"   L"o::"   L"oo:"   L"o::"   L"o::"   L":::"   );
	SetChar( L'G', L"::::"  L":ooo"  L"o:::"  L"o:oo"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( L'H', L"::::"  L"o::o"  L"o::o"  L"oooo"  L"o::o"  L"o::o"  L"::::"  );
	SetChar( L'I', L":::"   L"ooo"   L":o:"   L":o:"   L":o:"   L"ooo"   L":::"   );
	SetChar( L'J', L":::"   L"::o"   L"::o"   L"::o"   L"::o"   L"oo:"   L":::"   );
	SetChar( L'K', L"::::"  L"o::o"  L"o:o:"  L"oo::"  L"o:o:"  L"o::o"  L"::::"  );
	SetChar( L'L', L":::"   L"o::"   L"o::"   L"o::"   L"o::"   L"ooo"   L":::"   );
	SetChar( L'M', L":::::" L"oo:o:" L"o:o:o" L"o:o:o" L"o:::o" L"o:::o" L":::::" );
	SetChar( L'N', L":::::" L"oo::o" L"oo::o" L"o:o:o" L"o::oo" L"o::oo" L":::::" );
	SetChar( L'O', L"::::"  L":oo:"  L"o::o"  L"o::o"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( L'P', L"::::"  L"ooo:"  L"o::o"  L"ooo:"  L"o:::"  L"o:::"  L"::::"  );
	SetChar( L'Q', L"::::"  L":oo:"  L"o::o"  L"o::o"  L"o:oo"  L":ooo"  L"::::"  );
	SetChar( L'R', L"::::"  L"ooo:"  L"o::o"  L"ooo:"  L"o:o:"  L"o::o"  L"::::"  );
	SetChar( L'S', L"::::"  L":ooo"  L"o:::"  L":oo:"  L":::o"  L"ooo:"  L"::::"  );
	SetChar( L'T', L":::"   L"ooo"   L":o:"   L":o:"   L":o:"   L":o:"   L":::"   );
	SetChar( L'U', L"::::"  L"o::o"  L"o::o"  L"o::o"  L"o::o"  L":ooo"  L"::::"  );
	SetChar( L'V', L":::::" L"o:::o" L"o:::o" L":o:o:" L":o:o:" L"::o::" L":::::" );
	SetChar( L'W', L":::::" L"o:::o" L"o:::o" L"o:o:o" L"o:o:o" L":o:o:" L":::::" );
	SetChar( L'X', L":::::" L"o:::o" L":o:o:" L"::o::" L":o:o:" L"o:::o" L":::::" );
	SetChar( L'Y', L"::::"  L"o::o"  L"o::o"  L":ooo"  L":::o"  L":oo:"  L"::::"  );
	SetChar( L'Z', L":::::" L"ooooo" L":::o:" L"::o::" L":o:::" L"ooooo" L":::::" );
	SetChar( L'[', L"::"    L"oo"    L"o:"    L"o:"    L"o:"    L"oo"    L"::"    );
	SetChar( L'\\',L":::"   L"o::"   L"o::"   L":o:"   L"::o"   L"::o"   L":::"   );
	SetChar( L']', L"::"    L"oo"    L":o"    L":o"    L":o"    L"oo"    L"::"    );
	SetChar( L'^', L":::"   L":o:"   L"o:o"   L":::"   L":::"   L":::"   L":::"   );
	SetChar( L'_', L":::"   L":::"   L":::"   L":::"   L":::"   L"ooo"   L":::"   );
	
	SetChar( L'`', L"::"    L"o:"    L":o"    L"::"    L"::"    L"::"    L"::"    );
	SetChar( L'a', L":::"   L":::"   L"oo:"   L"::o"   L"o:o"   L":oo"   L":::"   );
	SetChar( L'b', L"::::"  L"o:::"  L"ooo:"  L"o::o"  L"o::o"  L"ooo:"  L"::::"  );
	SetChar( L'c', L":::"   L":::"   L":oo"   L"o::"   L"o::"   L":oo"   L":::"   );
	SetChar( L'd', L"::::"  L":::o"  L":ooo"  L"o::o"  L"o::o"  L":ooo"  L"::::"  );
	SetChar( L'e', L":::"   L":::"   L":o:"   L"o:o"   L"o::"   L":oo"   L":::"   );
	SetChar( L'f', L"::"    L":o"    L"o:"    L"oo"    L"o:"    L"o:"    L"::"    );
	SetChar( L'g', L"::::"  L"::::"  L":ooo"  L"o::o"  L":ooo"  L":::o"  L"ooo:"  );
	SetChar( L'h', L":::"   L"o::"   L"o::"   L"oo:"   L"o:o"   L"o:o"   L":::"   );
	SetChar( L'i', L":"     L"o"     L":"     L"o"     L"o"     L"o"     L":"     );
	SetChar( L'j', L":::"   L"::o"   L":::"   L"::o"   L"::o"   L"oo:"   L":::"   );
	SetChar( L'k', L":::"   L"o::"   L"o:o"   L"oo:"   L"o:o"   L"o:o"   L":::"   );
	SetChar( L'l', L":"     L"o"     L"o"     L"o"     L"o"     L"o"     L":"     );
	SetChar( L'm', L":::::" L":::::" L"oooo:" L"o:o:o" L"o:o:o" L"o:o:o" L":::::" );
	SetChar( L'n', L":::"   L":::"   L"oo:"   L"o:o"   L"o:o"   L"o:o"   L":::"   );
	SetChar( L'o', L"::::"  L"::::"  L":oo:"  L"o::o"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( L'p', L"::::"  L"::::"  L"ooo:"  L"o::o"  L"ooo:"  L"o:::"  L"::::"  );
	SetChar( L'q', L"::::"  L"::::"  L":ooo"  L"o::o"  L":ooo"  L":::o"  L"::::"  );
	SetChar( L'r', L"::"    L"::"    L"oo"    L"o:"    L"o:"    L"o:"    L"::"    );
	SetChar( L's', L":::"   L":::"   L":oo"   L"o::"   L"::o"   L"oo:"   L":::"   );
	SetChar( L't', L"::"    L"o:"    L"oo"    L"o:"    L"o:"    L":o"    L"::"    );
	SetChar( L'u', L":::"   L":::"   L"o:o"   L"o:o"   L"o:o"   L":oo"   L":::"   );
	SetChar( L'v', L":::"   L":::"   L"o:o"   L"o:o"   L":o:"   L":o:"   L":::"   );
	SetChar( L'w', L":::::" L":::::" L"o:o:o" L"o:o:o" L":o:o:" L":o:o:" L":::::" );
	SetChar( L'x', L":::"   L":::"   L"o:o"   L":o:"   L":o:"   L"o:o"   L":::"   );
	SetChar( L'y', L":::"   L":::"   L"o:o"   L"o:o"   L":o:"   L":o:"   L"o::"   );
	SetChar( L'z', L":::"   L":::"   L"ooo"   L"::o"   L"o::"   L"ooo"   L":::"   );
	SetChar( L'{', L":::"   L":oo"   L":o:"   L"oo:"   L":o:"   L":oo"   L":::"   );
	SetChar( L'|', L":"     L"o"     L"o"     L"o"     L"o"     L"o"     L":"     );
	SetChar( L'}', L":::"   L"oo:"   L":o:"   L":oo"   L":o:"   L"oo:"   L":::"   );
	SetChar( L'~', L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   L":::"   );
	
	SetChar( L'À', L"o:::"  L":o::"  L":oo:"  L"o::o"  L"oooo"  L"o::o"  L"::::"  );
	SetChar( L'Ä', L"o::o"  L"::::"  L":oo:"  L"o::o"  L"oooo"  L"o::o"  L"::::"  );
	SetChar( L'Á', L":::o"  L"::o:"  L":oo:"  L"o::o"  L"oooo"  L"o::o"  L"::::"  );

	SetChar( L'È', L'E' ); SetChar( L'Ë', L'E' ); SetChar( L'É', L'E' );
	SetChar( L'Ì', L'I' ); SetChar( L'Ï', L'I' ); SetChar( L'Í', L'I' );
	SetChar( L'Ò', L'O' ); SetChar( L'Ö', L'O' ); SetChar( L'Ó', L'O' );
	SetChar( L'Ù', L'U' ); SetChar( L'Ü', L'U' ); SetChar( L'Ú', L'U' );

	SetChar( L'à', L"o::"   L":o:"   L"oo:"   L"::o"   L"o:o"   L":oo"   L":::"   );
	SetChar( L'ä', L"o:o"   L":::"   L"oo:"   L"::o"   L"o:o"   L":oo"   L":::"   );
	SetChar( L'á', L"::o"   L":o:"   L"oo:"   L"::o"   L"o:o"   L":oo"   L":::"   );

	SetChar( L'è', L"o::"   L":o:"   L":o:"   L"o:o"   L"o::"   L":oo"   L":::"   );
	SetChar( L'ë', L"o:o"   L":::"   L":o:"   L"o:o"   L"o::"   L":oo"   L":::"   );
	SetChar( L'é', L"::o"   L":o:"   L":o:"   L"o:o"   L"o::"   L":oo"   L":::"   );

	SetChar( L'ì', L"o:"    L":o"    L"::"    L":o"    L":o"    L":o"    L"::"    );
	SetChar( L'ï', L":::"   L"o:o"   L":::"   L":o:"   L":o:"   L":o:"   L":::"   );
	SetChar( L'í', L":o"    L"o:"    L"::"    L"o:"    L"o:"    L"o:"    L"::"    );
	
	SetChar( L'ò', L"o:::"  L":o::"  L"::::"  L":oo:"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( L'ö', L"::::"  L"o::o"  L"::::"  L":oo:"  L"o::o"  L":oo:"  L"::::"  );
	SetChar( L'ó', L":::o"  L"::o:"  L"::::"  L":oo:"  L"o::o"  L":oo:"  L"::::"  );
	
	SetChar( L'ù', L"o::"   L":o:"   L":::"   L"o:o"   L"o:o"   L":oo"   L":::"   );
	SetChar( L'ü', L":::"   L"o:o"   L":::"   L"o:o"   L"o:o"   L":oo"   L":::"   );
	SetChar( L'ú', L"::o"   L":o:"   L":::"   L"o:o"   L"o:o"   L":oo"   L":::"   );

	SetChar( L'æ', L":::::" L":::::" L"oo:o:" L"::o:o" L"o:o::" L":o:oo" L":::::" );
}

void Font7x5Time::InitCharset()
{
	SetChar( 0,    L""      L""      L""      L""      L""      L""      L""      );

	SetChar( L':', L":"     L":"     L"o"     L":"     L"o"     L":"     L":"     );
	SetChar( L'1', L":::::" L"::o::" L":oo::" L"::o::" L"::o::" L":ooo:" L":::::" );
	SetChar( L'2', L":::::" L":ooo:" L"o:::o" L"::oo:" L":o:::" L"ooooo" L":::::" );
	SetChar( L'3', L":::::" L"oooo:" L"::::o" L"::oo:" L"::::o" L"oooo:" L":::::" );
	SetChar( L'4', L":::::" L"o::::" L"o::o:" L"ooooo" L":::o:" L":::o:" L":::::" );
	SetChar( L'5', L":::::" L"ooooo" L"o::::" L"oooo:" L"::::o" L"oooo:" L":::::" );
	SetChar( L'6', L":::::" L":oooo" L"o::::" L"oooo:" L"o:::o" L":ooo:" L":::::" );
	SetChar( L'7', L":::::" L"ooooo" L"::::o" L":::o:" L"::o::" L"::o::" L":::::" );
	SetChar( L'8', L":::::" L":ooo:" L"o:::o" L":ooo:" L"o:::o" L":ooo:" L":::::" );
	SetChar( L'9', L":::::" L":ooo:" L"o:::o" L":oooo" L"::::o" L":ooo:" L":::::" );
	SetChar( L'0', L":::::" L":ooo:" L"o::oo" L"o:o:o" L"oo::o" L":ooo:" L":::::" );
	SetChar( L' ', L":::::" L":::::" L":::::" L":::::" L":::::" L":::::" L":::::" );
	SetChar( L'/', L":::::" L"::::o" L":::o:" L"::o::" L":o:::" L"o::::" L":::::" );
}