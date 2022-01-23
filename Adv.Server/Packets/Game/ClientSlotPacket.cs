using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Packets.Game
{
    class ClientSlotPacket : GamePacket 
    {
        public byte Slot { get; set; }
        
        public ClientSlotPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
