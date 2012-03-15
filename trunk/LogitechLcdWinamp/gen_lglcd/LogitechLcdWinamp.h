#pragma once

#include "ScreenManager.h"
#include "Screens.h"

#define MAIN_TIMER_ID (0xdecaf)
#define CLOCK_TIMER_ID (0xcafe)
#define EMAIL_TIMER_ID (0xeeee)
#define TIMER_WAIT_FOR_LIB (0xaaaa)

class LogitechLcdWinamp
{
	HWND			m_Winamp;
	MainScreen*		m_MainScreen;

	static LogitechLcdWinamp* s_Instance;
	LogitechLcdWinamp() : m_MainScreen(0), m_Winamp(0) { };
	LogitechLcdWinamp(const LogitechLcdWinamp&);
	LogitechLcdWinamp& operator =(LogitechLcdWinamp& x) { return const_cast<LogitechLcdWinamp&>(x); }
	~LogitechLcdWinamp();

	void ProcessIPCMessage( DWORD lParam, DWORD wParam );
	void GetMetaData( wchar_t* buff, int buff_size, const wchar_t* file, const wchar_t* type );
	void SetFilename( const wchar_t* filename );
	void ProcessTimerMessage( WPARAM wParam );

	void deinit();

public:
	static LogitechLcdWinamp* getInstance();
	static void deleteInstance();

	int init( HWND );
	void config();

	static void ShowMessage( LPCTSTR );

	static void IPCMessage( DWORD lParam, DWORD wParam ) { getInstance()->ProcessIPCMessage( lParam, wParam ); }
	static void TimerMessage( WPARAM wParam ) { getInstance()->ProcessTimerMessage( wParam ); }
	static void SetString( TextID id, wchar_t* str ) { getInstance()->m_MainScreen->SetString( id, str ); }
	static void Update() { getInstance()->m_MainScreen->Update(); }

	static void EnableWaitForLibTimer() { SetTimer( getInstance()->m_Winamp, TIMER_WAIT_FOR_LIB, 1000, 0 ); }
};