#include "pch.h"
#include "Packet.h"

#include <iostream>

void Packet::Print(char* buffer, int size, bool silent)
{
	if (silent)
	{
		std::cout << name << std::endl;
		return;
	}

	std::cout << name;

	/*for (int i = 0; i < packet.idSize(); ++i)
	{
		printf("%02X ", (BYTE)packet.c_str()[i]);
	}*/
	printf("\n");
}
