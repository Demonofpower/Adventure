using System.Linq;
using System.Net.Sockets;
using Adv.Server.Master;

namespace Adv.Server.Util
{
    public static class ClientHelper
    {
        public static TcpClient GeTcpClientByCharacter(Character character)
        {
            return GameServer.sessions.First(s => s.Value.Item2 == character).Key;
        }


        public static void SendToAllExceptClient(byte[] packet, TcpClient client)
        {
            foreach (var session in GameServer.sessions)
            {
                if (session.Value != null && session.Key != client)
                {
                    session.Key.GetStream().Write(packet);
                }
            }
        }

        public static void SendToAllExceptCharacter(byte[] packet, Character character)
        {
            foreach (var session in GameServer.sessions)
            {
                if (session.Value != null && session.Key != GeTcpClientByCharacter(character))
                {
                    session.Key.GetStream().Write(packet);
                }
            }
        }
    }
}
