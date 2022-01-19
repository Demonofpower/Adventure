using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Packets.Master
{
    class ClientRegisterPacket : MasterPacket
    {
        public string Username { get; set; }
        public string TeamNameOrHash { get; set; }
        public string Password { get; set; }

        public ClientRegisterPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
