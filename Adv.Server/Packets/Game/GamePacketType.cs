namespace Adv.Server.Packets.Game
{
    enum GamePacketType
    {
		OnNPCConversationStateEvent = 0x2373,
        OnPlayerPositionEvent = 0x7070,
        OnRemoveItemEvent = 0x736d,
        OnActorSpawnEvent = 0x6d6b,
        OnReloadEvent = 0x726c,
        OnRespawnOtherPlayerEvent = 0x726f,
        OnAddItemEvent = 0x8370,
        OnRemoteReloadEvent = 0x7372,
        OnTeleportEvent = 0xfff3, //TODO
        OnStartQuestEvent = 0x6e71,
        OnTriggerEvent = 0x7472,
        OnPickedUpEvent = 0x7075,
        OnRelativeTeleportEvent = 0x7274,
        OnPositionAndVelocityEvent = 0x7073,
        OnRespawnThisPlayerEvent = 0x7273,
        OnStateEvent = 0x7374,
        OnPvpEnableEvent = 0x7076,
        OnDisplayEvent = 0x6576,
        OnPositionEvent = 0x6d76,
        OnActorDestroyEvent = 0x7878,
        OnPlayerItemEvent = 0x7069,
        OnCurrentSlotEvent = 0x733d,
        OnEquipItemEvent = 0x693d,
        OnCircuitOutputEvent = 0x3031,
        OnKillEvent = 0x2d39,
        OnSetCurrentQuestEvent = 0x713d,
        OnHealthUpdateEvent = 0x2b2b,
        OnChatEvent = 0x232a,
        OnFireBulletsEvent = 0x2a2a,
        OnNPCShopEvent = 0x2424,
        OnPvPCountdownUpdateEvent = 0xfff1, //TODO
        OnPlayerLeftEvent = 0x5e63,
        OnPlayerJoinedEvent = 0x6e63,
        OnManaUpdateEvent = 0xFFF4, //TODO
        OnAdvanceQuestToStateEvent = 0x713e,
        OnLoadedAmmoEvent = 0xFFF5, //TODO
        OnLastHitByItemEvent = 0x6c68,
        OnCountdownUpdateEvent = 0x6364,
        OnNPCConversationEndEvent = 0x2366,
        OnRegionChangeEvent = 0xFFF6, //TODO
        
        Use = 0x6565,
        TransitionToNPCState =  0x233e,
        BuyItem = 0x2426,
        Activate = 0x2a69,
        FireRequest = 0x6672,
        SellItem = 0x7324,
        Teleport = 0xFFF2, //TODO
        Sprint = 0x726e,
        Jump = 0x6a70,
        FastTravel = 0x6675,
        SubmitDLCKey = 0x6b79,
        
        ServerAck = 0x5248
	}
}
