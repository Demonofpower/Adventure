using System;
using System.Collections.Generic;
using System.Threading;
using Adv.Server.Master;
using Adv.Server.Util.Database;
using Adv.Server.Util.Database.API;

namespace Adv.Server
{
    class Program
    {
        private static MasterServer masterServer;
        private static GameServer gameServer;
        
        public static int Main(string[] args)
        {
            //var db = new DatabaseConnection("Server=localhost;Port=3306;Uid=Juli;Pwd=pwnadventure3;");
            var db = new FakeDatabaseConnection();
            
            masterServer = new MasterServer();
            var masterServerThread = new Thread(() => StartMasterServer(db));
            masterServerThread.Start();
            
            gameServer = new GameServer();
            var gameServerThread = new Thread(StartGameServer);
            gameServerThread.Start();
            
            Console.WriteLine("Servers are running..");
            Console.ReadLine();
            
            return 0;
        }

        private static void StartMasterServer(IDatabaseConnection dbConnection)
        {
            masterServer.Start(3333, @"C:\Users\Juli\Desktop\ssl\192.168.178.32.pfx", dbConnection);
        }

        private static void StartGameServer()
        {
            gameServer.Start(3003);
        }
    }
}

