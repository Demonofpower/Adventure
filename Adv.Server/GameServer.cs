using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using Adv.Server.Master;

namespace Adv.Server
{
    class GameServer
    {
        public void Start(int port)
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                ProcessClient(client);
            }
        }

        private void ProcessClient(TcpClient client)
        {
            var networkStream = client.GetStream();
            try
            {
                networkStream.ReadTimeout = int.MaxValue;
                networkStream.WriteTimeout = int.MaxValue;

                //var buffer = MasterConnectionApi.CreateWelcomePacket(5, "Custom Server", "By Paranoia with <3");
                //Console.WriteLine("Sending hello message.");
                //networkStream.Write(buffer);

                while (true)
                {
                    List<byte> packet = ReadMessage(networkStream);
                    //var reply = GetNewMessageAndCraftAnswer(packet, client);
                    Console.WriteLine("Msg got!");

                    //if (reply.Length == 0) continue;
                    //if (reply[0] == 0x81) break;

                    //networkStream.Write(reply);
                    Console.WriteLine("Msg answer sent!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
            }
            finally
            {
                networkStream.Close();
                client.Close();
            }
        }

        private static List<byte> ReadMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[2048];
            int bytes = 0;
            do
            {
                bytes = stream.Read(buffer, 0, buffer.Length);
            } while (bytes == 0);

            return buffer.ToList();
        }
    }
}
