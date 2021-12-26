using System.Collections.Generic;

namespace Adv.Server.Packets.Master
{
    class WelcomePacket : MasterPacket
    {
        public WelcomePacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
