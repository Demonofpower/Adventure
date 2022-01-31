using System.Collections.Generic;

namespace Adv.Server.Packets.Game
{
    class ClientUsePacket : GamePacket
    {
        public int ItemId { get; set; }

        public ClientUsePacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
