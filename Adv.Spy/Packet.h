#pragma once
#include <string>

enum PacketType
{
	ClientPosition,
	ClientJump,
	ClientShoot,
	ClientSetHand,
	ClientChat,
	ServerOk,
	ServerSetHandAck1,
	ServerSetHandAck2,
	ServerManaUpdate,

	OnNPCConversationStateEvent,
	OnPlayerPositionEvent,
	OnRemoveItemEvent,
	OnActorSpawnEvent,
	OnReloadEvent,
	OnRespawnOtherPlayerEvent,
	OnAddItemEvent,
	OnRemoteReloadEvent,
	OnTeleportEvent,
	OnStartQuestEvent,
	OnTriggerEvent,
	OnPickedUpEvent,
	OnRelativeTeleportEvent,
	OnPositionAndVelocityEvent,
	OnRespawnThisPlayerEvent,
	OnStateEvent,
	OnPvpEnableEvent,
	OnDisplayEvent,
	OnPositionEvent,
	OnActorDestroyEvent,
	OnPlayerItemEvent,
	OnCurrentSlotEvent,
	OnEquipItemEvent,
	OnCircuitOutputEvent,
	OnKillEvent,
	OnSetCurrentQuestEvent,
	OnHealthUpdateEvent,
	OnChatEvent,
	OnFireBulletsEvent,
	OnNPCShopEvent,
	OnPvPCountdownUpdateEvent,
	OnPlayerLeftEvent,
	OnPlayerJoinedEvent,
	OnManaUpdateEvent,
	OnAdvanceQuestToStateEvent,
	OnLoadedAmmoEvent,
	OnLastHitByItemEvent,
	OnCountdownUpdateEvent,
	OnNPCConversationEndEvent,
	OnRegionChangeEvent,
	Use,
	TransitionToNPCState,
	BuyItem,
	Activate,
	FireRequest,
	SellItem,
	Teleport,
	Sprint,
	Jump,
	FastTravel,
	SubmitDLCKey
};

struct SetHandPacket
{
	BYTE slot;
};
struct JumpPacket
{
	bool inAir;
};
struct ChatPacket
{
	BYTE length;
	BYTE _null;
	char msg;
};
struct ShootPacket
{
	BYTE length;
	BYTE _null;
	char* weaponName;
	float yaw;
	float pitch;
	float roll;
};

struct ManaUpdatePacket
{
	short _unknwn;
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

class JumpPacketHandler : RealPacket
{
public:
	struct JumpPacket* packet;

	JumpPacketHandler(char* buffer) : RealPacket(ClientJump, (WORD*)"\x6A\x70", buffer)
	{
		packet = (JumpPacket*)buffer;
	}
};

class ChatPacketHandler : RealPacket
{
public:
	struct ChatPacket* packet;

	ChatPacketHandler(char* buffer) : RealPacket(ClientChat, (WORD*)"\x23\x2A", buffer)
	{
		packet = (ChatPacket*)buffer;
	}
};

class ShootPacketHandler : RealPacket
{
public:
	struct ShootPacket* packet;

	ShootPacketHandler(char* buffer) : RealPacket(ClientShoot, (WORD*)"\x2A\x69", buffer)
	{
		packet = (ShootPacket*)buffer;
	}	
};

class ManaUpdatePacketHandler : RealPacket
{
public:
	struct ManaUpdatePacket* packet;

	ManaUpdatePacketHandler(char* buffer) : RealPacket(ServerManaUpdate, (WORD*)"\x7D\x0A", buffer)
	{
		packet = (ManaUpdatePacket*)buffer;
	}
	
};
