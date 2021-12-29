using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Packets.Game
{
    class ClientHelloPacket : GamePacket
    {
        public int CharacterId { get; set; }
        public string SessionId { get; set; }
        
        public ClientHelloPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
