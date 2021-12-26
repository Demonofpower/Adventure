using System.Collections.Generic;

namespace Adv.Server.Packets
{
    class Packet
    {
        public short Id { get; set; }
        
        public List<byte> RawData { get; set; }

        public Packet(List<byte> rawData)
        {
            RawData = rawData;
        }
    }
}
