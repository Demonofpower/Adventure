using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Adv.Server.Game.Processing
{
    class PacketManager
    {
        private readonly ConcurrentQueue<PacketManagerPacket> queuedPackets;

        public PacketManager()
        {
            queuedPackets = new ConcurrentQueue<PacketManagerPacket>();
        }

        public void Flush()
        {
            var packetsInQueue = queuedPackets.Count;

            for (int i = 0; i < packetsInQueue; i++)
            {
                var success = queuedPackets.TryDequeue(out var currPacket);

                if (currPacket.ReceiverType == PacketReceiver.All)
                {
                    foreach (var tcpClient in GameServer.sessions.Keys)
                    {
                        var clientStream = tcpClient.GetStream();
                        clientStream.Write(currPacket.Data);
                    }
                }
                else
                {
                    foreach (var tcpClient in currPacket.Receivers)
                    {
                        var clientStream = tcpClient.GetStream();
                        clientStream.Write(currPacket.Data);
                    }
                }
            }
        }

        public void Enqueue(PacketManagerPacket packet)
        {
            queuedPackets.Enqueue(packet);
        }

        public void Enqueue(byte[] packet)
        {
            queuedPackets.Enqueue(new PacketManagerPacket(packet));
        }
    }

    struct PacketManagerPacket
    {
        public byte[] Data { get; set; }
        public PacketReceiver ReceiverType { get; set; }
        public List<TcpClient> Receivers { get; set; }

        public PacketManagerPacket(byte[] data)
        {
            Data = data;
            ReceiverType = PacketReceiver.All;
            Receivers = new List<TcpClient>();
        }

        public PacketManagerPacket(byte[] data, List<TcpClient> receivers)
        {
            Data = data;
            ReceiverType = PacketReceiver.Specific;
            Receivers = receivers;
        }

        public PacketManagerPacket(byte[] data, TcpClient receiver)
        {
            Data = data;
            ReceiverType = PacketReceiver.Specific;
            Receivers = new List<TcpClient>() {receiver};
        }
    }

    enum PacketReceiver
    {
        All,
        Specific
    }
}
