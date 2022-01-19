using System.Collections.Generic;

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
