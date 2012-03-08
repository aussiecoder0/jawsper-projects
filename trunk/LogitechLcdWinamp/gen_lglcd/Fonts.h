#pragma once

#include <map>

using namespace std;

class FontChar
{
public:
	FontChar(wchar_t ac, wchar_t* ad, int max_height);
	~FontChar();
	wchar_t c;
	wchar_t* d;
	int w;
};

typedef map<wchar_t,FontChar*> FontMap;

/*abstract*/ class Font
{
protected:
	int			m_MaxWidth;
	int			m_MaxHeight;
	FontMap*	m_Font;
	//int			m_CharWidth[256];
	int			s_Transl[256];

	void Init(int);
	virtual void InitCharset() = 0;
	void SetChar( wchar_t p, wchar_t* c );

public:
	Font();
	virtual ~Font();
	wchar_t* GetChar( wchar_t c, int* w, int* h );
	int MeasureWidth( wchar_t* c );
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