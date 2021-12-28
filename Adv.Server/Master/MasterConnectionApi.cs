using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adv.Server.Packets;
using Adv.Server.Packets.Master;

namespace Adv.Server.Master
{
    class MasterConnectionApi
    {
        public static MasterPacketType GetMasterPacketType(byte[] buffer)
        {
            return (MasterPacketType) buffer[0];
        }

        public static byte[] CreateWelcomePacket(int version, string title, string text)
        {
            var buffer = new List<byte>();
            byte[] welcome = Encoding.ASCII.GetBytes("PWN3");
            buffer.AddRange(welcome);

            buffer.Add((byte) version);
            buffer.Add(0x00);

            var titleSize = title.Length;
            buffer.Add((byte) titleSize);
            buffer.Add(0x0);
            byte[] titlePacket = Encoding.ASCII.GetBytes(title);
            buffer.AddRange(titlePacket);

            var textLength = text.Length;
            buffer.Add((byte) textLength);
            buffer.Add(0x0);
            byte[] textPacket = Encoding.ASCII.GetBytes(text);
            buffer.AddRange(textPacket);

            return buffer.ToArray();
        }

        public static ClientLoginPacket ProcessClientLoginPacket(byte[] packet)
        {
            var clientLoginPacket = new ClientLoginPacket(packet.ToList())
            {
                Id = PacketProcessor.Read8(ref packet),
                Username = PacketProcessor.ReadString(ref packet),
                Password = PacketProcessor.ReadString(ref packet)
            };

            return clientLoginPacket;
        }

        public static ClientJoinGameServerPacket ProcessClientJoinGameServerPacket(byte[] packet)
        {
            var clientJoinGameServerPacket = new ClientJoinGameServerPacket(packet.ToList())
            {
                CharacterId = PacketProcessor.Read32(ref packet)
            };

            return clientJoinGameServerPacket;
        }

        public static byte[] CreateServerLoginPacket(User user)
        {
            var buffer = new List<byte>();

            buffer.Add(0x1);

            buffer.Write32(user.Id);
            buffer.WriteString(user.Team.SecretTeamName);
            buffer.WriteString(user.Team.TeamName);
            buffer.Write8(user.IsAdmin);

            return buffer.ToArray();
        }

        public static byte[] CreateServerNoActionPacket()
        {
            var buffer = new List<byte>();

            return buffer.ToArray();
        }

        public static byte[] CreateServerCharacterListPacket(User user)
        {
            var buffer = new List<byte>();

            var charCount = user.Characters.Count;
            buffer.Write16((short) charCount);

            foreach (var character in user.Characters)
            {
                buffer.Write32(character.Id);
                buffer.WriteString(character.Name);
                buffer.WriteString(character.Location.ToString());
                buffer.Write8(character.Avatar);
                buffer.Write32(character.ColorA);
                buffer.Write32(character.ColorB);
                buffer.Write32(character.ColorC);
                buffer.Write32(character.ColorD);
                buffer.Write32(character.Flags);
                buffer.Write8(character.IsAdmin);
            }

            return buffer.ToArray();
        }

        public static byte[] CreateServerPlayerCountPacket()
        {
            var buffer = new List<byte>();

            //TODO!!!
            buffer.Write32(69);
            //TODO!!!
            buffer.Write32(420);

            return buffer.ToArray();
        }

        public static byte[] CreateServerJoinGameServerPacket(User user, Character character)
        {
            var buffer = new List<byte>();

            //Success
            buffer.Write8(0x1);
            //Server available
            buffer.Write8(0x1);
            buffer.WriteString("192.168.178.32");
            buffer.Write16(3003);
            //Token TODO!!!
            buffer.WriteString("token1");
            buffer.WriteString(character.Name);
            buffer.WriteString(user.Team.TeamName);
            buffer.Write8(character.IsAdmin && user.IsAdmin);

            //TODO!!!
            var quests = MasterServer.Quests;
            buffer.Write16((short) quests.Count);
            foreach (var quest in quests)
            {
                buffer.WriteString(quest.Name);
                //Quest state
                buffer.WriteString("Todo");
                //unknown count
                buffer.Write32(1);
            }
            
            //Current quest
            buffer.WriteString(quests[0].Name);
            
            //TODO!!!
            var items = MasterServer.Items;
            buffer.Write16((short) items.Count);
            foreach (var item in items)
            {
                buffer.WriteString(item.Name);
                //Unknown
                buffer.Write32(1);
                //Unknown
                buffer.Write16(1);
            }

            //TODO!!!
            foreach (var item in items)
            {
                //Unknown maybe weapon name in slot MAYBE in WHILE???
                buffer.WriteString("unknown");
            }
           
            //Current slot?
            buffer.Write8(0);
            
            //TODO!!
            var achievements = MasterServer.Achievements;
            buffer.Write16((short) achievements.Count);
            foreach (var achievement in achievements)
            {
                buffer.WriteString(achievement.Name);
            }
            
            return buffer.ToArray();
        }
    }
}