#pragma once

#include "inc/lglcd.h"
#include "Surface.h"

class ScreenManager
{
	int m_connection;
	int m_device;
	Surface* m_surface;
public:
	ScreenManager(void);
	~ScreenManager(void);

	void Update();
};

