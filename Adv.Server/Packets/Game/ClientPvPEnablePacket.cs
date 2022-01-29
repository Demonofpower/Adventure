using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Packets.Game
{
    class ClientPvPEnablePacket : GamePacket
    {
        public byte State { get; set; }

        public ClientPvPEnablePacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
