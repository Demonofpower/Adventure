#include "pch.h"
#include "PacketReverser.h"


#include "HexArithmetic.h"
#include "Packet.h"

PacketReverser::PacketReverser()
{
	knownPackets = std::list<Packet*>();

	knownPackets.push_front(new Packet("ClientHello", (char*)"\x16\x03\x01\x00\xD2\x01\x00\x00\xCE\x03\x01", 11));
}

void PacketReverser::Print(char* buffer, int size, Direction dir, Type type)
{
	if (type == MASTER)
	{
		printf("[Master]");
	}
	else
	{
		printf("[Game]");
	}

	if (dir == SEND)
	{
		printf(" <-- ");
	}
	else
	{
		printf(" --> ");
	}

	Reverse(buffer, size);

	printf("\n");
}


void PacketReverser::Reverse(char* buffer, int size)
{
	for (auto knownPacket : knownPackets)
	{
		if (Hex::StartsSame(knownPacket->id, buffer, knownPacket->idSize))
		{
			knownPacket->Print(buffer, size);

			return;
		}
	}

	for (int i = 0; i < size; ++i)
	{
		printf("%02X ", (BYTE)buffer[i]);
	}
}
