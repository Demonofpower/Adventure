#pragma once
#include <string>

enum PacketType
{
	ClientPosition,
	ClientJump,
	ClientFireball,
	ClientSetHand,
	ServerOk,
	ServerSetHandAck1,
	ServerSetHandAck2
};

struct SetHandPacket
{
	BYTE slot;
};

class Packet
{
public:
	PacketType type;
	WORD* id;

	Packet(PacketType type, WORD* id)
	{
		this->type = type;
		this->id = id;
	}
};

class RealPacket : Packet
{
public:
	char* buffer;

	RealPacket(PacketType type, WORD* id, char* buffer): Packet(type, id)
	{
		this->buffer = buffer;
	}
};

class SetHandPacketHandler : RealPacket
{
public:
	struct SetHandPacket* packet;
	
	SetHandPacketHandler(char* buffer)	: RealPacket(ClientSetHand, (WORD*)"\x73\x3D", buffer)
	{
		packet = (SetHandPacket*) buffer;
	}
};