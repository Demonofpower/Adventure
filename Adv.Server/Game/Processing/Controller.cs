using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using Adv.Server.Game.Model.Objects.GameObjects;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Processing
{
    class Controller
    {
        private const int TickTime = 100;

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
            //Console.WriteLine($"Tick {currTick}");
            for (var index = 0; index < gameObjects.Count; index++)
            {
                var gameObject = gameObjects[index];
                if (gameObject.IsFaded)
                {
                    gameObjects.Remove(gameObject);
                    continue;
                }

                gameObject.Tick();
            }

            CreatePackets();
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

        private void CreatePackets()
        {
            foreach (var gameObject in gameObjects)
            {
                switch (gameObject)
                {
                    case Fireball c:
                        var fireballUpdatePacket =
                            GameConnectionApi.CreateServerPositionPacket(c.actorId, c.position, c.rotation);
                        packetManager.Enqueue(fireballUpdatePacket);
                        break;
                    default:
                        throw new NotImplementedException(nameof(gameObject));
                    case null:
                        throw new ArgumentNullException(nameof(gameObject));
                }
            }
        }

        public void CreateFireball(Vector3 position, Rotation rotation)
        {
            var fireball = new Fireball(position, rotation);
            gameObjects.Add(fireball);

            Console.WriteLine($"Create Fireball {fireball.actorId} {fireball.position.X} {fireball.position.Y} {fireball.position.Z}");

            var fireballSpawnPacket = GameConnectionApi.CreateActorSpawnPacket(fireball.actorId, fireball.actorType,
                fireball.position, fireball.rotation, 16);
            packetManager.Enqueue(fireballSpawnPacket);
            packetManager.Flush();
        }
    }
}
