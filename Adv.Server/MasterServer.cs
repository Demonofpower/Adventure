using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Adv.Server.Master;
using Adv.Server.Master.Enums;
using Adv.Server.Packets.Master;

namespace Adv.Server
{
    class MasterServer
    {
        public static List<User> Users;
        public static List<Team> Teams;
        public static List<Quest> Quests;
        public static List<Item> Items;
        public static List<Achievement> Achievements;

        private static X509Certificate serverCertificate = null;

        private Dictionary<TcpClient, User> loggedInUser;

        public void Start(int port, string certificate)
        {
            Populate();

            serverCertificate = X509Certificate2.CreateFromSignedFile(certificate);

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                TcpClient client = listener.AcceptTcpClient();
                ProcessClient(client);
            }
        }

        private void ProcessClient(TcpClient client)
        {
            var sslStream = new SslStream(client.GetStream(), false);
            try
            {
                sslStream.AuthenticateAsServer(serverCertificate, false, true);

                sslStream.ReadTimeout = int.MaxValue;
                sslStream.WriteTimeout = int.MaxValue;

                loggedInUser.Add(client, null);

                var buffer = MasterConnectionApi.CreateWelcomePacket(5, "Custom Server", "By Paranoia with <3");
                Console.WriteLine("Sending hello message.");
                sslStream.Write(buffer);

                var t = new Thread(() => SendOk(sslStream));
                t.Start();
                
                while (true)
                {
                    List<byte> packet = ReadMessage(sslStream);
                    var reply = GetNewMessageAndCraftAnswer(packet, client);
                    Console.WriteLine("Msg got!");

                    if (reply.Length == 0) continue;
                    if (reply[0] == 0x81) break;

                    sslStream.Write(reply);
                    Console.WriteLine("Msg answer sent!");
                }
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }

                Console.WriteLine("Authentication failed - closing the connection.");
            }
            finally
            {
                sslStream.Close();
                client.Close();
            }
        }

        private void SendOk(SslStream s)
        {
            while (true)
            {
                Thread.Sleep(10000);
                s.Write(new byte[] { 0x80 });
            }
        }
        
        private List<byte> ReadMessage(SslStream sslStream)
        {
            byte[] buffer = new byte[2048];
            int bytes = 0;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
            } while (bytes == 0);

            return buffer.ToList();
        }

        private byte[] GetNewMessageAndCraftAnswer(List<byte> packet, TcpClient client)
        {
            var currentUser = GetUserByTcpClient(client);
            Character currentCharacter = null;
            var masterPacketType = Enum.Parse<MasterPacketType>(packet[0].ToString());

            switch (masterPacketType)
            {
                case MasterPacketType.Login:
                    var clientLoginPacket = MasterConnectionApi.ProcessClientLoginPacket(packet.ToArray());

                    loggedInUser[client] = Users.FirstOrDefault(u => u.Username == clientLoginPacket.Username);

                    return MasterConnectionApi.CreateServerLoginPacket(Users.FirstOrDefault(u =>
                        u.Username == clientLoginPacket.Username && u.Password == clientLoginPacket.Password));
                case MasterPacketType.Register:
                    break;
                case MasterPacketType.GetPlayerCounts:
                    return MasterConnectionApi.CreateServerPlayerCountPacket();
                case MasterPacketType.GetTeammates:
                    break;
                case MasterPacketType.CharacterList:
                    return MasterConnectionApi.CreateServerCharacterListPacket(currentUser);
                case MasterPacketType.CreateCharacter:
                    break;
                case MasterPacketType.DeleteCharacter:
                    break;
                case MasterPacketType.JoinGameServer:
                    var clientJoinGameServerPacket = MasterConnectionApi.ProcessClientJoinGameServerPacket(packet.ToArray());
                    currentCharacter = GetCharacterById(clientJoinGameServerPacket.CharacterId, currentUser);
                    
                    return MasterConnectionApi.CreateServerJoinGameServerPacket(currentUser, currentCharacter);
                case MasterPacketType.ValidateCharacterToken:
                    break;
                case MasterPacketType.AddServerToPool:
                    break;
                case MasterPacketType.CharacterRegionChange:
                    break;
                case MasterPacketType.StartQuest:
                    break;
                case MasterPacketType.UpdateQuest:
                    break;
                case MasterPacketType.CompleteQuest:
                    break;
                case MasterPacketType.SetActiveQuest:
                    break;
                case MasterPacketType.UpdateItems:
                    break;
                case MasterPacketType.MarkAsPickedUp:
                    break;
                case MasterPacketType.GetFlag:
                    break;
                case MasterPacketType.SubmitFlag:
                    break;
                case MasterPacketType.SubmitAnswer:
                    break;
                case MasterPacketType.NoAction:
                    return MasterConnectionApi.CreateServerNoActionPacket();
                case MasterPacketType.End:
                    return new byte[] {0x81};
                default: throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        private void Populate()
        {
            loggedInUser = new Dictionary<TcpClient, User>();

            Quests = new List<Quest>();
            Quests.Add(new Quest() { Name = "LostCave" });
            
            Items = new List<Item>();
            Achievements = new List<Achievement>();

            Teams = new List<Team>();
            Teams.Add(new Team("Dev", "133769420"));

            var userCharList = new List<Character>();
            userCharList.Add(new Character(1, "Juli", Location.TODO, 1, 1, 1, 1, 1, 1, true));

            Users = new List<User>();
            Users.Add(new User("j", "j", 1, Teams[0], true, userCharList));
        }

        private User GetUserByTcpClient(TcpClient client)
        {
            return loggedInUser[client];
        }

        private Character GetCharacterById(int id, User user)
        {
            return user.Characters.FirstOrDefault();

            //TODO!!!
            return user.Characters.FirstOrDefault(c => id == c.Id);
        }
    }
}