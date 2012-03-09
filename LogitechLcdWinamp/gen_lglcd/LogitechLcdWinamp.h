#pragma once

#include "ScreenManager.h"
#include "inc/wa_ipc.h"

class LogitechLcdWinamp
{
	HWND m_Winamp;
	ScreenManager* m_ScreenManager;
	UINT m_TimerID;

	static LogitechLcdWinamp* s_Instance;
	LogitechLcdWinamp() : m_ScreenManager(0), m_Winamp(0), m_TimerID(0xFF) { };
	~LogitechLcdWinamp();

	void ProcessIPCMessage( DWORD lParam, DWORD wParam );
	void GetMetaData( wchar_t* buff, int buff_size, const wchar_t* file, const wchar_t* type );
	void SetFilename( const wchar_t* filename );
	void ProcessTimerMessage( WPARAM wParam );

public:
	static LogitechLcdWinamp* getInstance();
	static void deleteInstance();

	int init( HWND );
	void config();

	void ShowMessage( LPCTSTR );

	static void IPCMessage( DWORD lParam, DWORD wParam ) { getInstance()->ProcessIPCMessage( lParam, wParam ); }
	static void TimerMessage( WPARAM wParam ) { getInstance()->ProcessTimerMessage( wParam ); }
	static void SetString( TextID id, wchar_t* str ) { getInstance()->m_ScreenManager->SetString( id, str ); }
	static void Update() { getInstance()->m_ScreenManager->Update(); }
};