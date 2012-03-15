// This is the main DLL file.

#include "stdafx.h"
#include "inc/wa_ipc.h"
#include "LogitechLcdWinamp.h"
#include <time.h>

/* static */ LogitechLcdWinamp* LogitechLcdWinamp::s_Instance = 0;

/* static */ LogitechLcdWinamp* LogitechLcdWinamp::getInstance()
{
	if( s_Instance == 0 )
	{
		s_Instance = new LogitechLcdWinamp();
	}
	return s_Instance;
}

/* static */ void LogitechLcdWinamp::deleteInstance()
{
	if( s_Instance != 0 )
	{
		delete s_Instance;
		s_Instance = 0;
	}
}

void LogitechLcdWinamp::deinit()
{
	KillTimer( m_Winamp, MAIN_TIMER_ID );
	KillTimer( m_Winamp, CLOCK_TIMER_ID );
	KillTimer( m_Winamp, EMAIL_TIMER_ID );
	delete m_MainScreen;
}

LogitechLcdWinamp::~LogitechLcdWinamp()
{
	deinit();
}

int LogitechLcdWinamp::init( HWND a_Winamp )
{
	m_Winamp = a_Winamp;
	m_MainScreen = new MainScreen();

	m_MainScreen->SetString( TXT_ARTIST, _T("Artist") );
	m_MainScreen->SetString( TXT_TITLE,  _T("Title") );
	m_MainScreen->SetString( TXT_ALBUM,  _T("Album") );
	m_MainScreen->SetString( TXT_TIME_POS,  _T("00:00") );
	m_MainScreen->SetString( TXT_TIME_LENGTH, _T("00:00") );
	m_MainScreen->SetString( TXT_PL_DISP, _T("0000/0000") );
	m_MainScreen->SetString( TXT_CLOCK, _T("00:00:00") );
	m_MainScreen->SetProgress( 0, 0, 0 );

	m_MainScreen->Update( false, true );

	SetTimer( m_Winamp, CLOCK_TIMER_ID, 100, 0 );
	SetTimer( m_Winamp, EMAIL_TIMER_ID, 1000, 0 );
	return 0;
}

void LogitechLcdWinamp::config()
{
	ShowMessage( _T("Why did you even bother clicking this button?") );
}

void LogitechLcdWinamp::ShowMessage( LPCTSTR msg )
{
	MessageBox( s_Instance->m_Winamp, msg, PLUGIN_NAME, MB_OK );
}

inline void format_time( int time, wchar_t* buff )
{
	if( time < 0 ) time = 0;
	wsprintf( buff, _T("%02d:%02d"), time / 60, time % 60 );
}
inline void format_playlist_num( int pos, int len, wchar_t* buff )
{
	// pos + 1 because winamp gives zero based pos back it seems!
	wsprintf( buff, _T("%04d/%04d"), pos + 1 , len );
}

#define SendIPCMessage(wParam,lParam) SendMessage( m_Winamp, WM_WA_IPC, (WPARAM)wParam, (LPARAM)lParam )

void LogitechLcdWinamp::ProcessIPCMessage( DWORD lParam, DWORD wParam )
{
	static wchar_t buff[ 1024 ];
	switch( lParam )
	{
		case IPC_CB_OUTPUTCHANGED:
		case IPC_CB_MISC:
			if( lParam != IPC_CB_MISC || wParam == IPC_CB_MISC_STATUS )
			{
				int list_pos = SendIPCMessage( 0, IPC_GETLISTPOS );
				SetFilename( (const wchar_t*) SendIPCMessage( list_pos, IPC_GETPLAYLISTFILEW ) );
				format_playlist_num( list_pos, SendIPCMessage( 0, IPC_GETLISTLENGTH ), buff );
				m_MainScreen->SetString( TXT_PL_DISP, buff );

				// track length in seconds
				int track_length = SendIPCMessage( 1, IPC_GETOUTPUTTIME );
				format_time( track_length, buff );
				m_MainScreen->SetString( TXT_TIME_LENGTH, buff );

				SetTimer( m_Winamp, MAIN_TIMER_ID, USER_TIMER_MINIMUM, 0 );

				m_MainScreen->Update();
			}
			break;
	}
}

void LogitechLcdWinamp::GetMetaData( wchar_t* buff, int buff_size, const wchar_t* file, const wchar_t* type )
{
	extendedFileInfoStructW efw =
	{
		file,
		type,
		buff, 
		buff_size
	};
	SendIPCMessage( &efw, IPC_GET_EXTENDED_FILE_INFOW );
}

void LogitechLcdWinamp::SetFilename( const wchar_t* filename )
{
	static wchar_t buff[ 1024 ];
#define SetMeta(ID,type) \
	GetMetaData( buff, 1024, filename, _T(type) );\
	m_MainScreen->SetString( ID, buff )
	SetMeta( TXT_ARTIST, "artist" );
	SetMeta( TXT_TITLE,  "title"  );
	SetMeta( TXT_ALBUM,  "album"  );
#undef SetMeta
}

void LogitechLcdWinamp::ProcessTimerMessage( WPARAM wParam )
{
	static wchar_t buff[ 1024 ];
	switch( wParam )
	{
		case MAIN_TIMER_ID:
			{
				int track_pos = SendIPCMessage( 0, IPC_GETOUTPUTTIME ) / 1000;
				int track_length = SendIPCMessage( 1, IPC_GETOUTPUTTIME );

				format_time( track_pos, buff );
				m_MainScreen->SetString( TXT_TIME_POS, buff );
				
				m_MainScreen->SetProgress( track_pos, 0, track_length );

				int state = SendIPCMessage( 0, IPC_ISPLAYING );

				if( state > 0 )
					SetTimer( m_Winamp, MAIN_TIMER_ID, 100, 0 );
				else
					KillTimer( m_Winamp, MAIN_TIMER_ID );

				m_MainScreen->SetPlayingState( state );
			}
			m_MainScreen->Update();
			break;
		case CLOCK_TIMER_ID:
			time_t rawtime;
			struct tm timeinfo;
			time( &rawtime );
			localtime_s( &timeinfo, &rawtime );
			wcsftime( buff, 1024, _T("%X"), &timeinfo );

			m_MainScreen->SetString( TXT_CLOCK, buff );
			m_MainScreen->Update();
			break;
		case EMAIL_TIMER_ID:
			m_MainScreen->UpdateMailCount();
			break;
		case TIMER_WAIT_FOR_LIB:
			m_MainScreen->LibReconnect();
			break;
	}
}