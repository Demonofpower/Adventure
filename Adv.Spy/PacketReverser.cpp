#include "pch.h"
#include "PacketReverser.h"

#include "Packet.h"

PacketReverser::PacketReverser()
{
	knownPackets = std::list<Packet*>();

	knownPackets.push_front(new Packet("ClientHello", (char*) "\x16\x03\x01\x00\xD2\x01\x00\x00\xCE\x03\x01"));
}

void PacketReverser::Reverse(char* buffer, int length)
{	
	for (auto known_packet : knownPackets)
	{
		if (strcmp(known_packet->id, buffer) != 0)
		{
			known_packet->Print(buffer, false);
		}
	}
	
	for (int i = 0; i < length; ++i)
	{
		printf("%02X ", (BYTE)buffer[i]);
	}
}
