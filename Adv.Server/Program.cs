using System;
using System.Threading;
using Adv.Server.Util.Database;

namespace Adv.Server
{
    class Program
    {
        private static MasterServer masterServer;
        private static GameServer gameServer;
        
        public static int Main(string[] args)
        {
            var db = new DatabaseConnection("Server=localhost;Port=3306;Uid=Juli;Pwd=pwnadventure3;");
            
            masterServer = new MasterServer();
            var masterServerThread = new Thread(StartMasterServer);
            masterServerThread.Start();
            
            gameServer = new GameServer();
            var gameServerThread = new Thread(StartGameServer);
            gameServerThread.Start();
            
            Console.WriteLine("Servers are running..");
            Console.ReadLine();
            
            return 0;
        }

        private static void StartMasterServer()
        {
            masterServer.Start(3333, @"C:\Users\Juli\Desktop\ssl\192.168.178.32.pfx");
        }

        private static void StartGameServer()
        {
            gameServer.Start(3003);
        }
    }
}

