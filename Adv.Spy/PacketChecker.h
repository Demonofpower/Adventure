#pragma once
#include "Packet.h"
#include "PacketReverser.h"

namespace PacketChecker
{
	struct QueueItem
	{
		char buffer[128];
		int len;
	};
		
	std::list<Packet*> GetPackedIds();
		
	void Check(char* buffer, int size, Direction dir, Type type);


	PacketType GetNewPacketType(char* buffer);
	void Reset();
	void ProcessMasterPacket(char* buffer, int size, Direction dir);
	void ProcessStartPacket(char* packet);
	void ProcessLoginPacket(char* packet);
}
