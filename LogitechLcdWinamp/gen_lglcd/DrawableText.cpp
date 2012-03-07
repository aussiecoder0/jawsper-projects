#include "Stdafx.h"
#include "ScreenManager.h"

void DrawableText::SetText( wchar_t* a_str )
{
	wcscpy_s( str, MAX_PATH, a_str );
	m_Changed = true;
}

void DrawableText::Draw( Surface* a_Surface )
{
	if( m_Changed )
	{
		m_Changed = false;
		w = min( mw, a_Surface->GetFont()->MeasureWidth( str ) );
		a_Surface->Bar( x, y, x + w - 1, y + 6, PIXEL_OFF );
		a_Surface->Print( str, x, y, PIXEL_ON );
	}
}