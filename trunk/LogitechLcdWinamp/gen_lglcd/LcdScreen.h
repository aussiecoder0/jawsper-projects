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

DWORD WINAPI OnConfigureCB( IN int connection, IN const PVOID pContext );
DWORD WINAPI OnNotificationCB( IN int connection, IN const PVOID pContext, IN DWORD notificationCode, IN DWORD notifyParm1, IN DWORD notifyParm2, IN DWORD notifyParm3, IN DWORD notifyParm4 );
DWORD WINAPI OnSoftbuttonsCB(IN int device, IN DWORD dwButtons, IN const PVOID pContext);

/* abstract */ class LcdScreen
{
protected:
	int m_Connection;
	int m_Device;
	Surface* m_Surface;
	TextMap* m_Texts;
	bool m_Initialized;
	bool m_Functioning;
	bool m_Disabled;
	DWORD m_Priority;

	bool m_ButtonsPressed[4];
	bool m_ButtonsDown[4];

	bool LibLogitechInit();
	bool LibLogitechDeInit();
	bool LibLogitechConnect();
	bool LibLogitechDisconnect();
	bool LibLogitechOpen();
	bool LibLogitechClose();
	void LibLogitechSetForeground(bool);

	void Draw();

	bool ButtonPressed(int);

	virtual void ButtonsUpdate() = 0;
public:
	LcdScreen();
	virtual ~LcdScreen();

	void Update( bool draw = true, bool priority = false );

	void SetString( TextID, wchar_t* );
	
	DWORD OnConfigureCallback( int device );
	DWORD OnNofificationCallback( int, DWORD, DWORD );
	DWORD OnSoftButtonsCallback( int device, DWORD dwButtons );

	void LibReconnect();
};

