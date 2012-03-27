#include "stdafx.h"

MainScreen::MainScreen() : ScreenManager(), m_HasMail( false )
{
	m_Font = new Font7x5();
	m_TimeFont = new Font7x5Time();
	
	#define make_text_c(lbl,dt) m_Texts->insert( make_pair( lbl, dt ) )
	#define make_text(lbl,x,y,w) make_text_c( lbl, new DrawableText( x, y, w, m_Font ) )
	#define make_text_time(lbl,x,y,w) make_text_c( lbl, new DrawableText( x, y, w, m_TimeFont ) )

	make_text( TXT_ARTIST, 0, 0, m_Surface->GetWidth() );
	make_text_time( TXT_PL_DISP, m_Surface->GetWidth(), 0, -53 );

	make_text( TXT_TITLE, 0, 8, m_Surface->GetWidth() );

	make_text( TXT_ALBUM, 0, 16, m_Surface->GetWidth() );
	
	make_text( TXT_STATUS, 0, 23, 80 );
	make_text( TXT_EMAIL, m_Surface->GetWidth(), 23, -70 );

#define TIME_YPOS (m_Surface->GetHeight() - 1 - 5 - 7)
	make_text_time( TXT_TIME_POS, 0, TIME_YPOS, 25 );
	make_text_time( TXT_CLOCK, (m_Surface->GetWidth() / 2) - (39 / 2), TIME_YPOS, 39 );
	make_text_time( TXT_TIME_LENGTH, m_Surface->GetWidth(), TIME_YPOS, -25 );
	
	#undef make_text
	#undef make_text_time
	#undef make_text_c

	DrawInit();
}

MainScreen::~MainScreen()
{
	delete m_Font;
	delete m_TimeFont;
}

void MainScreen::DrawInit()
{
	m_Surface->BoxAbs( 0, m_Surface->GetHeight() - 1 - 5, m_Surface->GetWidth() - 1, m_Surface->GetHeight() - 1 );
}

void MainScreen::UpdateMailCount()
{
	unsigned int total = 0;
	
	GetUnreadMailCount( m_MailCount );
	for( size_t i = 0; i < m_MailCount.size(); i++ )
	{
		total += m_MailCount[i].count;
	}
	//total = UnreadMailCount();
	m_HasMail = total > 0;
	static wchar_t buff[MAX_PATH];
	if( m_HasMail )
	{
		wsprintf( buff, total == 1 ? _T("%d e-mail") : _T("%d e-mails"), total );
	}
	else
	{
		wcscpy_s( buff, _T("") );
	}
	SetString( TXT_EMAIL, buff );
}

void MainScreen::ButtonsUpdate()
{
	if( ButtonPressed( 3 ) )
	{
		// 0x4A == J
		keybd_event( VK_CONTROL, 0, 0, 0 );
		keybd_event( VK_MENU, 0, 0, 0 );
		keybd_event( 0x4A, 0, 0, 0 );
		keybd_event( 0x4A, 0, KEYEVENTF_KEYUP, 0 );
		keybd_event( VK_MENU, 0, KEYEVENTF_KEYUP, 0 );
		keybd_event( VK_CONTROL, 0, KEYEVENTF_KEYUP, 0 );
	}
}

void MainScreen::SetProgress( int val, int min, int max )
{
	m_Surface->BarAbs( 2, m_Surface->GetHeight() - 1 - 3, m_Surface->GetWidth() - 3, m_Surface->GetHeight() - 3, PIXEL_OFF );
	if( val > 0 && max - min > 0 )
	{
		int x2 = static_cast<int>(( (double)( val - min ) / (double)( max - min ) ) * (double)( m_Surface->GetWidth() - 5 ) + 2.0);
		if( x2 > 2 ) m_Surface->BarAbs( 2, m_Surface->GetHeight() - 1 - 3, x2, m_Surface->GetHeight() - 3, PIXEL_ON );
	}
}

void MainScreen::SetPlayingState( int state )
{
	switch( state )
	{
		case 1: // playing
			SetString( TXT_STATUS, _T("Playing") );
			break;
		case 3: // paused
			SetString( TXT_STATUS, _T("Paused") );
			break;
		case 0: // not playing
			SetString( TXT_STATUS, _T("Stopped") );
			break;
	}
}