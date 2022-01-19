using System.Collections.Generic;

namespace Adv.Server.Packets.Master
{
    class ClientJoinGameServerPacket : MasterPacket
    {
        public int CharacterId { get; set; }
        
        public ClientJoinGameServerPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
