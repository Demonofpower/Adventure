using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using Adv.Server.Packets;
using Adv.Server.Packets.Game;
using Adv.Server.Util;

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

        public static byte[] CreateServerHelloPacket(Vector3 position, Rotation rotation)
        {
            var packet = new List<byte>();

            //TODO!!! id
            packet.Write32(1);
            
            packet.WriteVector3(position);
            packet.WriteRotation(rotation);
            
            return packet.ToArray();
        }
    }
}
