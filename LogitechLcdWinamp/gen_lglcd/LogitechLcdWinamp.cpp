// This is the main DLL file.

#include "stdafx.h"

#include "LogitechLcdWinamp.h"

/* static */ LogitechLcdWinamp* LogitechLcdWinamp::s_Instance = 0;

/* static */ LogitechLcdWinamp* LogitechLcdWinamp::getInstance()
{
	if( LogitechLcdWinamp::s_Instance == 0 )
	{
		LogitechLcdWinamp::s_Instance = new LogitechLcdWinamp();
	}
	return LogitechLcdWinamp::s_Instance;
}

/* static */ void LogitechLcdWinamp::deleteInstance()
{
	delete LogitechLcdWinamp::s_Instance;
}


LogitechLcdWinamp::~LogitechLcdWinamp()
{
	delete m_ScreenManager;
	//ShowMessage( _T("Quit event triggered for gen_myplugin." );
}

int LogitechLcdWinamp::init( HWND winamp )
{
	m_Winamp = winamp;
	m_ScreenManager = new ScreenManager();

	m_ScreenManager->SetString( TXT_ARTIST, _T("Artist") );
	m_ScreenManager->SetString( TXT_TITLE, _T("Title") );
	m_ScreenManager->SetString( TXT_ALBUM, _T("Album") );
	m_ScreenManager->SetString( TXT_TIME_LEFT, _T("00:00") );
	m_ScreenManager->SetString( TXT_TIME_RIGHT, _T("12:34") );

	m_ScreenManager->Update();
	return 0;
}

void LogitechLcdWinamp::config()
{
	ShowMessage( _T("I don't want to be configured yet.") );
}

void LogitechLcdWinamp::ShowMessage( LPCTSTR msg )
{
	MessageBox( m_Winamp, msg, PLUGIN_NAME, MB_OK );
}