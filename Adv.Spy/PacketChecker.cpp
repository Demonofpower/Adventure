#include "pch.h"
#include "PacketChecker.h"

#include "Packet.h"
#include <iostream>

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

		return knownPackets;
	}

	void Check(char* buffer, int size, Direction dir, Type type)
	{
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
			else
			{
				
			}
		}

		std::cout << "UNKNOWN ";

		for (int i = 0; i < 2; ++i)
		{
			printf("%02X ", (BYTE)*buffer);
			buffer += 1;
		}

		printf("\n");
	}
}
