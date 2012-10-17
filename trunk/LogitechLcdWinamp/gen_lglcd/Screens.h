#pragma once

#include "LcdScreen.h"

class MainScreen : public LcdScreen
{
protected:
	Font* m_Font;
	Font* m_TimeFont;

	bool m_HasMail;
	vector<MailCount> m_MailCount;

	void DrawInit();
	void ButtonsUpdate();
public:
	MainScreen();
	~MainScreen();
	void UpdateMailCount();
	void SetProgress( int val, int min, int max );
	void SetPlayingState( int state );
};