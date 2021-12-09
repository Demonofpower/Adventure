using System;
using System.Collections.Generic;
using System.Linq;
using Adv.Sniffer.Enums;
using Adv.Sniffer.Packets;

namespace Adv.Sniffer
{
    class PacketReverser
    {
        private readonly ServerType serverType;
        
        private readonly List<Packet> knownPackets;

        public PacketReverser(string name)
        {
            if(name == "master")
            {
                serverType = ServerType.Master;
            }
            else
            {
                serverType = ServerType.Game;
            }
            
            this.knownPackets = new List<Packet>();
            
            //Login

            knownPackets.Add(new Packet("ClientHello", "16030100d2010000ce0301", Sender.Client));
            knownPackets.Add(new Packet("ServerHello", "160301003a020000360301", Sender.Server));

            knownPackets.Add(new Packet("ClientHelloBack", "1603010086100000820080", Sender.Client));
            //knownPackets.Add(new Packet("ServerHelloBack", "16030100aa040000a600001c2000a090a2bacb7999005b94671682fec23a49", Sender.Server));
            
            knownPackets.Add(new Packet("ServerInfosA", "16030100aa040000a600001c2000a0b1af3913d1b77c5ab2465151bed7be1a", Sender.Server));

            knownPackets.Add(new Packet("???", "1703010020", Sender.Unknown));

            knownPackets.Add(new Packet("End?", "1503010020", Sender.Client));
            
            //-----
            
            
            //knownPackets.Add(new Packet("ClientXYZYP", "6d76", Sender.Server));

            //knownPackets.Add(new Packet("ServerNull", "0000", Sender.Server));
        }


        public bool Reverse(string data, Sender sender, Client client)
        {
            if (serverType == ServerType.Game)
            {
                return true;
            }
            
            var known = false;
            foreach (var knownPacket in knownPackets)
            {
                if (data.StartsWith(knownPacket.Id))
                {
                    //knownPacket.Print(data);
                    //known = true;
                }
            }

            if (!known)
            {
                Console.WriteLine(serverType + "#" + sender + "#" + data);
            }

            return true;
        }
    }
}