#include "stdafx.h"

unsigned int UnreadMailCount()
{
	HKEY   hkey;
	DWORD  dwDisposition;
 
	DWORD dwType, dwSize;
 
	unsigned int count = 0;
 
	LSTATUS error;
	error = RegCreateKeyEx( 
		HKEY_CURRENT_USER, 
		TEXT("Software\\Microsoft\\Windows\\CurrentVersion\\UnreadMail"), 
		0, 
		NULL, 
		0,
		KEY_READ,
		NULL, 
		&hkey, &dwDisposition );
	if( error == ERROR_SUCCESS)
	{
		int i = 0;
		wchar_t name[MAX_PATH];
		DWORD name_len;
		while( ERROR_SUCCESS == ( error = RegEnumKeyEx( hkey, i++, name, &name_len, NULL, NULL, NULL, NULL ) ) )
		{
			HKEY hky;
			if( ERROR_SUCCESS == RegCreateKeyEx( hkey, name, 0, 0, 0, KEY_READ, 0, &hky, &dwDisposition ) )
			{
				dwType = REG_DWORD;
				dwSize = sizeof(DWORD);
				unsigned int c = 0;
				if( ERROR_SUCCESS == RegQueryValueEx( hky, TEXT("MessageCount"), NULL, &dwType, (PBYTE)&c, &dwSize ) )
					count += c;
				RegCloseKey( hky );
			}
		}
		RegCloseKey( hkey );
	};
	return count;
}

void GetUnreadMailCount( vector<MailCount>& vect )
{
	vect.clear();

	HKEY   hkey;
	DWORD  dwDisposition;
 
	if( ERROR_SUCCESS == RegCreateKeyEx( HKEY_CURRENT_USER, TEXT("Software\\Microsoft\\Windows\\CurrentVersion\\UnreadMail"), 0, NULL, 0, KEY_READ, NULL, &hkey, &dwDisposition ) )
	{
		int i = 0;
		wchar_t name[MAX_PATH];
		DWORD name_len;
		while( ERROR_SUCCESS == RegEnumKeyEx( hkey, i++, name, &name_len, NULL, NULL, NULL, NULL ) )
		{
			HKEY hky;
			if( ERROR_SUCCESS == RegCreateKeyEx( hkey, name, 0, 0, 0, KEY_READ, 0, &hky, &dwDisposition ) )
			{
				DWORD dwType, dwSize;
				dwType = REG_DWORD;
				dwSize = sizeof(DWORD);
				unsigned int c = 0;
				if( ERROR_SUCCESS == RegQueryValueEx( hky, TEXT("MessageCount"), NULL, &dwType, (PBYTE)&c, &dwSize ) )
				{
					MailCount mc = { 0 };
					wcscpy_s( mc.email, name );
					mc.count = c;
					vect.push_back( mc );
				}
				RegCloseKey( hky );
			}
		}
		RegCloseKey( hkey );
	};
}