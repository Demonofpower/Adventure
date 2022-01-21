using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Packets.Master
{
    class ClientCreateCharacterPacket : MasterPacket
    {
        public string Name { get; set; }
        public byte Avatar { get; set; }
        public int ColorA { get; set; }
        public int ColorB { get; set; }
        public int ColorC { get; set; }
        public int ColorD { get; set; }

        public ClientCreateCharacterPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}