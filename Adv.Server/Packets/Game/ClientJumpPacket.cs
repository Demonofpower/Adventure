using System.Collections.Generic;

namespace Adv.Server.Packets.Game
{
    class ClientJumpPacket : GamePacket
    {
        public byte JumpState { get; set; }

        public ClientJumpPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}