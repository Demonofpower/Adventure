using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Adv.Server.Master;
using Adv.Server.Packets;
using Adv.Server.Packets.Game;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

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

        public static byte[] CreateServerHelloPacket(int charId, Vector3 position, Rotation rotation)
        {
            var packet = new List<byte>();

            packet.Write32(charId);

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

        public static byte[] CreateServerPositionPacket(int charId, Vector3 position, Rotation rotation)
        {
            var packet = new List<byte>();

            //PACKETID
            packet.Write8(0x6d);
            packet.Write8(0x76);

            //PLAYERID??
            packet.Write32(charId);

            packet.WriteVector3(position);
            packet.WriteRotation(rotation);

            packet.Write16(0x0);

            return packet.ToArray();
        }

        public static byte[] CreateActorSpawnPacket(int actorId, ActorType actor, Vector3 position, Rotation rotation)
        {
            var packet = new List<byte>();

            //PACKETID
            packet.Write8(0x6d);
            packet.Write8(0x6b);

            packet.Write32(actorId);
            packet.Write32(0x0);
            packet.Write8(0x0);
            packet.WriteString(actor.ToString());
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

        public static byte[] CreateServerPlayerJoinedPacket(Character character)
        {
            var packet = new List<byte>();

            packet.Write8(0x6e);
            packet.Write8(0x63);

            packet.Write32(character.Id);
            packet.WriteString(character.Name);
            packet.WriteString(character.User.Team.TeamName);
            
            packet.Write8(character.Avatar);
            packet.Write32(character.ColorA);
            packet.Write32(character.ColorB);
            packet.Write32(character.ColorC);
            packet.Write32(character.ColorD);
            
            packet.WriteVector3(character.Position);
            packet.WriteRotation(character.Rotation);

            //UNKNOWN
            packet.WriteString("");

            packet.Write32(character.Health);

            //UNKNOWN!!!
            short x = 0x0;
            packet.Write16(x);
            if (x > 0)
            {
                while (false)
                {
                    packet.WriteString("yyy");
                    packet.Write8(character.Avatar);
                }
            }


            return packet.ToArray();
        }

        public static byte[] CreateServerPlayerPositionPacket(Character character)
        {
            var packet = new List<byte>();

            packet.Write8(0x70);
            packet.Write8(0x70);

            packet.Write32(character.Id);
            packet.WriteVector3(character.Position);
            packet.WriteRotation(character.Rotation);

            //UNKNOWN
            packet.Write16(0x0);
            packet.Write16(0x0);
            packet.Write16(0x0);
            packet.Write8(0x0);
            packet.Write8(0x0);


            return packet.ToArray();
        }

        public static byte[] CreateServerStatePacket(int charId, State state, byte value)
        {
            var packet = new List<byte>();

            packet.Write8(0x73);
            packet.Write8(0x74);

            packet.Write32(charId);
            packet.WriteString(state.ToString());
            packet.Write8(value);

            return packet.ToArray();
        }

        public static ClientPvPEnablePacket ProcessClientPvPEnablePacket(ref Span<byte> packet)
        {
            var pvpEnablePacket = new ClientPvPEnablePacket(packet.ToArray().ToList());

            pvpEnablePacket.State = PacketProcessor.Read8(ref packet);
            
            return pvpEnablePacket;
        }

        public static byte[] CreateServerPvpEnablePacket(byte state)
        {
            var packet = new List<byte>();

            packet.Write8(0x70);
            packet.Write8(0x76);

            packet.Write8(state);

            return packet.ToArray();
        }

        public static byte[] CreateServerPvpCountdownUpdatePacket(byte state, int timeLeft)
        {
            var packet = new List<byte>();

            packet.Write8(0xe2);
            packet.Write8(0x18);

            packet.Write8(state);
            
            packet.Write32(timeLeft);

            return packet.ToArray();
        }

        public static ClientUsePacket ProcessClientUsePacket(ref Span<byte> packet)
        {
            var clientUsePacket = new ClientUsePacket(packet.ToArray().ToList());

            clientUsePacket.ItemId = PacketProcessor.Read32(ref packet);

            return clientUsePacket;
        }
    }
}