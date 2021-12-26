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
                Id = PacketProcessor.Read8(packet),
                Username = PacketProcessor.ReadString(packet),
                Password = PacketProcessor.ReadString(packet)
            };
            
            return clientLoginPacket;
        }

        public static byte[] CreateLoginPacket(User user)
        {
            var buffer = new List<byte>();
            
            buffer.Add(0x1);
            
            buffer.Write32(user.Id);
            buffer.WriteString(user.Team.SecretTeamName);
            buffer.WriteString(user.Team.TeamName);
            buffer.Write8(user.IsAdmin);
            
            return buffer.ToArray();
        }
    }
}