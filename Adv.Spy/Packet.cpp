#include "pch.h"
#include "Packet.h"

#include <iostream>

void Packet::Print(std::string packet, bool silent = false)
{
	if (silent)
	{
		std::cout << name << std::endl;
		return;
	}

	std::cout << name;

	/*for (int i = 0; i < packet.length(); ++i)
	{
		printf("%02X ", (BYTE)packet.c_str()[i]);
	}*/
	printf("\n");
}
