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
	int x, y, w, mw;
	wchar_t str[MAX_PATH];
	Font* m_Font;
public:
	DrawableText(int a_x, int a_y, int a_w, Font* a_Font = 0) : 
	  x(a_x), y(a_y), w(a_w), mw(a_w), m_Changed(false), m_Font(a_Font)
		{ wcscpy_s( str, MAX_PATH, L"" ); }

	void SetText( wchar_t* );
	void Draw( Surface* );
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
public:
	ScreenManager(void);
	~ScreenManager(void);

	void Draw();
	void Update();

	void SetString( TextID, wchar_t* );
};

