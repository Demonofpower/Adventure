using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Adv.Server.Packets.Game
{
    class ClientActivatePacket : GamePacket
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }

        public ClientActivatePacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
