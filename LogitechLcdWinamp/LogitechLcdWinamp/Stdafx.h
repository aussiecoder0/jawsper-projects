// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#include <windows.h>
#include <tchar.h>
#include "LogitechLcdWinamp.h"

// plugin version (don't touch this)
#define GPPHDR_VER 0x10

// plugin name/title (change this to something you like)
#define PLUGIN_NAME "Winamp lgLCD display"
 
// main structure with plugin information, version, name...
typedef struct
{
	int version;                   // version of the plugin structure
	char *description;             // name/title of the plugin 
	int (*init)();                 // function which will be executed on init event
	void (*config)();              // function which will be executed on config event
	void (*quit)();                // function which will be executed on quit event
	HWND hwndParent;               // hwnd of the Winamp client main window (stored by Winamp when dll is loaded)
	HINSTANCE hDllInstance;        // hinstance of this plugin DLL. (stored by Winamp when dll is loaded) 
} winampGeneralPurposePlugin;