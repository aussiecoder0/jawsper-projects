// gen_lglcd.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "gen_lglcd.h"
#include "inc/gen.h"
#include "inc/wa_ipc.h"
#include <fstream>

using namespace std;

// these are callback functions/events which will be called by Winamp
int  init(void);
void config(void);
void quit(void);
WNDPROC oldWndProc;
static LRESULT WINAPI SubclassProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);

// this structure contains plugin information, version, name...
// GPPHDR_VER is the version of the winampGeneralPurposePlugin (GPP) structure
winampGeneralPurposePlugin plugin =
{
	GPPHDR_VER,  // version of the plugin, defined in "gen_myplugin.h"
	PLUGIN_NAME_SB, // name/title of the plugin, defined in "gen_myplugin.h"
	init,        // function name which will be executed on init event
	config,      // function name which will be executed on config event
	quit,        // function name which will be executed on quit event
	0,           // handle to Winamp main window, loaded by winamp when this dll is loaded
	0            // hinstance to this dll, loaded by winamp when this dll is loaded
};

fstream logger;

int init()
{
	logger.open("gen_lglcd.log.txt", fstream::out | fstream::trunc );
	oldWndProc = (WNDPROC)SetWindowLongPtr( plugin.hwndParent, GWLP_WNDPROC, (LONG_PTR)SubclassProc );
	return LogitechLcdWinamp::getInstance()->init(plugin.hwndParent);
}

void config()
{
	LogitechLcdWinamp::getInstance()->config();
	logger << "---------------------" << endl;
}

void quit()
{
	LogitechLcdWinamp::deleteInstance();
	SetWindowLongPtr( plugin.hwndParent, GWLP_WNDPROC, (LONG_PTR)oldWndProc );
	logger.close();
}

// This is an export function called by winamp which returns this plugin info.
// We wrap the code in 'extern "C"' to ensure the export isn't mangled if used in a CPP file.
extern "C" __declspec(dllexport) winampGeneralPurposePlugin * winampGetGeneralPurposePlugin()
{
	return &plugin;
}

char* lParamName( LPARAM lParam, bool& care )
{
	switch( lParam )
	{
#define msgi(lp) case lp: care = true; return #lp
#define msg(lp) case lp: care = false; return 0
		msgi( IPC_GETPLAYLISTFILE );
		msgi( IPC_GETPLAYLISTFILEW );

		msg( IPC_CB_PEINFOTEXT );
		msg( IPC_REGISTER_WINAMP_IPCMESSAGE );
		msg( IPC_GET_API_SERVICE );
		msg( IPC_GETINIFILE );
		msg( IPC_CB_GETTOOLTIP );
		msg( IPC_CB_ONTOGGLEAOT );
		msg( IPC_GET_GENSKINBITMAP );
		msg( IPC_CB_RESETFONT );
		msg( IPC_SETIDEALVIDEOSIZE );
		msg( IPC_EMBED_ENUM );
		msg( IPC_ADD_DISPATCH_OBJECT );
		msg( IPC_GETINIDIRECTORY );
		msg( IPC_GETPLUGINDIRECTORY );
		msg( IPC_GET_HMENU );
		msg( IPC_ADJUST_OPTIONSMENUPOS );
		msg( IPC_ADJUST_FFWINDOWSMENUPOS );
		msg( IPC_ADD_PREFS_DLGW );
		msg( IPC_GETWND );
		msg( IPC_GET_EMBEDIF );
		msg( IPC_USE_UXTHEME_FUNC );
		msg( IPC_GET_DISPATCH_OBJECT );
		msg( IPC_REGISTER_LOWORD_COMMAND );
		msg( IPC_GET_RANDFUNC );
		msg( IPC_GETSKIN );
		msg( IPC_STATS_LIBRARY_ITEMCNT );
		msg( IPC_IS_EXIT_ENABLED );
		msg( IPC_HOOK_OKTOQUIT );
		msgi( IPC_PLAYLIST_MODIFIED );
		msgi( IPC_GETLISTLENGTH );
		msg( IPC_INITIAL_SHOW_STATE );
		msg( IPC_CB_OUTPUTCHANGED );


		msgi( IPC_CB_MISC );
		msg( IPC_WRITECONFIG );
		msgi( IPC_HOOK_TITLES );
		msgi( IPC_HOOK_TITLESW );
		msgi( IPC_GET_EXTENDED_FILE_INFOW_HOOKABLE );
		msgi( IPC_ISPLAYING );
		msg( IPC_PLAYING_FILE );
		msgi( IPC_PLAYING_FILEW );
#undef msg
		case 406: care = false; return "IPC_406";
	}
	care = true;
	return 0;
}

static LRESULT WINAPI SubclassProc( HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam )
{
	if( msg == WM_WA_IPC )
	{
		bool care;
		char* lpn = lParamName( lParam, care );
		if( care )
		{
			logger << "lParam: ";
			if( lpn )
				logger << lpn;
			else
				logger << lParam;
			logger << endl;
		}
		switch( lParam )
		{
			case IPC_PLAYING_FILEW:
				break;
		}
		int a = 0;
	}
	return CallWindowProc( oldWndProc, hwnd, msg, wParam, lParam );
}