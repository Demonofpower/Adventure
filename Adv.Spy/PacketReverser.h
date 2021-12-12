#pragma once
#include <list>

namespace Pipe
{
	struct Pckt
	{
		int len;
		char* data;

		Pckt(char* data, int len)
		{
			this->data = data;
			this->len = len;
		}
	};

	static HANDLE hPipe;

	void CreatePipe();

	void SendPipe(Pckt* send_st, int zeroSendOneRecv, int port);
}

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
	void Reverse(char* buffer, int size, Direction dir, Type type, bool silent = false);
};
