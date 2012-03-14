#include "StdAfx.h"
#include "ScreenManager.h"

ScreenManager::ScreenManager() : m_Initialized( false )
{
	m_Texts = new TextMap();

	m_Surface = new Surface();

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
}

void ScreenManager::Draw()
{
	for( TextMap::iterator iter = m_Texts->begin(); iter != m_Texts->end(); iter++ )
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

	Update();

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