// LogitechLcdWinamp.h

#pragma once

#include "ScreenManager.h"

class LogitechLcdWinamp
{
	static LogitechLcdWinamp* s_instance;
	LogitechLcdWinamp() {};
	~LogitechLcdWinamp();

	HWND m_Winamp;
	ScreenManager* m_screenManager;
public:
	static LogitechLcdWinamp* getInstance();
	static void deleteInstance();

	int init( HWND );
	void config();

	void ShowMessage( LPCTSTR );
};