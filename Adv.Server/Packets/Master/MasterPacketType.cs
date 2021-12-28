namespace Adv.Server.Packets.Master
{
    enum MasterPacketType
    {
        Login = 0x0,
        Register = 0x01,
        GetPlayerCounts = 0x2,
        GetTeammates = 0x3,
        CharacterList = 0xa,
        CreateCharacter = 0xb,
        DeleteCharacter = 0xc,
        JoinGameServer = 0xd,
        ValidateCharacterToken = 0x14,
        AddServerToPool = 0x15,
        CharacterRegionChange = 0x16,
        StartQuest = 0x1e,
        UpdateQuest = 0x1f,
        CompleteQuest = 0x20,
        SetActiveQuest = 0x21,
        UpdateItems = 0x5c,
        MarkAsPickedUp = 0x23,
        GetFlag = 0x28,
        SubmitFlag = 0x29,
        SubmitAnswer = 0x2a,
        NoAction = 0x80,
        End = 0x81,
    }
}