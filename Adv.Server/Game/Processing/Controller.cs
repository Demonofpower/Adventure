using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Adv.Server.Game.Model.GameObjects;

namespace Adv.Server.Game.Processing
{
    class Controller
    {
        private const int TickTime = 50;

        private readonly PacketManager packetManager;
        private ulong currTick;

        private List<GameObject> gameObjects;

        public Controller(PacketManager packetManager)
        {
            this.packetManager = packetManager;
            currTick = 0;

            gameObjects = new List<GameObject>();
        }

        public void Start()
        {
            new Thread(() =>
            {
                while (true)
                {
                    Loop();
                }
            }).Start();
        }

        private void Loop()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //TODO
            currTick += 1;
            Console.WriteLine($"Tick {currTick}");
            foreach (var gameObject in gameObjects)
            {
                gameObject.Tick();
            }
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

            CreateFireball();
        }

        public void CreateFireball()
        {
            gameObjects.Add(new Fireball());
        }
    }
}
