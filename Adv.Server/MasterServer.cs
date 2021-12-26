using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Adv.Server.Master;

namespace Adv.Server
{
    class MasterServer
    {
        private static X509Certificate serverCertificate = null;

        public static List<User> Users;
        public static List<Team> Teams;

        public void Start(string certificate)
        {
            Populate();

            serverCertificate = X509Certificate2.CreateFromSignedFile(certificate);

            TcpListener listener = new TcpListener(IPAddress.Any, 3333);
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
            SslStream sslStream = new SslStream(client.GetStream(), false);
            try
            {
                sslStream.AuthenticateAsServer(serverCertificate, false, true);

                sslStream.ReadTimeout = int.MaxValue;
                sslStream.WriteTimeout = int.MaxValue;

                var buffer = MasterConnectionApi.CreateWelcomePacket(5, "Custom Server", "By Paranoia with <3");
                Console.WriteLine("Sending hello message.");
                sslStream.Write(buffer);

                while (true)
                {
                    List<byte> packet = ReadMessage(sslStream);
                    var reply = GetNewMessage(packet);
                    Console.WriteLine("Login attempt got!");
                    sslStream.Write(reply);
                    Console.WriteLine("Login reply sent!");
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
                sslStream.Close();
                client.Close();
                return;
            }
            finally
            {
                sslStream.Close();
                client.Close();
            }
        }

        static List<byte> ReadMessage(SslStream sslStream)
        {
            byte[] buffer = new byte[2048];
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
            } while (bytes != 0);

            return buffer.ToList();
        }

        private byte[] GetNewMessage(List<byte> packet)
        {
            var clientLoginPacket = MasterConnectionApi.ProcessClientLoginPacket(packet.ToArray());

            return MasterConnectionApi.CreateLoginPacket(Users.FirstOrDefault(u => u.Username == clientLoginPacket.Username && u.Password == clientLoginPacket.Password));
        }

        private void Populate()
        {
            Teams = new List<Team>();
            Teams.Add(new Team("Dev", "133769420"));

            Users = new List<User>();
            Users.Add(new User("1", "j", 1, Teams[0], true));
        }
    }
}