using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Adv.Server
{
    class MasterServer
    {
        private static X509Certificate serverCertificate = null;

        public void Start(string certificate)
        {
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

                sslStream.ReadTimeout = 5000;   
                sslStream.WriteTimeout = 5000;

                var buffer = new List<byte>();
                byte[] welcome = Encoding.UTF8.GetBytes("PWN3");
                buffer.AddRange(welcome);
                
                buffer.Add(0x05);
                buffer.Add(0x00);

                buffer.Add(0x3);
                buffer.Add(0x0);
                byte[] title = Encoding.UTF8.GetBytes("HaX");
                buffer.AddRange(title);

                buffer.Add(0x8);
                buffer.Add(0x0);
                byte[] msg = Encoding.UTF8.GetBytes("Paranoia");
                buffer.AddRange(msg);

                Console.WriteLine("Sending hello message.");

                sslStream.Write(buffer.ToArray());

                Console.WriteLine("Waiting for client message...");
                string messageData = ReadMessage(sslStream);
                Console.WriteLine("Received: {0}", messageData);
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

        static string ReadMessage(SslStream sslStream)
        {
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                if (messageData.ToString().IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            return messageData.ToString();
        }
    }
}