#pragma once
#include <list>

enum Direction
{
	SEND,
	RECV
};

enum Type
{
	GAME,
	MASTER
};

class Packet;

class PacketReverser
{
public:
	std::list<Packet*> knownPackets;

	PacketReverser();

	void Print(char* buffer, int size, Direction dir, Type type);
	void Reverse(char *buffer, int size);
};