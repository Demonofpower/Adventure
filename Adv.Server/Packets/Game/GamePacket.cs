using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Packets.Game
{
    class GamePacket : Packet
    {
        public GamePacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
