#include "pch.h"
#include "PacketReverser.h"

#include <iostream>


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

	knownPackets.push_front(new Packet(ClientPosition, (WORD*)"\x6D\x76"));
	knownPackets.push_front(new Packet(ClientJump, (WORD*)"\x6A\x70"));
	knownPackets.push_front(new Packet(ClientFireball, (WORD*)"\x2A\x69"));
	knownPackets.push_front(new Packet(ClientSetHand, (WORD*)"\x73\x3D"));

	knownPackets.push_front(new Packet(ServerOk, (WORD*)"\x52\x48"));
}

void PacketReverser::Print(char* buffer, int size, Direction dir, Type type)
{
	bool silent = false;

	Reverse(buffer, size, dir, type, silent);
}


void PacketReverser::Reverse(char* buffer, int size, Direction dir, Type type, bool silent)
{
	char* realBuffer = buffer;

re:
	while (buffer < realBuffer + size)
	{
	start:
		for (auto knownPacket : knownPackets)
		{
			if (*knownPacket->id == *((WORD*)buffer))
			{
				buffer += 2;

				switch (knownPacket->type)
				{
				case ClientSetHand:
					{
						const SetHandPacketHandler handler(buffer);
						buffer += sizeof(SetHandPacket);
						std::cout << "ClientSetHand: " << ((int)handler.packet->slot) << std::endl;
						break;
					}
				case ClientPosition:
					{
						std::cout << "ClientPosition" << std::endl;
						buffer += 1000;
						break;
					}
				case ClientJump:
					{
						std::cout << "ClientJump" << std::endl;
						buffer += 1000;
						break;
					}
				case ClientFireball:
					{
						std::cout << "ClientFireball" << std::endl;
						buffer += 1000;
						break;
					}
				case ServerOk:
					{
						std::cout << "ServerOk" << std::endl;
						buffer += 1000;
						break;
					}
				case ServerSetHandAck1:
					{
						std::cout << "ServerSetHandAck1" << std::endl;
						buffer += 1000;
						break;
					}
				case ServerSetHandAck2:
					{
						std::cout << "ServerSetHandAck2" << std::endl;
						buffer += 1000;
						break;
					}
				default:
					std::cout << "Unimplemented packet" << std::endl;
					buffer += 1000;
					break;
				}
			}
		}
		int todo = size - (buffer - realBuffer);

		if (todo > 0)
		{
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


			for (int i = 0; i < todo; ++i)
			{
				/*for (auto knownPacket : knownPackets)
				{
					if (*knownPacket->id == *((WORD*)buffer))
					{
						printf("\n");
						goto start;
					}
				}*/

				printf("%02X ", (BYTE)buffer[i]);
				buffer += 1;
			}
			printf("\n");
		}


		//for (auto knownPacket : knownPackets)
		//{
		//	if (*knownPacket->id == *((WORD*)buffer))
		//	{
		//		buffer += 2;

		//		switch (knownPacket->type)
		//		{
		//		case ClientSetHand:
		//			{
		//				const SetHandPacketHandler handler(buffer);
		//				buffer += sizeof(SetHandPacket);
		//				std::cout << "ClientSetHand: " << handler.packet->slot << std::endl;
		//				break;
		//			}
		//		default:
		//			break;
		//		}

		//		/*knownPacket->Print(buffer, size);

		//		Pipe::Pckt p(buffer, size);
		//		int port;
		//		if (type == GAME)
		//		{
		//			port = 3002;
		//		}
		//		else
		//		{
		//			port = 3333;
		//		}
		//		SendPipe(&p, 0, port);*/
		//	}
		//	else
		//	{
		//		buffer += 2;
		//		int todo = size - (buffer - realBuffer);
		//		if (!silent)
		//		{
		//			if (!silent)
		//			{
		//				if (type == MASTER)
		//				{
		//					printf("[Master]");
		//				}
		//				else
		//				{
		//					printf("[Game]");
		//				}

		//				if (dir == SEND)
		//				{
		//					printf(" <-- ");
		//				}
		//				else
		//				{
		//					printf(" --> ");
		//				}
		//			}

		//			for (int i = 0; i < todo; ++i)
		//			{
		//				for (auto knownPacket : knownPackets)
		//				{
		//					if (*knownPacket->id == *((WORD*)buffer))
		//					{
		//					}
		//				}

		//				printf("%02X ", (BYTE)buffer[i]);
		//			}
		//			printf("\n");
		//		}
		//	}
		//}
	}
}
