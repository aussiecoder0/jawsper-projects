#pragma once

typedef struct
{
	char c;
	char* def;
} FontDef;

/*abstract*/ class Font
{
protected:
	int			m_Length;
	int			m_MaxWidth;
	int			m_MaxHeight;
	wchar_t*	m_Font[256];
	int			m_CharWidth[256];
	int			s_Transl[256];

	void Init(int,int);
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
	Font7x5() : Font() { Init(256, 7); };
};