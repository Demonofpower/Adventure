using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CANAPE.Net.Templates;

namespace Adv.Sniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            var master = new Server("192.168.178.32", 3333, "142.93.101.220", 3333, true, "master");
            var game = new Server("192.168.178.32", 3002, "142.93.101.220", 3002, true, "game");

            master.Start();
            game.Start();

            Console.ReadLine();
        }
    }
}