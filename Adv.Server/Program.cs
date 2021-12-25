using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Adv.Server
{
    class Program
    {
        private static MasterServer masterServer;
        
        public static int Main(string[] args)
        {
            masterServer = new MasterServer();
            var masterServerThread = new Thread(StartMasterServer);
            masterServerThread.Start();


            Console.WriteLine("Servers are running..");
            Console.ReadLine();
            
            return 0;
        }

        private static void StartMasterServer()
        {
            masterServer.Start(@"C:\Users\Juli\Desktop\ssl\192.168.178.32.pfx");
        }
    }
}

