using System.Collections.Concurrent;

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

                foreach (var tcpClient in GameServer.sessions.Keys)
                {
                    var clientStream = tcpClient.GetStream();
                    clientStream.Write(currPacket);
                }
            }
        }

        public void Enqueue(byte[] packet)
        {
            queuedPackets.Enqueue(packet);
        }
    }
}
