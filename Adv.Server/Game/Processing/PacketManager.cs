using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Adv.Server.Game.Processing
{
    class PacketManager
    {
        private readonly ConcurrentQueue<byte[]> queuedPackets;

        public PacketManager()
        {
            queuedPackets = new ConcurrentQueue<byte[]>();
        }

        public void Flush()
        {
            var packetsInQueue = queuedPackets.Count;

            for (int i = 0; i < packetsInQueue; i++)
            {
                var success = queuedPackets.TryDequeue(out var currPacket);

                //TODO
            }
        }

        public void Enqueue(byte[] packet)
        {
            queuedPackets.Enqueue(packet);
        }
    }
}
