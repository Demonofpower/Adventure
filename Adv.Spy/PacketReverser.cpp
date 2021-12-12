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
	knownPackets.push_front(new Packet(ClientShoot, (WORD*)"\x2A\x69"));
	knownPackets.push_front(new Packet(ClientSetHand, (WORD*)"\x73\x3D"));
	knownPackets.push_front(new Packet(ClientChat, (WORD*)"\x23\x2A"));

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
						goto unknwn;
					}
				case ClientPosition:
					{
						//std::cout << "ClientPosition" << std::endl;
						buffer += 1000;
						goto unknwn;
					}
				case ClientJump:
					{
						const JumpPacketHandler handler(buffer);
						buffer += sizeof(JumpPacket);
						std::cout << "ClientJump: " << ((int)handler.packet->inAir) << std::endl;

						/*for (int i = 0; i < size-2; i++)
						{
							printf("%02X ", (BYTE)*buffer);
							buffer += 1;
						}
						std::cout << std::endl;
						buffer += 1000;*/
						goto unknwn;
					}
				case ClientShoot:
					{
						const ShootPacketHandler handler(buffer);
						int nameLength = (int)handler.packet->length;
						buffer += 2;
						std::cout << "ClientShoot" << std::endl;

						for (int i = 0; i < nameLength; ++i)
						{
							printf("%c", *buffer);
							buffer += 1;
						}
						
						float yaw = (float)*buffer;
						buffer += 4;
						float pitch = (float)*buffer;
						buffer += 4;
						float roll = (float)*buffer;
						buffer += 4;

						printf(" %f %f %f ", yaw, pitch, roll);
						std::cout << std::endl;
						
						goto unknwn;
					}
				case ClientChat:
					{
						const ChatPacketHandler handler(buffer);
						int chatLength = (int)handler.packet->length;
						buffer += 2;
						std::cout << "ClientChat: ";
						for (int i = 0; i < chatLength; ++i)
						{
							printf("%c", *buffer);
							buffer += 1;
						}
						printf("\n");

						goto unknwn;
					}
				case ServerOk:
					{
						//std::cout << "ServerOk" << std::endl;
						buffer += 1000;
						goto unknwn;
					}
				case ServerSetHandAck1:
					{
						std::cout << "ServerSetHandAck1" << std::endl;
						buffer += 1000;
						goto unknwn;
					}
				case ServerSetHandAck2:
					{
						std::cout << "ServerSetHandAck2" << std::endl;
						buffer += 1000;
						goto unknwn;
					}
				default:
					std::cout << "Unimplemented packet" << std::endl;
					buffer += 1000;
					goto unknwn;
				}
			}
		}

	unknwn:

		int todo = size - (buffer - realBuffer);

		if (todo > 0)
		{
			for (auto knownPacket : knownPackets)
			{
				if (*knownPacket->id == *((WORD*)buffer))
				{
					goto start;
				}
			}

			if (!silent)
			{
				if (type == MASTER)
				{
					return;
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

				for (int i = 0; i < todo; ++i)
				{
					for (auto knownPacket : knownPackets)
					{
						if (*knownPacket->id == *((WORD*)buffer))
						{
							printf("\n");
							goto start;
						}
					}

					printf("%02X ", (BYTE)*buffer);
					buffer += 1;
				}
				printf("\n");
			}
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
