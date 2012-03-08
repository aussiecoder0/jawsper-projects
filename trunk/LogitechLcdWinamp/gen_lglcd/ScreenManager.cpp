#include "StdAfx.h"
#include "ScreenManager.h"

ScreenManager::ScreenManager(void)
{
	m_Initialized = false;
	m_Texts = new TextMap();
	m_Surface = new Surface( );
	m_TimeFont = new Font7x5Time();

	#define make_text(lbl,x,y,w) m_Texts->insert( make_pair( lbl, new DrawableText( x, y, w ) ) )
	#define make_text_f(lbl,x,y,w,f) m_Texts->insert( make_pair( lbl, new DrawableText( x, y, w, f ) ) )
	make_text( TXT_ARTIST, 0, 0, LCD_W );
	make_text( TXT_TITLE, 0, 8, LCD_W );
	make_text( TXT_ALBUM, 0, 16, LCD_W );
	make_text_f( TXT_TIME_LEFT, 0, LCD_H - 1 - 6 - 7, LCD_W, m_TimeFont );
	make_text_f( TXT_TIME_RIGHT, LCD_W - 25, LCD_H - 1 - 6 - 7, 25, m_TimeFont );
	#undef make_text
	#undef make_text_f

	DWORD retval;

	retval = lgLcdInit();


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
			0,
			0
		};

		lgLcdOpenByType( &octx );

		m_Device = octx.device;

		m_Initialized = true;
		
		Update();
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


ScreenManager::~ScreenManager(void)
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




void ScreenManager::Draw()
{
	for( TextMap::const_iterator iter = m_Texts->begin(); iter != m_Texts->end(); iter++ )
	{
		iter->second->Draw( m_Surface );
	}
	m_Surface->BoxAbs( 0, LCD_H - 1 - 6, LCD_W - 1, LCD_H - 1 );
	m_Surface->BarAbs( 2, LCD_H - 1 - 4, LCD_W - 3, LCD_H - 3 );
}

void ScreenManager::Update()
{
	if( !m_Initialized ) return;
	Draw();
	DWORD retval = lgLcdUpdateBitmap( m_Device, m_Surface->Get(), LGLCD_PRIORITY_ALERT );
	if( ERROR_SUCCESS != retval )
	{
		wchar_t str[MAX_PATH];
		wsprintf( str, _T("Error %d"), retval );
		MessageBox( 0, str, PLUGIN_NAME, MB_OK );
	}
}

void ScreenManager::SetString( TextID id, wchar_t* str )
{
	TextMap::iterator iter = m_Texts->find( id );
	if( iter != m_Texts->end() )
	{
		iter->second->SetText( str );
	}
}