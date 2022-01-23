using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Packets.Game
{
    class ClientSprintPacket : GamePacket
    {
        public byte SprintState { get; set; }

        public ClientSprintPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
