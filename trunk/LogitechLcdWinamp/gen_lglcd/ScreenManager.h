#pragma once

#include "inc/lglcd.h"
#include "Surface.h"
#include <map>
#include "TextID.h"
#include "Fonts.h"
#include "DrawableText.h"

using namespace std;

typedef map<TextID,DrawableText*> TextMap;

DWORD WINAPI OnSoftbuttonsCB(IN int device, IN DWORD dwButtons, IN const PVOID pContext);

class ScreenManager
{
	Font* m_TimeFont;
	int m_Connection;
	int m_Device;
	Surface* m_Surface;
	TextMap* m_Texts;
	bool m_Initialized;

	bool m_ButtonsPressed[4];
	bool m_ButtonsDown[4];


	void DrawInit();
	void Draw();

	bool ButtonPressed(int);

	void ButtonsUpdate();
public:
	ScreenManager();
	~ScreenManager();

	void Update( bool draw = true, bool priority = false );

	void SetString( TextID, wchar_t* );
	void SetProgress( int val, int min, int max );

	DWORD OnSoftButtonsCallback( int device, DWORD dwButtons );
};

