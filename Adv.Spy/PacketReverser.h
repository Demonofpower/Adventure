#pragma once
#include <list>

class Packet;

class PacketReverser
{
public:
	std::list<Packet*> knownPackets;

	PacketReverser();

	void Reverse(char *buffer, int length);
};
