#include "stdafx.h"
#include "inc/gen.h"
#include "inc/wa_ipc.h"

int  init();
void config();
void quit();

static WNDPROC oldWndProc;
static LRESULT WINAPI SubclassProc( HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam );

winampGeneralPurposePlugin plugin = { GPPHDR_VER, PLUGIN_NAME_SB, init, config, quit, 0, 0 };

int init()
{
	oldWndProc = (WNDPROC)SetWindowLongPtr( plugin.hwndParent, GWLP_WNDPROC, (LONG_PTR)SubclassProc );
	return LogitechLcdWinamp::getInstance()->init( plugin.hwndParent );
}

void config()
{
	LogitechLcdWinamp::getInstance()->config();
}

void quit()
{
	LogitechLcdWinamp::deleteInstance();
	SetWindowLongPtr( plugin.hwndParent, GWLP_WNDPROC, (LONG_PTR)oldWndProc );
}

// This is an export function called by winamp which returns this plugin info.
// We wrap the code in 'extern "C"' to ensure the export isn't mangled if used in a CPP file.
extern "C" __declspec(dllexport) winampGeneralPurposePlugin * winampGetGeneralPurposePlugin()
{
	return &plugin;
}

static LRESULT WINAPI SubclassProc( HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam )
{
	switch( msg )
	{
		case WM_WA_IPC:
			LogitechLcdWinamp::IPCMessage( lParam, wParam );
			break;
		case WM_TIMER:
			LogitechLcdWinamp::TimerMessage( wParam );
			break;
	}
	return CallWindowProc( oldWndProc, hwnd, msg, wParam, lParam );
}