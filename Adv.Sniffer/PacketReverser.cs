using System;
using System.Collections.Generic;
using System.Linq;

namespace Adv.Sniffer
{
    class PacketReverser
    {
        private readonly List<Packet> knownPackets;

        public PacketReverser()
        {
            this.knownPackets = new List<Packet>();

            knownPackets.Add(new Packet("ClientHello", "16030100d2010000ce0301", Sender.Client));
            knownPackets.Add(new Packet("ServerHello", "160301003a020000360301", Sender.Server));

            knownPackets.Add(new Packet("ClientHelloBack", "1603010086100000820080", Sender.Client));
            knownPackets.Add(new Packet("ServerHelloBack",
                "16030100aa040000a600001c2000a090a2bacb7999005b94671682fec23a49", Sender.Server));

            knownPackets.Add(new Packet("KeepAlive", "1703010020", Sender.Unknown));

            knownPackets.Add(new Packet("ClientXYZYP", "6d76", Sender.Server));

            knownPackets.Add(new Packet("ServerNull", "0000", Sender.Server));
        }


        public bool Reverse(string data, Sender sender, string name, Client client)
        {
            var known = false;
            foreach (var knownPacket in knownPackets)
            {
                if (data.StartsWith(knownPacket.Id))
                {
                    knownPacket.Print(data);
                    known = true;

                    if (knownPacket.Name == "ClientXYZYP")
                    {
                        var h = HexArithmetic.StringToByteArray(data);
                        client.SendToServer(h);
                        return false;
                        client.SendToServer(knownPacket.Send(Packet.InterceptPlayerCoordsPacket(-39279.363f, -20140.918f, 2604.7231f, data)));
                        return false;
                    }
                }

                
            }

            return true;
            if (!known)
            {
                Console.WriteLine(name + "#" + sender + "#" + data);
            }
        }
    }
}