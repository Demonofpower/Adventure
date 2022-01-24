using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Packets.Game
{
    class ClientChatPacket : GamePacket
    {
        public string Message { get; set; }

        public ClientChatPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
