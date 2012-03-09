// This is the main DLL file.

#include "stdafx.h"
#include "inc/wa_ipc.h"
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
}

int LogitechLcdWinamp::init( HWND winamp )
{
	m_Winamp = winamp;
	m_ScreenManager = new ScreenManager();

	m_ScreenManager->SetString( TXT_ARTIST, _T("Artist") );
	m_ScreenManager->SetString( TXT_TITLE,  _T("Title") );
	m_ScreenManager->SetString( TXT_ALBUM,  _T("Album") );
	m_ScreenManager->SetString( TXT_TIME_POS,  _T("00:00") );
	m_ScreenManager->SetString( TXT_TIME_LENGTH, _T("00:00") );
	m_ScreenManager->SetString( TXT_PL_DISP, _T("0000/0000") );
	m_ScreenManager->SetProgress( 0, 0, 0 );

	m_ScreenManager->Update( false );
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
				m_ScreenManager->SetString( TXT_PL_DISP, buff );

				// track length in seconds
				int track_length = SendIPCMessage( 1, IPC_GETOUTPUTTIME );
				format_time( track_length, buff );
				m_ScreenManager->SetString( TXT_TIME_LENGTH, buff );

				SetTimer( m_Winamp, m_TimerID, USER_TIMER_MINIMUM, 0 );

				m_ScreenManager->Update();
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
	GetMetaData( buff, MAX_PATH + 1, filename, _T(type) );\
	m_ScreenManager->SetString( ID, buff )
	SetMeta( TXT_ARTIST, "artist" );
	SetMeta( TXT_TITLE,  "title"  );
	SetMeta( TXT_ALBUM,  "album"  );
#undef SetMeta
}

void LogitechLcdWinamp::ProcessTimerMessage( WPARAM wParam )
{
	if( wParam != m_TimerID ) return;
	static wchar_t buff[ 1024 ];

	int track_pos = SendIPCMessage( 0, IPC_GETOUTPUTTIME ) / 1000;
	int track_length = SendIPCMessage( 1, IPC_GETOUTPUTTIME );

	format_time( track_pos, buff );
	m_ScreenManager->SetString( TXT_TIME_POS, buff );
				
	m_ScreenManager->SetProgress( track_pos, 0, track_length );

	m_ScreenManager->Update();

	int is_playing = SendIPCMessage( 0, IPC_ISPLAYING );

	if( is_playing > 0 )	SetTimer( m_Winamp, m_TimerID, 100, 0 );
	else					KillTimer( m_Winamp, m_TimerID );
}