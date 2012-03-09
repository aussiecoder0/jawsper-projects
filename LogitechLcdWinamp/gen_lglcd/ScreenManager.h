#pragma once

#include "inc/lglcd.h"
#include "Surface.h"
#include <map>
#include "TextID.h"
#include "Fonts.h"

using namespace std;

class DrawableText
{
	bool m_Changed;
	int m_X, m_Y, m_Width, m_MaxWidth;
	wchar_t m_Str[MAX_PATH];
	Font* m_Font;
	int prev_len;
public:
	DrawableText(int a_X, int a_Y, int a_Width, Font* a_Font = 0) : 
	  m_X(a_X), m_Y(a_Y), m_Width(a_Width), m_MaxWidth(a_Width), m_Changed(false), m_Font(a_Font)
		{ wcscpy_s( m_Str, MAX_PATH, L"" ); prev_len = -1; }

	void SetText( wchar_t* a_Str );
	bool Draw( Surface* a_Surface );
};

typedef map<TextID,DrawableText*> TextMap;

class ScreenManager
{
	Font* m_TimeFont;
	int m_Connection;
	int m_Device;
	Surface* m_Surface;
	TextMap* m_Texts;
	bool m_Initialized;
	void DrawInit();
	void Draw();
public:
	ScreenManager();
	~ScreenManager();

	void Update( bool draw = true );

	void SetString( TextID, wchar_t* );
	void SetProgress( int val, int min, int max );
};

