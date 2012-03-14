#pragma once

#include "MailCount.h"
#include "inc/lglcd.h"
#include "Surface.h"
#include <map>
#include "TextID.h"
#include "Fonts.h"
#include "DrawableText.h"

using namespace std;

typedef map<TextID,DrawableText*> TextMap;

DWORD WINAPI OnSoftbuttonsCB(IN int device, IN DWORD dwButtons, IN const PVOID pContext);

/* abstract */ class ScreenManager
{
protected:
	int m_Connection;
	int m_Device;
	Surface* m_Surface;
	TextMap* m_Texts;
	bool m_Initialized;

	bool m_ButtonsPressed[4];
	bool m_ButtonsDown[4];

	void Draw();

	bool ButtonPressed(int);

	virtual void ButtonsUpdate() = 0;
public:
	ScreenManager();
	virtual ~ScreenManager();

	void Update( bool draw = true, bool priority = false );

	void SetString( TextID, wchar_t* );

	DWORD OnSoftButtonsCallback( int device, DWORD dwButtons );
};

