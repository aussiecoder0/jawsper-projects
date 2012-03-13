#include "StdAfx.h"
#include "ScreenManager.h"

ScreenManager::ScreenManager()
{
	m_Initialized = false;
	m_Texts = new TextMap();
	m_Surface = new Surface( );
	m_TimeFont = new Font7x5Time();
	
	#define make_text_c(lbl,dt) m_Texts->insert( make_pair( lbl, dt ) )
	#define make_text(lbl,x,y,w) make_text_c( lbl, new DrawableText( x, y, w ) )
	#define make_text_f(lbl,x,y,w,f) make_text_c( lbl, new DrawableText( x, y, w, f ) )

	make_text( TXT_ARTIST, 0, 0, LCD_W );
	make_text( TXT_TITLE, 0, 8, LCD_W );
	make_text( TXT_ALBUM, 0, 16, LCD_W );
#define TIME_YPOS (LCD_H - 1 - 6 - 7)
	make_text_f( TXT_TIME_POS, 0, TIME_YPOS, 25, m_TimeFont );
	make_text_f( TXT_TIME_LENGTH, LCD_W, TIME_YPOS, -25, m_TimeFont );
		
	make_text_f( TXT_PL_DISP, LCD_W, 0, -53, m_TimeFont );

	make_text_f( TXT_CLOCK, (LCD_W / 2) - (39 / 2), TIME_YPOS, 39, m_TimeFont );
	
	#undef make_text
	#undef make_text_f
	#undef make_text_c

	DWORD retval;

	retval = lgLcdInit();

	for( int i = 0; i < 4; ++i ) m_ButtonsDown[i] = m_ButtonsPressed[i] = false;

	lgLcdConnectContextEx cctx = 
	{
		PLUGIN_NAME,				// appFriendlyName
		false,						// isPersistent
		false,						// isAutostartable
		{ 0, 0 },					// onConfigure
		LGLCD_INVALID_CONNECTION,	// connection
		LGLCD_APPLET_CAP_BW,		// dwAppletCapabilitiesSupported
		0,							// dwReserved1
		{ 0, 0 }					// onNotify
	};

	retval = lgLcdConnectEx( &cctx );

	if( ERROR_SUCCESS == retval )
	{
		m_Connection = cctx.connection;

		lgLcdOpenByTypeContext octx = 
		{
			m_Connection,
			LGLCD_DEVICE_BW,
			{ &OnSoftbuttonsCB, this },
			0
		};

		if( ERROR_SUCCESS == lgLcdOpenByType( &octx ) )
		{

			m_Device = octx.device;

			m_Initialized = true;

			DrawInit();
		
			Update();

		}
	}
	else
	{
		if( ERROR_SERVICE_NOT_ACTIVE == retval )
		{
			MessageBox( 0, _T("lgLcdConnectEx ---> ERROR_SERVICE_NOT_ACTIVE\n"), _T(""), MB_OK );
		}

		m_Connection = LGLCD_INVALID_CONNECTION;
	}
}


ScreenManager::~ScreenManager()
{
	if( LGLCD_INVALID_DEVICE != m_Device )
	{
		lgLcdClose( m_Device );
		m_Device = LGLCD_INVALID_DEVICE;
	}
	if( LGLCD_INVALID_CONNECTION != m_Connection )
    {
        lgLcdDisconnect( m_Connection );
        m_Connection = LGLCD_INVALID_CONNECTION;
    }

	lgLcdDeInit();

	for( TextMap::iterator iter = m_Texts->begin(); iter != m_Texts->end(); ++iter )
	{
		delete iter->second;
	}
	delete m_Texts;
	delete m_Surface;
	delete m_TimeFont;
}


void ScreenManager::DrawInit()
{
	m_Surface->BoxAbs( 0, LCD_H - 1 - 6, LCD_W - 1, LCD_H - 1 );
}

void ScreenManager::Draw()
{
	for( TextMap::const_iterator iter = m_Texts->begin(); iter != m_Texts->end(); iter++ )
	{
		iter->second->Draw( m_Surface );
	}
}

void ScreenManager::Update( bool a_Draw, bool a_Priority )
{
	if( !m_Initialized ) return;
	if( a_Draw ) Draw();
	DWORD retval = lgLcdUpdateBitmap( m_Device, m_Surface->Get(), a_Priority ? LGLCD_PRIORITY_ALERT : LGLCD_PRIORITY_NORMAL );
	if( ERROR_SUCCESS != retval )
	{
		wchar_t str[MAX_PATH];
		wsprintf( str, _T("Error %d"), retval );
		MessageBox( 0, str, PLUGIN_NAME, MB_OK );
	}
}

void ScreenManager::SetString( TextID a_Id, wchar_t* a_Str )
{
	TextMap::iterator iter = m_Texts->find( a_Id );
	if( iter != m_Texts->end() )
	{
		iter->second->SetText( a_Str );
	}
}

void ScreenManager::SetProgress( int val, int min, int max )
{
	m_Surface->BarAbs( 2, LCD_H - 1 - 4, LCD_W - 3, LCD_H - 3, PIXEL_OFF );
	if( val > 0 && max - min > 0 )
	{
		int x2 = static_cast<int>(( (double)( val - min ) / (double)( max - min ) ) * (double)( LCD_W - 5 ) + 2.0);
		if( x2 > 2 ) m_Surface->BarAbs( 2, LCD_H - 1 - 4, x2, LCD_H - 3, PIXEL_ON );
	}
}


DWORD WINAPI OnSoftbuttonsCB(IN int device, IN DWORD dwButtons, IN const PVOID pContext)
{
	return static_cast<ScreenManager*>(pContext)->OnSoftButtonsCallback( device, dwButtons );
}

DWORD ScreenManager::OnSoftButtonsCallback( int device, DWORD dwButtons )
{
	if( m_Device != device ) return 0;

	int i;
	for( i = 0; i < 4; ++i )
	{
		if( ( dwButtons >> i ) & 0x01 )
		{
			if( !m_ButtonsDown[i] )
				m_ButtonsDown[i] = true;
		}
		else
		{
			if( m_ButtonsDown[i] )
			{
				m_ButtonsDown[i] = false;
				m_ButtonsPressed[i] = true;
			}
		}
	}

	ButtonsUpdate();

	return 0;
}

bool ScreenManager::ButtonPressed(int num)
{
	if( m_ButtonsPressed[num % 4] )
	{
		m_ButtonsPressed[num % 4] = false;
		return true;
	}
	return false;
}

void ScreenManager::ButtonsUpdate()
{
	if( ButtonPressed( 0 ) )
	{
	}
}