using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using Adv.Server.Game.Model.Objects;
using Adv.Server.Game.Model.Objects.EventObjects;
using Adv.Server.Game.Model.Objects.GameObjects;
using Adv.Server.Master;
using Adv.Server.Util;

namespace Adv.Server.Game.Processing
{
    class Controller
    {
        public static PacketManager PacketManager;

        private const int TickTime = 100;

        private ulong currTick;

        private List<IObject> gameObjects;

        public Controller(PacketManager packetManager)
        {
            PacketManager = packetManager;
            currTick = 0;

            gameObjects = new List<IObject>();
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

            UpdateHealth();
            UpdateMana();
            
            CreatePackets();
            PacketManager.Flush();

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

        private void UpdateHealth()
        {
            foreach (var client in GameServer.sessions)
            {
                if (client.Value.Item2.Health < 100)
                {
                    var healthUpdatePacket = GameConnectionApi.CreateServerHealthUpdatePacket(++client.Value.Item2.Health);
                    PacketManager.Enqueue(new PacketManagerPacket(healthUpdatePacket, ClientHelper.GeTcpClientByCharacter(client.Value.Item2)));
                }
            }
        }

        private void UpdateMana()
        {
            foreach (var client in GameServer.sessions)
            {
                if (client.Value.Item2.Mana < 100)
                {
                    var manaUpdatePacket = GameConnectionApi.CreateServerManaUpdatePacket(++client.Value.Item2.Mana);
                    PacketManager.Enqueue(new PacketManagerPacket(manaUpdatePacket, ClientHelper.GeTcpClientByCharacter(client.Value.Item2)));
                }
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
                        PacketManager.Enqueue(fireballUpdatePacket);
                        
                        if (c.IsFaded)
                        {
                            PacketManager.Enqueue(GameConnectionApi.CreateActorDestroyPacket(c.actorId));
                        }
                        break;
                    default:
                        Console.WriteLine($"Not implemented packet {gameObject.GetType()}");
                        return;
                    case null:
                        throw new ArgumentNullException(nameof(gameObject));
                }
            }
        }

        public void CreateFireball(Vector3 position, Rotation rotation, Character sender)
        {
            var fireball = new Fireball(position, rotation);
            gameObjects.Add(fireball);

            sender.Mana -= 5;

            Console.WriteLine($"Create Fireball {fireball.actorId} {fireball.position.X} {fireball.position.Y} {fireball.position.Z}");

            var fireballSpawnPacket = GameConnectionApi.CreateActorSpawnPacket(fireball.actorId, fireball.actorType,
                fireball.position, fireball.rotation, sender.Id, 0x64);

            PacketManager.Enqueue(GameConnectionApi.CreateServerManaUpdatePacket(sender.Mana));
            PacketManager.Enqueue(fireballSpawnPacket);
            PacketManager.Enqueue(GameConnectionApi.CreateServerPositionPacket(fireball.actorId, fireball.position, fireball.rotation));
            PacketManager.Flush();
        }

        public void CreatePvPEnableEvent(bool enable, Character sender)
        {
            var oldPvPEvent = gameObjects.FirstOrDefault(o =>
            {
                if (o is PvPEnableEvent e)
                {
                    if (e.EventCharacter == sender)
                    {
                        return true;
                    }
                }

                return false;
            });
            
            if (oldPvPEvent != null)
            {
                gameObjects.Remove(oldPvPEvent);
                return;
            }
            
            var pvpEvent = new PvPEnableEvent(enable, sender);
            gameObjects.Add(pvpEvent);
        }
    }
}
