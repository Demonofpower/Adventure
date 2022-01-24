using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Adv.Server.Packets;
using Adv.Server.Packets.Game;
using Adv.Server.Util;

namespace Adv.Server.Game
{
    static class GameConnectionApi
    {
        public static ClientHelloPacket ProcessClientHelloPacket(Span<byte> packet)
        {
            var clientHelloPacket = new ClientHelloPacket(packet.ToArray().ToList());

            clientHelloPacket.CharacterId = PacketProcessor.Read32(ref packet);
            clientHelloPacket.SessionId = PacketProcessor.ReadString(ref packet);

            return clientHelloPacket;
        }

        public static byte[] CreateServerHelloPacket(Vector3 position, Rotation rotation)
        {
            var packet = new List<byte>();

            //TODO!!! actorId 0x2bad7
            packet.Write32(1);

            packet.WriteVector3(position);
            packet.WriteRotation(rotation);

            return packet.ToArray();
        }

        public static ClientPositionPacket ProcessClientPositionPacket(ref Span<byte> packet)
        {
            var clientPlayerPositionPacket = new ClientPositionPacket(packet.ToArray().ToList());

            clientPlayerPositionPacket.Position = PacketProcessor.ReadVector3(ref packet);
            clientPlayerPositionPacket.Rotation = PacketProcessor.ReadRotation(ref packet);
            clientPlayerPositionPacket.Forward = PacketProcessor.Read8(ref packet);
            clientPlayerPositionPacket.Strafe = PacketProcessor.Read8(ref packet);

            return clientPlayerPositionPacket;
        }

        public static byte[] CreateClientPositionPacket(Vector3 position, Rotation rotation)
        {
            var packet = new List<byte>();

            //PACKETID
            packet.Write8(0x6d);
            packet.Write8(0x76);
            
            //PLAYERID??
            packet.Write32(0x1);
            
            packet.WriteVector3(position);
            packet.WriteRotation(rotation);

            packet.Write16(0x0);

            return packet.ToArray();
        }

        public static byte[] CreateActorSpawnPacket(Vector3 position, Rotation rotation)
        {
            var packet = new List<byte>();

            //PACKETID
            packet.Write8(0x6d);
            packet.Write8(0x6b);

            packet.Write32(0x1);
            packet.Write32(0x0);
            packet.Write8(0x0);
            packet.WriteString("GreatBallsOfFire");
            packet.WriteVector3(position);
            packet.WriteRotation(rotation);
            packet.Write32(0x0);

            return packet.ToArray();
        }

        public static ClientJumpPacket ProcessClientJumpPacket(ref Span<byte> packet)
        {
            var clientJumpPacket = new ClientJumpPacket(packet.ToArray().ToList());

            clientJumpPacket.JumpState = PacketProcessor.Read8(ref packet);

            return clientJumpPacket;
        }

        public static ClientSprintPacket ProcessClientSprintPacket(ref Span<byte> packet)
        {
            var clientSprintPacket = new ClientSprintPacket(packet.ToArray().ToList());

            clientSprintPacket.SprintState = PacketProcessor.Read8(ref packet);

            return clientSprintPacket;
        }

        public static ClientSlotPacket ProcessClientSlotPacket(ref Span<byte> packet)
        {
            var clientSlotPacket = new ClientSlotPacket(packet.ToArray().ToList());

            clientSlotPacket.Slot = PacketProcessor.Read8(ref packet);

            return clientSlotPacket;
        }

        public static byte[] CreateServerSlotPacket(byte slot)
        {
            var packet = new List<byte>();
            
            packet.Write8(0x73);
            packet.Write8(0x3d);

            packet.Write8(slot);

            return packet.ToArray();
        }

        public static ClientChatPacket ProcessClientChatPacket(ref Span<byte> packet)
        {
            var clientChatPacket = new ClientChatPacket(packet.ToArray().ToList());

            clientChatPacket.Message = PacketProcessor.ReadString(ref packet);

            return clientChatPacket;
        }

        public static byte[] CreateServerChatPacket(int charId, string message)
        {
            var packet = new List<byte>();

            packet.Write8(0x23);
            packet.Write8(0x2a);

            packet.Write32(charId);
            packet.WriteString(message);

            return packet.ToArray();
        }
    }
}