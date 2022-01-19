using System.Collections.Generic;

namespace Adv.Server.Packets.Game
{
    class GamePacket : Packet
    {
        public GamePacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
