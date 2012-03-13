#pragma once

#include <map>

using namespace std;

class FontChar
{
public:
	FontChar( wchar_t a_Char, wchar_t* a_Data, size_t a_MaxHeight );
	~FontChar();
	wchar_t		m_Char;
	wchar_t*	m_Data;
	size_t	m_Width;
};

typedef map<wchar_t,FontChar*>	FontMap;
typedef map<wchar_t,wchar_t>	FontRemap;

/*abstract*/ class Font
{
protected:
	int				m_MaxWidth;
	int				m_MaxHeight;
	FontMap*		m_Font;
	FontRemap*		m_FontRemap;

	void			Init(int);
	virtual void	InitCharset() = 0;
	void			SetChar( wchar_t p, wchar_t* c );
	void			SetChar( wchar_t from, wchar_t to );

public:
	Font();
	virtual		~Font();
	wchar_t*	GetChar( wchar_t c, int& w, int& h );
	int			MeasureWidth( wchar_t* c );
};

class Font7x5 : public Font
{
	void InitCharset();
public:
	Font7x5() : Font() { Init(7); };
};

class Font7x5Time : public Font
{
	void InitCharset();
public:
	Font7x5Time() : Font() { Init(7); };
};