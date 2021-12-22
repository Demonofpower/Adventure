#include "pch.h"
#include "Packet.h"

#include <fstream>
#include <iostream>

struct HexCharStruct
{
	unsigned char c;

	HexCharStruct(unsigned char _c) : c(_c)
	{
	}
};

inline std::ostream& operator<<(std::ostream& o, const HexCharStruct& hs)
{
	return (o << std::hex << (int)hs.c);
}

inline HexCharStruct hex(unsigned char _c)
{
	return HexCharStruct(_c);
}

//void Packet::Print(char* buffer, int size, bool silent)
//{
//	if (silent)
//	{
//		std::cout << name << std::endl;
//		return;
//	}
//
//	/*FILE* fptr;
//	fopen_s(&fptr, "C:\\Users\\Juli\\Desktop\\src\\Adventure\\Debug\\Sessions\\a.txt", "a");
//	
//	for (int i = 0; i < size; ++i)
//	{
//		printf("%02X ", (BYTE)buffer[i]);
//
//		fprintf(fptr, "%02X", (BYTE)buffer[i]);
//	}
//
//	fprintf(fptr, "\n");
//
//	fclose(fptr);*/
//}
std::string Packet::GetPackeName()
{
	switch (type)
	{
	case ClientPosition: return "ClientPosition";
	case ClientJump: return "ClientJump";
	case ClientShoot: return "ClientShoot";
	case ClientSetHand: return "ClientSetHand";
	case ClientChat: return "ClientChat";
	case ServerOk: return "ServerOk";
	case ServerSetHandAck1: return "ServerSetHandAck1";
	case ServerSetHandAck2: return "ServerSetHandAck2";
	case ServerManaUpdate: return "ServerManaUpdate";
	case OnNPCConversationStateEvent: return "OnNPCConversationStateEvent";
	case OnPlayerPositionEvent: return "OnPlayerPositionEvent";
	case OnRemoveItemEvent: return "OnRemoveItemEvent";
	case OnActorSpawnEvent: return "OnActorSpawnEvent";
	case OnReloadEvent: return "OnReloadEvent";
	case OnRespawnOtherPlayerEvent: return "OnRespawnOtherPlayerEvent";
	case OnAddItemEvent: return "OnAddItemEvent";
	case OnRemoteReloadEvent: return "OnRemoteReloadEvent";
	case OnTeleportEvent: return "OnTeleportEvent";
	case OnStartQuestEvent: return "OnStartQuestEvent";
	case OnTriggerEvent: return "OnTriggerEvent";
	case OnPickedUpEvent: return "OnPickedUpEvent";
	case OnRelativeTeleportEvent: return "OnRelativeTeleportEvent";
	case OnPositionAndVelocityEvent: return "OnPositionAndVelocityEvent";
	case OnRespawnThisPlayerEvent: return "OnRespawnThisPlayerEvent";
	case OnStateEvent: return "OnStateEvent";
	case OnPvpEnableEvent: return "OnPvpEnableEvent";
	case OnDisplayEvent: return "OnDisplayEvent";
	case OnPositionEvent: return "OnPositionEvent";
	case OnActorDestroyEvent: return "OnActorDestroyEvent";
	case OnPlayerItemEvent: return "OnPlayerItemEvent";
	case OnCurrentSlotEvent: return "OnCurrentSlotEvent";
	case OnEquipItemEvent: return "OnEquipItemEvent";
	case OnCircuitOutputEvent: return "OnCircuitOutputEvent";
	case OnKillEvent: return "OnKillEvent";
	case OnSetCurrentQuestEvent: return "OnSetCurrentQuestEvent";
	case OnHealthUpdateEvent: return "OnHealthUpdateEvent";
	case OnChatEvent: return "OnChatEvent";
	case OnFireBulletsEvent: return "OnFireBulletsEvent";
	case OnNPCShopEvent: return "OnNPCShopEvent";
	case OnPvPCountdownUpdateEvent: return "OnPvPCountdownUpdateEvent";
	case OnPlayerLeftEvent: return "OnPlayerLeftEvent";
	case OnPlayerJoinedEvent: return "OnPlayerJoinedEvent";
	case OnManaUpdateEvent: return "OnManaUpdateEvent";
	case OnAdvanceQuestToStateEvent: return "OnAdvanceQuestToStateEvent";
	case OnLoadedAmmoEvent: return "OnLoadedAmmoEvent";
	case OnLastHitByItemEvent: return "OnLastHitByItemEvent";
	case OnCountdownUpdateEvent: return "OnCountdownUpdateEvent";
	case OnNPCConversationEndEvent: return "OnNPCConversationEndEvent";
	case OnRegionChangeEvent: return "OnRegionChangeEvent";
	case Use: return "Use";
	case TransitionToNPCState: return "TransitionToNPCState";
	case BuyItem: return "BuyItem";
	case Activate: return "Activate";
	case FireRequest: return "FireRequest";
	case SellItem: return "SellItem";
	case Teleport: return "Teleport";
	case Sprint: return "Sprint";
	case Jump: return "Jump";
	case FastTravel: return "FastTravel";
	case SubmitDLCKey: return "SubmitDLCKey";
	case Login: return "Login";
	case Register: return "Register";
	case GetPlayerCounts: return "GetPlayerCounts";
	case GetTeammates: return "GetTeammates";
	case CharacterList: return "CharacterList";
	case CreateCharacter: return "CreateCharacter";
	case DeleteCharacter: return "DeleteCharacter";
	case JoinGameServer: return "JoinGameServer";
	case ValidateCharacterToken: return "ValidateCharacterToken";
	case AddServerToPool: return "AddServerToPool";
	case CharacterRegionChange: return "CharacterRegionChange";
	case StartQuest: return "StartQuest";
	case UpdateQuest: return "UpdateQuest";
	case CompleteQuest: return "CompleteQuest";
	case SetActiveQuest: return "SetActiveQuest";
	case UpdateItems: return "UpdateItems";
	case MarkAsPickedUp: return "MarkAsPickedUp";
	case GetFlag: return "GetFlag";
	case SubmitFlag: return "SubmitFlag";
	case SubmitAnswer: return "SubmitAnswer";
	case END: return "END";
	default: return "UNKNOWN TYPE!";
	}
}
