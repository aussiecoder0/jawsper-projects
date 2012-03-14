#pragma once
#include "Fonts.h"

class DrawableText
{
	bool m_Changed;
	int m_X, m_Y, m_Width, m_MaxWidth;
	wchar_t m_Str[MAX_PATH];
	Font* m_Font;
	int prev_len;
public:
	DrawableText(int a_X, int a_Y, int a_Width, Font* a_Font ) : 
	  m_X(a_X), m_Y(a_Y), m_Width(a_Width), m_MaxWidth(a_Width), m_Changed(false), m_Font(a_Font)
		{ wcscpy_s( m_Str, MAX_PATH, L"" ); prev_len = -1; }

	void SetText( const wchar_t* a_Str );
	bool Draw( Surface* a_Surface );
};