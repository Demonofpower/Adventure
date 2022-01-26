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
using Adv.Server.Packets;
using Adv.Server.Packets.Master;
using Adv.Server.Util.Database;
using Adv.Server.Util.Database.API;
using Adv.Server.Util.Enums;

namespace Adv.Server
{
    class MasterServer
    {
        public static List<User> Users;
        public static List<Team> Teams;
        public static List<Character> Characters;
        public static List<Quest> Quests;
        public static List<Item> Items;
        public static List<Achievement> Achievements;

        private Dictionary<TcpClient, User> loggedInUser;

        private DatabaseConnection dbConnection = null;

        private static X509Certificate serverCertificate;

        public void Start(int port, string certificate, DatabaseConnection dbConnection)
        {
            this.dbConnection = dbConnection;

            Populate();

            serverCertificate = X509Certificate.CreateFromSignedFile(certificate);

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                new Thread(() => ProcessClient(client)).Start();
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
                sslStream.Write(buffer);

                while (true)
                {
                    List<byte> packet = ReadMessage(sslStream);
                    var arrayPacket = new Span<byte>(packet.ToArray());

                    var reply = GetNewMessageAndCraftAnswer(arrayPacket, client);

                    if (reply.Length == 0) continue;
                    if (reply[0] == 0x81) break;

                    sslStream.Write(reply);
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
                loggedInUser.Remove(client);
            }
        }

        private List<byte> ReadMessage(SslStream sslStream)
        {
            byte[] buffer = new byte[2048];
            int bytes;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
            } while (bytes == 0);

            return buffer.ToList();
        }

        private byte[] GetNewMessageAndCraftAnswer(Span<byte> packet, TcpClient client)
        {
            var currentUser = GetUserByTcpClient(client);
            Character currentCharacter = null;
            var masterPacketType = Enum.Parse<MasterPacketType>(PacketProcessor.Read8(ref packet).ToString());

            switch (masterPacketType)
            {
                case MasterPacketType.Login:
                    var clientLoginPacket = MasterConnectionApi.ProcessClientLoginPacket(packet);
                    Console.WriteLine(
                        $"UserLogin - User: {clientLoginPacket.Username} PW: {clientLoginPacket.Password}");

                    var user = Users.FirstOrDefault(u =>
                        u.Username == clientLoginPacket.Username && u.Password == clientLoginPacket.Password);

                    if (user != null)
                    {
                        loggedInUser[client] = user;
                        Console.WriteLine("Logged in.");
                    }
                    else
                    {
                        Console.WriteLine("Username or Password incorrect!");
                    }

                    return MasterConnectionApi.CreateServerLoginPacket(user);
                case MasterPacketType.Register:
                    var clientRegisterPacket = MasterConnectionApi.ProcessClientRegisterPacket(packet);
                    Console.WriteLine(
                        $"UserRegister - Name: {clientRegisterPacket.Username} Team: {clientRegisterPacket.TeamNameOrHash} Pw: {clientRegisterPacket.Password}");

                    var team = new Team(clientRegisterPacket.TeamNameOrHash, clientRegisterPacket.TeamNameOrHash);
                    var newUser = new User(clientRegisterPacket.Username, clientRegisterPacket.Password, team, false,
                        new List<Character>());
                    var addUserResult = DatabaseUserApi.AddUser(newUser, Teams, dbConnection);

                    Reload();

                    if (addUserResult)
                    {
                        loggedInUser[client] = Users.First(u => u.Username == newUser.Username);
                        return MasterConnectionApi.CreateServerRegisterPacket(newUser, null);
                    }
                    else
                    {
                        return MasterConnectionApi.CreateServerRegisterPacket(null,
                            "An error occurred while creating your account!");
                    }
                case MasterPacketType.GetPlayerCounts:
                    return MasterConnectionApi.CreateServerPlayerCountPacket(
                        loggedInUser.Count(u => u.Value != null && u.Value.Team.Id == currentUser.Team.Id) - 1,
                        loggedInUser.Count(u => u.Value != null));
                case MasterPacketType.GetTeammates:
                    break;
                case MasterPacketType.CharacterList:
                    return MasterConnectionApi.CreateServerCharacterListPacket(currentUser);
                case MasterPacketType.CreateCharacter:
                    var clientCreateCharacterPacket = MasterConnectionApi.ProcessClientCreateCharacterPacket(packet);
                    Console.WriteLine(
                        $"CreateCharacter - Name: {clientCreateCharacterPacket.Name} User: {currentUser.Username}");


                    var character = new Character(clientCreateCharacterPacket.Name, Location.TODO,
                        clientCreateCharacterPacket.Avatar, clientCreateCharacterPacket.ColorA,
                        clientCreateCharacterPacket.ColorB, clientCreateCharacterPacket.ColorC,
                        clientCreateCharacterPacket.ColorD, 0, false, currentUser);

                    var createCharacterResult = DatabaseCharacterApi.AddCharacter(character, dbConnection);

                    Reload();

                    if (createCharacterResult)
                    {
                        return MasterConnectionApi.CreateServerCreateCharacterPacket(
                            Characters.First(c => c.Name == character.Name));
                    }
                    else
                    {
                        return MasterConnectionApi.CreateServerCreateCharacterPacket(null,
                            "An error occurred while creating your character!");
                    }
                case MasterPacketType.DeleteCharacter:
                    break;
                case MasterPacketType.JoinGameServer:
                    var clientJoinGameServerPacket =
                        MasterConnectionApi.ProcessClientJoinGameServerPacket(packet.ToArray());
                    currentCharacter = GetCharacterById(clientJoinGameServerPacket.CharacterId);

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
            Quests.Add(new Quest() {Name = "LostCave"});

            Items = new List<Item>();
            Achievements = new List<Achievement>();

            Teams = new List<Team>();
            var dbTeams = DatabaseTeamApi.GetAllTeams(dbConnection);
            if (dbTeams != null)
            {
                Teams.AddRange(dbTeams);
            }

            Users = new List<User>();
            var dbUsers = DatabaseUserApi.GetAllUsers(dbConnection, Teams);
            if (dbUsers != null)
            {
                Users.AddRange(dbUsers);
            }

            Characters = new List<Character>();
            var dbCharacters = DatabaseCharacterApi.GetAllCharacters(dbConnection, Users);
            if (dbCharacters != null)
            {
                Characters.AddRange(dbCharacters);
            }

            foreach (var user in Users)
            {
                user.Characters.AddRange(Characters.Where(c => c.User == user));
            }
        }

        private void Reload()
        {
            Teams.Clear();
            var dbTeams = DatabaseTeamApi.GetAllTeams(dbConnection);
            if (dbTeams != null)
            {
                Teams.AddRange(dbTeams);
            }

            Users.Clear();
            var dbUsers = DatabaseUserApi.GetAllUsers(dbConnection, Teams);
            if (dbUsers != null)
            {
                Users.AddRange(dbUsers);
            }

            Characters.Clear();
            var dbCharacters = DatabaseCharacterApi.GetAllCharacters(dbConnection, Users);
            if (dbCharacters != null)
            {
                Characters.AddRange(dbCharacters);
            }

            foreach (var user in Users)
            {
                user.Characters.AddRange(Characters.Where(c => c.User == user));
            }
        }

        private User GetUserByTcpClient(TcpClient client)
        {
            return loggedInUser[client];
        }

        private Character GetCharacterById(int id)
        {
            return Characters.FirstOrDefault(c => id == c.Id);
        }
    }
}