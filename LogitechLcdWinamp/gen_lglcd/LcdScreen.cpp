#include "StdAfx.h"
#include "LcdScreen.h"

LcdScreen::LcdScreen() : 
		m_Initialized( false ), m_Functioning( false ), 
		m_Disabled( true ), m_Priority( LGLCD_PRIORITY_NORMAL )
{
	m_Texts = new TextMap();
	m_Surface = new Surface();

	for( int i = 0; i < 4; ++i ) m_ButtonsDown[i] = m_ButtonsPressed[i] = false;


	if( LibLogitechInit() )
	{
		if( !LibLogitechConnect() )
		{
			m_Connection = LGLCD_INVALID_CONNECTION;
			LogitechLcdWinamp::EnableWaitForLibTimer();
		}
	}
}


LcdScreen::~LcdScreen()
{
	LibLogitechClose();
	LibLogitechDisconnect();
	LibLogitechDeInit();

	for( TextMap::iterator iter = m_Texts->begin(); iter != m_Texts->end(); ++iter )
	{
		delete iter->second;
	}
	delete m_Texts;
	
	delete m_Surface;
}

void LcdScreen::Draw()
{
	for( TextMap::iterator iter = m_Texts->begin(); iter != m_Texts->end(); iter++ )
	{
		iter->second->Draw( m_Surface );
	}
}

void LcdScreen::Update( bool a_Draw, bool a_Priority )
{
	if( !m_Initialized || !m_Functioning || m_Disabled ) return;

	if( a_Draw ) Draw();

	DWORD retval = lgLcdUpdateBitmap( m_Device, m_Surface->Get(), a_Priority ? LGLCD_PRIORITY_ALERT : m_Priority );

	m_Functioning = retval == ERROR_SUCCESS;
}

void LcdScreen::SetString( TextID a_Id, wchar_t* a_Str )
{
	TextMap::iterator iter = m_Texts->find( a_Id );
	if( iter != m_Texts->end() )
	{
		iter->second->SetText( a_Str );
	}
}

#pragma region LibLogitech functions

bool LcdScreen::LibLogitechInit()
{
	DWORD status = lgLcdInit();
	return ERROR_SUCCESS == status || ERROR_ALREADY_INITIALIZED == status;
}
bool LcdScreen::LibLogitechDeInit()
{
	DWORD status = lgLcdDeInit();
	return ERROR_SUCCESS == status;
}

bool LcdScreen::LibLogitechConnect()
{
	lgLcdConnectContextEx cctx = 
	{
		PLUGIN_NAME,				// appFriendlyName
		false,						// isPersistent
		false,						// isAutostartable
		{ &OnConfigureCB, this },	// onConfigure
		LGLCD_INVALID_CONNECTION,	// connection
		LGLCD_APPLET_CAP_BW,		// dwAppletCapabilitiesSupported
		0,							// dwReserved1
		{ &OnNotificationCB, this }	// onNotify
	};
	
	DWORD status = lgLcdConnectEx( &cctx );
	if( ERROR_SUCCESS == status )
	{
		m_Connection = cctx.connection;
		return true;
	}
	return false;
}
bool LcdScreen::LibLogitechDisconnect()
{
	if( LGLCD_INVALID_CONNECTION != m_Connection )
    {
        DWORD status = lgLcdDisconnect( m_Connection );
        m_Connection = LGLCD_INVALID_CONNECTION;
		return ERROR_SUCCESS == status;
    }
	return false;
}

bool LcdScreen::LibLogitechOpen()
{
	lgLcdOpenByTypeContext octx = 
	{
		m_Connection,
		LGLCD_DEVICE_BW,
		{ &OnSoftbuttonsCB, this },
		0
	};

	DWORD status = lgLcdOpenByType( &octx );
	if( ERROR_SUCCESS == status )
	{
		m_Device = octx.device;
		return true;
	}
	return false;
}
bool LcdScreen::LibLogitechClose()
{
	if( LGLCD_INVALID_DEVICE != m_Device )
	{
		DWORD status = lgLcdClose( m_Device );
		m_Device = LGLCD_INVALID_DEVICE;
		return ERROR_SUCCESS == status;
	}
	return false;
}

void LcdScreen::LibLogitechSetForeground( bool a_Priority )
{
	lgLcdSetAsLCDForegroundApp( m_Device, a_Priority ? LGLCD_LCD_FOREGROUND_APP_YES : LGLCD_LCD_FOREGROUND_APP_NO );
}

void LcdScreen::LibReconnect()
{
	if( !LibLogitechConnect() )
	{
		m_Connection = LGLCD_INVALID_CONNECTION;
	}
}

#pragma endregion

#pragma region Configuration

DWORD WINAPI OnConfigureCB( int device, const PVOID pContext )
{
	return static_cast<LcdScreen*>( pContext )->OnConfigureCallback( device );
}

DWORD LcdScreen::OnConfigureCallback( int device )
{
	if( device != m_Device ) return 0;
	LogitechLcdWinamp::ShowMessage( _T("Don't configure me, bro!") );
	return 0;
}

#pragma endregion

#pragma region Notifications

#pragma warning( disable: 4100 ) // unreferenced formal parameter
DWORD WINAPI OnNotificationCB( IN int connection, IN const PVOID pContext, IN DWORD notificationCode, IN DWORD notifyParm1, IN DWORD notifyParm2, IN DWORD notifyParm3, IN DWORD notifyParm4 )
{
	return static_cast<LcdScreen*>( pContext )->OnNofificationCallback( connection, notificationCode, notifyParm1 );
}
#pragma warning( default: 4100 )

DWORD LcdScreen::OnNofificationCallback( int connection, DWORD notificationCode, DWORD notifyParm1 )
{
	if( connection != m_Connection ) return 0;

	switch( notificationCode )
	{
		case LGLCD_NOTIFICATION_DEVICE_ARRIVAL:
			if( notifyParm1 == LGLCD_DEVICE_BW )
			{
				if( LibLogitechOpen() )
				{
					m_Initialized = true;
					m_Functioning = true;
		
					Update();
				}
				else
				{
					m_Device = LGLCD_INVALID_DEVICE;
				}
			}
			break;
		case LGLCD_NOTIFICATION_DEVICE_REMOVAL:
			if( notifyParm1 == LGLCD_DEVICE_BW )
			{
				LibLogitechClose();
				m_Initialized = m_Functioning = false;
			}
			break;
		case LGLCD_NOTIFICATION_APPLET_ENABLED:
			m_Disabled = false;
			//LibLogitechSetForeground( true );
			Update( true, true );
			break;
		case LGLCD_NOTIFICATION_APPLET_DISABLED:
			m_Disabled = true;
			break;
		case LGLCD_NOTIFICATION_CLOSE_CONNECTION:
			m_Device = LGLCD_INVALID_DEVICE;
			m_Connection = LGLCD_INVALID_CONNECTION;
			m_Functioning = false;
			LogitechLcdWinamp::EnableWaitForLibTimer();
			break;
	}
	return 0;
}

#pragma endregion

#pragma region Softbuttons

DWORD WINAPI OnSoftbuttonsCB( IN int device, IN DWORD dwButtons, IN const PVOID pContext )
{
	return static_cast<LcdScreen*>(pContext)->OnSoftButtonsCallback( device, dwButtons );
}

DWORD LcdScreen::OnSoftButtonsCallback( int device, DWORD dwButtons )
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

	Update();

	return 0;
}

bool LcdScreen::ButtonPressed(int num)
{
	if( m_ButtonsPressed[num % 4] )
	{
		m_ButtonsPressed[num % 4] = false;
		return true;
	}
	return false;
}

#pragma endregion