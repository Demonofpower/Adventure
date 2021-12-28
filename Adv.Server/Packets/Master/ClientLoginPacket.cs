using System.Collections.Generic;

namespace Adv.Server.Packets.Master
{
    class ClientLoginPacket : MasterPacket
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public ClientLoginPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
