#include "pch.h"
#include "PacketReverser.h"

#include "HexArithmetic.h"
#include "Packet.h"

void Pipe::CreatePipe()
{
	Pipe::hPipe = CreateFileA(R"(\\.\pipe\Gatekeeper)", GENERIC_WRITE, FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
	if (!Pipe::hPipe)
	{
		// loop and wait..? orrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr
		Pipe::hPipe = CreateFileA(R"(\\.\pipe\Gatekeeper)", GENERIC_WRITE, FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0,
		                          0);
		Sleep(1000);
	}

	printf_s("hPipe == %p\n", Pipe::hPipe);
}

void Pipe::SendPipe(Pckt* send_st, int zeroSendOneRecv, int port)
{
	auto time = GetTickCount();
	auto size = send_st->len;

	char* writeBuffer = new char[size + 0x200];
	memset(writeBuffer, 0, size + 0x200);

	if (port != 80 && port != 443)
	{
		sprintf_s(writeBuffer, size + 0x200, "%d|%u|%d|%d|", port, zeroSendOneRecv, time, size);
		memcpy(writeBuffer + strlen(writeBuffer), send_st->data, size);

		WriteFile(Pipe::hPipe, writeBuffer, size + 0x200, 0, 0);
	}
	delete writeBuffer;
}

PacketReverser::PacketReverser()
{
	knownPackets = std::list<Packet*>();

	knownPackets.push_front(new Packet("ClientHello", (char*)"\x16\x03\x01\x00\xD2\x01\x00\x00\xCE\x03\x01", 11));
}

void PacketReverser::Print(char* buffer, int size, Direction dir, Type type)
{
	bool silent = true;
	if (!silent)
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
	}

	Reverse(buffer, size, dir, type);

	if (!silent)
	{
		printf("\n");
	}
}


void PacketReverser::Reverse(char* buffer, int size, Direction dir, Type type)
{
	for (auto knownPacket : knownPackets)
	{
		if (Hex::StartsSame(knownPacket->id, buffer, knownPacket->idSize))
		{
			knownPacket->Print(buffer, size);

			Pipe::Pckt p(buffer, size);
			int port;
			if (type == GAME)
			{
				port = 3002;
			}
			else
			{
				port = 3333;

			}
			SendPipe(&p, 0, port);

			return;
		}
	}

	/*for (int i = 0; i < size; ++i)
	{
		printf("%02X ", (BYTE)buffer[i]);
	}*/
}
