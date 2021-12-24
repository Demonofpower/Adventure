#include "pch.h"
#include "PacketChecker.h"

#include "Packet.h"
#include <iostream>

#include "HexArithmetic.h"

namespace PacketChecker
{
	std::list<Packet*> GetPackedIds()
	{
		auto knownPackets = std::list<Packet*>();

		knownPackets.push_front(new Packet(OnNPCConversationStateEvent, (WORD*)"\x23\x73"));
		knownPackets.push_front(new Packet(OnPlayerPositionEvent, (WORD*)"\70\x70"));
		knownPackets.push_front(new Packet(OnRemoveItemEvent, (WORD*)"\x73\x6d"));
		knownPackets.push_front(new Packet(OnActorSpawnEvent, (WORD*)"\x6d\x6b"));
		knownPackets.push_front(new Packet(OnReloadEvent, (WORD*)"\x72\x6c"));
		knownPackets.push_front(new Packet(OnRespawnOtherPlayerEvent, (WORD*)"\x72\x6f"));
		knownPackets.push_front(new Packet(OnAddItemEvent, (WORD*)"\x83\x70"));
		knownPackets.push_front(new Packet(OnRemoteReloadEvent, (WORD*)"\x73\x72"));
		knownPackets.push_front(new Packet(OnTeleportEvent, (WORD*)"\xFF\xFF"));
		knownPackets.push_front(new Packet(OnStartQuestEvent, (WORD*)"\x6e\x71"));
		knownPackets.push_front(new Packet(OnTriggerEvent, (WORD*)"\x74\x72"));
		knownPackets.push_front(new Packet(OnPickedUpEvent, (WORD*)"\x70\x75"));
		knownPackets.push_front(new Packet(OnRelativeTeleportEvent, (WORD*)"\x72\x74"));
		knownPackets.push_front(new Packet(OnPositionAndVelocityEvent, (WORD*)"\x70\x73"));
		knownPackets.push_front(new Packet(OnRespawnThisPlayerEvent, (WORD*)"\x72\x73"));
		knownPackets.push_front(new Packet(OnStateEvent, (WORD*)"\x73\x74"));
		knownPackets.push_front(new Packet(OnPvpEnableEvent, (WORD*)"\x70\x76"));
		knownPackets.push_front(new Packet(OnDisplayEvent, (WORD*)"\x65\x76"));
		knownPackets.push_front(new Packet(OnPositionEvent, (WORD*)"\x6d\x76"));
		knownPackets.push_front(new Packet(OnActorDestroyEvent, (WORD*)"\x78\x78"));
		knownPackets.push_front(new Packet(OnPlayerItemEvent, (WORD*)"\x70\x69"));
		knownPackets.push_front(new Packet(OnCurrentSlotEvent, (WORD*)"\x73\x3d"));
		knownPackets.push_front(new Packet(OnEquipItemEvent, (WORD*)"\x69\x3d"));
		knownPackets.push_front(new Packet(OnCircuitOutputEvent, (WORD*)"\x30\x31"));
		knownPackets.push_front(new Packet(OnKillEvent, (WORD*)"\x2d\x39"));
		knownPackets.push_front(new Packet(OnSetCurrentQuestEvent, (WORD*)"\x71\x3d"));
		knownPackets.push_front(new Packet(OnHealthUpdateEvent, (WORD*)"\x2b\x2b"));
		knownPackets.push_front(new Packet(OnChatEvent, (WORD*)"\x23\x2a"));
		knownPackets.push_front(new Packet(OnFireBulletsEvent, (WORD*)"\x2a\x2a"));
		knownPackets.push_front(new Packet(OnNPCShopEvent, (WORD*)"\x24\x24"));
		knownPackets.push_front(new Packet(OnPvPCountdownUpdateEvent, (WORD*)"\xFF\xFF"));
		knownPackets.push_front(new Packet(OnPlayerLeftEvent, (WORD*)"\x5e\x63"));
		knownPackets.push_front(new Packet(OnPlayerJoinedEvent, (WORD*)"\x6e\x63"));
		knownPackets.push_front(new Packet(OnManaUpdateEvent, (WORD*)"\xFF\xFF"));
		knownPackets.push_front(new Packet(OnAdvanceQuestToStateEvent, (WORD*)"\x71\x3e"));
		knownPackets.push_front(new Packet(OnLoadedAmmoEvent, (WORD*)"\xFF\xFF"));
		knownPackets.push_front(new Packet(OnLastHitByItemEvent, (WORD*)"\x6c\x68"));
		knownPackets.push_front(new Packet(OnCountdownUpdateEvent, (WORD*)"\x63\x64"));
		knownPackets.push_front(new Packet(OnNPCConversationEndEvent, (WORD*)"\x23\x66"));
		knownPackets.push_front(new Packet(OnRegionChangeEvent, (WORD*)"\xFF\xFF"));
		knownPackets.push_front(new Packet(Use, (WORD*)"\x65\x65"));
		knownPackets.push_front(new Packet(TransitionToNPCState, (WORD*)"\x23\x3e"));
		knownPackets.push_front(new Packet(BuyItem, (WORD*)"\x24\x26"));
		knownPackets.push_front(new Packet(Activate, (WORD*)"\x2a\x69"));
		knownPackets.push_front(new Packet(FireRequest, (WORD*)"\x66\x72"));
		knownPackets.push_front(new Packet(SellItem, (WORD*)"\x73\x24"));
		knownPackets.push_front(new Packet(Teleport, (WORD*)"\xFF\xFF"));
		knownPackets.push_front(new Packet(Sprint, (WORD*)"\x72\x6e"));
		knownPackets.push_front(new Packet(Jump, (WORD*)"\x6a\x70"));
		knownPackets.push_front(new Packet(FastTravel, (WORD*)"\x66\x75"));
		knownPackets.push_front(new Packet(SubmitDLCKey, (WORD*)"\x6b\x79"));

		knownPackets.push_front(new Packet(Login, (WORD*)"\x00\x00"));
		knownPackets.push_front(new Packet(Register, (WORD*)"\x01\x00"));
		knownPackets.push_front(new Packet(GetPlayerCounts, (WORD*)"\x02\x00"));
		knownPackets.push_front(new Packet(GetTeammates, (WORD*)"\x03\x00"));
		knownPackets.push_front(new Packet(CharacterList, (WORD*)"\x0a\x00"));
		knownPackets.push_front(new Packet(CreateCharacter, (WORD*)"\x0b\x00"));
		knownPackets.push_front(new Packet(DeleteCharacter, (WORD*)"\x0c\x00"));
		knownPackets.push_front(new Packet(JoinGameServer, (WORD*)"\x0d\x00"));
		knownPackets.push_front(new Packet(ValidateCharacterToken, (WORD*)"\x14\x00"));
		knownPackets.push_front(new Packet(AddServerToPool, (WORD*)"\x15\x00"));
		knownPackets.push_front(new Packet(CharacterRegionChange, (WORD*)"\x16\x00"));
		knownPackets.push_front(new Packet(StartQuest, (WORD*)"\x1e\x00"));
		knownPackets.push_front(new Packet(UpdateQuest, (WORD*)"\x1f\x00"));
		knownPackets.push_front(new Packet(CompleteQuest, (WORD*)"\x20\x00"));
		knownPackets.push_front(new Packet(SetActiveQuest, (WORD*)"\x21\x00"));
		knownPackets.push_front(new Packet(UpdateItems, (WORD*)"\x5c\x00"));
		knownPackets.push_front(new Packet(MarkAsPickedUp, (WORD*)"\x23\x00"));
		knownPackets.push_front(new Packet(GetFlag, (WORD*)"\x28\x00"));
		knownPackets.push_front(new Packet(SubmitFlag, (WORD*)"\x29\x00"));
		knownPackets.push_front(new Packet(SubmitAnswer, (WORD*)"\x2a\x00"));
		knownPackets.push_front(new Packet(NoAction, (WORD*)"\x80\x00"));
		knownPackets.push_front(new Packet(END, (WORD*)"\x81\x00"));


		return knownPackets;
	}

