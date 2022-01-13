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
        public static ClientHelloPacket ProcessClientHelloPacket(byte[] packet)
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

        public static ClientPositionPacket ProcessClientPositionPacket(byte[] packet)
        {
            var clientPlayerPositionPacket = new ClientPositionPacket(packet.ToList());

            clientPlayerPositionPacket.Position = PacketProcessor.ReadVector3(ref packet);
            clientPlayerPositionPacket.Rotation = PacketProcessor.ReadRotation(ref packet);
            clientPlayerPositionPacket.Forward = PacketProcessor.Read8(ref packet);
            clientPlayerPositionPacket.Strafe = PacketProcessor.Read8(ref packet);

            return clientPlayerPositionPacket;
        }
    }
}
