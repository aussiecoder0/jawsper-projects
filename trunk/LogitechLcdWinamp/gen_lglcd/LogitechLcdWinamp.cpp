// This is the main DLL file.

#include "stdafx.h"

#include "LogitechLcdWinamp.h"

/* static */ LogitechLcdWinamp* LogitechLcdWinamp::s_instance = 0;

/* static */ LogitechLcdWinamp* LogitechLcdWinamp::getInstance()
{
	if( LogitechLcdWinamp::s_instance == 0 )
	{
		LogitechLcdWinamp::s_instance = new LogitechLcdWinamp();
	}
	return LogitechLcdWinamp::s_instance;
}

/* static */ void LogitechLcdWinamp::deleteInstance()
{
	delete LogitechLcdWinamp::s_instance;
}


LogitechLcdWinamp::~LogitechLcdWinamp()
{
	delete m_screenManager;
	//MessageBox(0, L"Quit event triggered for gen_myplugin.", L"", MB_OK);
}

int LogitechLcdWinamp::init( HWND winamp )
{
	m_Winamp = winamp;
	m_screenManager = new ScreenManager();
	return 0;
}

void LogitechLcdWinamp::config()
{
	MessageBox( m_Winamp, _T("I don't want to be configured yet."), _T(""), MB_OK );
}

void LogitechLcdWinamp::ShowMessage( LPCTSTR msg )
{
	MessageBox( m_Winamp, msg, PLUGIN_NAME, MB_OK );
}