	void Check(char* buffer, int size, Direction dir, Type type)
	{
		if (type == MASTER)
		{
			return;
		}

		if (*((WORD*)buffer) == *(WORD*)"\x6d\x76")
		{
			return;
		}
		if (*((WORD*)buffer) == *(WORD*)"\x52\x48")
		{
			return;
		}

		auto knownPackets = GetPackedIds();

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

		for (auto knownPacket : knownPackets)
		{
			if (type == GAME)
			{
				if (*knownPacket->id == *((WORD*)buffer))
				{
					std::cout << knownPacket->GetPackeName() << "  ";

					for (int i = 0; i < 2; ++i)
					{
						printf("%02X ", (BYTE)*buffer);
						buffer += 1;
					}
					printf("\n");
					return;
				}
			}
		}

		std::cout << "UNKNOWN ";

		for (int i = 0; i < size; ++i)
		{
			printf("%02X ", (BYTE)*buffer);
			buffer += 1;
		}

		printf("\n");
	}

	PacketType GetNewPacketType(char* buffer)
	{
		auto knownPackets = GetPackedIds();
		for (auto knownPacket : knownPackets)
		{
			if (*(char*)knownPacket->id == *buffer)
			{
				return knownPacket->type;
			}
		}

		throw;
	}

	bool startFinished = false;
	bool hasFullPacket = false;
	char currPacket[256];
	int currPacketSize = 0;
	int currPacketParts = 0;
	PacketType currPacketType;

