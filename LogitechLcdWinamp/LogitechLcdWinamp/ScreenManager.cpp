#include "StdAfx.h"
#include "ScreenManager.h"

ScreenManager::ScreenManager(void)
{
	DWORD retval;

	retval = lgLcdInit();


	lgLcdConnectContextEx cctx = 
	{
		_T(PLUGIN_NAME),			// appFriendlyName
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
		m_connection = cctx.connection;

		lgLcdOpenByTypeContext octx = 
		{
			m_connection,
			LGLCD_DEVICE_BW,
			0,
			0
		};

		lgLcdOpenByType( &octx );

		m_device = octx.device;

		m_surface = new Surface( );

		m_surface->BoxAbs( 0, LCD_H - 1 - 6, LCD_W - 1, LCD_H - 1 );
		m_surface->BarAbs( 2, LCD_H - 1 - 4, LCD_W - 3, LCD_H - 3 );

		//m_surface->Print( "Nu een realistische zin.", 0, 2 );
		//m_surface->Print( "AaBbCcDdEeFfGgHhIiJjKkLlMm", 0, 7 );
		//m_surface->Print( "NnOoPpQqRrSsTtUuVvWwXxYyZz", 0, 14 );
		//m_surface->Print( " !\"#$%&'()*+,-./:;<=>?", 0, 21 );
		//m_surface->Print( "@[\\]^_`{|}~", 0, 28 );
		
		Update();
	}
	else
	{
		if( ERROR_SERVICE_NOT_ACTIVE == retval )
		{
			MessageBox( 0, _T("lgLcdConnectEx ---> ERROR_SERVICE_NOT_ACTIVE\n"), _T(""), MB_OK );
		}

		m_connection = LGLCD_INVALID_CONNECTION;
	}
}


ScreenManager::~ScreenManager(void)
{
	delete m_surface;
	if( LGLCD_INVALID_DEVICE != m_device )
	{
		lgLcdClose( m_device );
		m_device = LGLCD_INVALID_DEVICE;
	}
	if( LGLCD_INVALID_CONNECTION != m_connection )
    {
        lgLcdDisconnect( m_connection );
        m_connection = LGLCD_INVALID_CONNECTION;
    }

	lgLcdDeInit();
}

void ScreenManager::Update()
{
	DWORD retval = lgLcdUpdateBitmap( m_device, m_surface->Get(), LGLCD_PRIORITY_ALERT );
	if( ERROR_SUCCESS != retval )
	{
		wchar_t str[MAX_PATH];
		wsprintf( str, _T("Error %d"), retval );
		MessageBox( 0, str, _T(PLUGIN_NAME), MB_OK );
	}
}