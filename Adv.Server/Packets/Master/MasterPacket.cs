using System.Collections.Generic;

namespace Adv.Server.Packets.Master
{
    class MasterPacket : Packet
    {
        public byte SmallId =>  (byte) Id;

        public MasterPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
