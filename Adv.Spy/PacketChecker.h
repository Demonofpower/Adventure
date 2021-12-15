#pragma once
#include "PacketReverser.h"

namespace PacketChecker
{
	std::list<Packet*> GetPackedIds();

	void Check(char* buffer, int size, Direction dir, Type type);
}
