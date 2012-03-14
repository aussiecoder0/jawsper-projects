#pragma once

#include <vector>

using namespace std;

struct MailCount
{
	wchar_t email[MAX_PATH];
	unsigned int count;
};

unsigned int UnreadMailCount();
void GetUnreadMailCount( vector<MailCount>& map );
