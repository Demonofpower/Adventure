#include "pch.h"
#include "PacketReverser.h"

#include <cstdio>

void PacketReverser::Reverse(char* buffer, int length)
{
	for (int i = 0; i < length; ++i)
	{
		printf("%02X ", (BYTE)buffer[i]);
	}
}
