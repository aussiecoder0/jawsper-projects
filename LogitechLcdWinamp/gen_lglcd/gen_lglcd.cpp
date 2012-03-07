// gen_lglcd.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "gen_lglcd.h"

// these are callback functions/events which will be called by Winamp
int  init(void);
void config(void);
void quit(void);

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

// event functions follow

int init()
{
	//A basic messagebox that tells you the 'init' event has been triggered.
	//If everything works you should see this message when you start Winamp once your plugin has been installed.
	//You can change this later to do whatever you want (including nothing)
	return LogitechLcdWinamp::getInstance()->init(plugin.hwndParent);
}

void config()
{
	//A basic messagebox that tells you the 'config' event has been triggered.
	//You can change this later to do whatever you want (including nothing)
	LogitechLcdWinamp::getInstance()->config();
	//MessageBox(plugin.hwndParent, L"Config event triggered for gen_myplugin.", L"", MB_OK);
}

void quit()
{
	//A basic messagebox that tells you the 'quit' event has been triggered.
	//If everything works you should see this message when you quit Winamp once your plugin has been installed.
	//You can change this later to do whatever you want (including nothing)
	LogitechLcdWinamp::deleteInstance();
}

// This is an export function called by winamp which returns this plugin info.
// We wrap the code in 'extern "C"' to ensure the export isn't mangled if used in a CPP file.
extern "C" __declspec(dllexport) winampGeneralPurposePlugin * winampGetGeneralPurposePlugin()
{
	return &plugin;
}