	void Reset()
	{
		hasFullPacket = false;
		currPacketSize = 0;
		currPacketParts = 0;
	}

	void ProcessMasterPacket(char* buffer, int size, Direction dir)
	{
		memcpy(&currPacket[currPacketSize], buffer, size);
		currPacketSize += size;
		currPacketParts += 1;

		printf("%i   ", currPacketParts);
		for (int i = 0; i < size; ++i)
		{
			printf("%02X ", (BYTE)*buffer);
			buffer += 1;
		}
		buffer -= size;
		printf("\n");
		
		if (!startFinished)
		{
			if (currPacketParts < 6)
			{
				return;
			}

			ProcessStartPacket(&currPacket[0]);
			startFinished = true;
			Reset();
			return;
		}

		if (currPacketParts == 1)
		{
			currPacketType = GetNewPacketType(buffer);
		}

		switch (currPacketType)
		{
		case Login:
			{
				if (currPacketParts == 8)
				{
					ProcessLoginPacket(&currPacket[0]);
					Reset();
				}
				break;
			}
		case Register: break;
		case GetPlayerCounts: break;
		case GetTeammates: break;
		case CharacterList: break;
		case CreateCharacter: break;
		case DeleteCharacter: break;
		case JoinGameServer: break;
		case ValidateCharacterToken: break;
		case AddServerToPool: break;
		case CharacterRegionChange: break;
		case StartQuest: break;
		case UpdateQuest: break;
		case CompleteQuest: break;
		case SetActiveQuest: break;
		case UpdateItems: break;
		case MarkAsPickedUp: break;
		case GetFlag: break;
		case SubmitFlag: break;
		case SubmitAnswer: break;
		case NoAction:
			{
			printf("80!!!!\n");
				Reset();
				break;
			}
		case END: break;
		default: throw;
		}
	}

	void ProcessStartPacket(char* buffer)
	{
		printf("Welcome pwn3: ");
		auto welcome = Hex::Read32(&buffer);
		std::cout << welcome << std::endl;

		printf("Version: ");
		auto version = Hex::Read16(&buffer);
		std::cout << version << std::endl;

		printf("LoginTitle: ");
		auto title = Hex::ReadString(&buffer);
		std::cout << title << std::endl;

		printf("LoginText: ");
		auto text = Hex::ReadString(&buffer);
		std::cout << text << std::endl;
	}

	void ProcessLoginPacket(char* buffer)
	{
		printf("Login\n");
		buffer += 1;
		
		printf("Username: ");
		auto user = Hex::ReadString(&buffer);
		std::cout << user << std::endl;

		printf("Password: ");
		auto pw = Hex::ReadString(&buffer);
		std::cout << pw << std::endl;

		printf("Result: ");
		auto result = Hex::Read8(&buffer);
		std::cout << result << std::endl;
		
		printf("Id: ");
		auto id = Hex::Read32(&buffer);
		std::cout << id << std::endl;

		printf("TeamHash: ");
		auto teamHash = Hex::ReadString(&buffer);
		std::cout << teamHash << std::endl;

		printf("TeamName: ");
		auto teamName = Hex::ReadString(&buffer);
		std::cout << teamName << std::endl;

		printf("IsAdmin: ");
		auto isAdmin = Hex::Read8(&buffer);
		std::cout << isAdmin << std::endl;
	}
}
