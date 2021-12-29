using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using Adv.Server.Packets;
using Adv.Server.Packets.Game;

namespace Adv.Server.Game
{
    static class GameConnectionApi
    {
        public static ClientHelloPacket ProcessClientHelloPacket(byte[] packet, TcpClient client)
        {
            var clientHelloPacket = new ClientHelloPacket(packet.ToList());
            
            clientHelloPacket.CharacterId = PacketProcessor.Read32(ref packet);
            clientHelloPacket.SessionId = PacketProcessor.ReadString(ref packet);

            return clientHelloPacket;
        }

        public static byte[] CreateServerHelloPacket(Vector3 position, Vector3 rotation)
        {
            
            return new byte[] { };
        }
    }
}
