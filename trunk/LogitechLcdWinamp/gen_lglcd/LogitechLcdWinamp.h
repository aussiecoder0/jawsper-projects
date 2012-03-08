// LogitechLcdWinamp.h

#pragma once

#include "ScreenManager.h"
#include "inc/wa_ipc.h"

class LogitechLcdWinamp
{
	static LogitechLcdWinamp* s_Instance;
	LogitechLcdWinamp() {};
	~LogitechLcdWinamp();

	HWND m_Winamp;
	ScreenManager* m_ScreenManager;
public:
	static LogitechLcdWinamp* getInstance();
	static void deleteInstance();

	int init( HWND );
	void config();

	void ShowMessage( LPCTSTR );
};