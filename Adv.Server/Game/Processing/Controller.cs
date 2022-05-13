using System;
using System.Diagnostics;
using System.Threading;

namespace Adv.Server.Game.Processing
{
    class Controller
    {
        private const int TickTime = 50;

        private readonly PacketManager packetManager;
        private ulong currTick;

        public Controller(PacketManager packetManager)
        {
            this.packetManager = packetManager;
            currTick = 0;
        }

        public void Loop()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //TODO
            currTick += 1;
            Console.WriteLine($"Tick {currTick}");
            packetManager.Flush();

            stopwatch.Stop();
            var sleepTime = stopwatch.Elapsed.Milliseconds;
            if (sleepTime < TickTime)
            {
                Thread.Sleep(TickTime - sleepTime);
            }
            else
            {
                Console.WriteLine($"TICK TOOK TOO LONG {currTick}");
            }
        }
    }
}
