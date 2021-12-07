using System;
using Adv.Sniffer.Enums;

namespace Adv.Sniffer.Packets
{
    class Packet
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Sender Sender { get; set; }

        public Packet(string name, string id, Sender sender)
        {
            Name = name;
            Id = id;
            Sender = sender;
        }

        public void Print(string data)
        {
            var newData = data.Replace(Id, "");
            switch (Name)
            {
                case "ClientXYZYP":
                    Console.WriteLine(data);
                    break;
                    var xHex = HexArithmetic.HexToFloat(newData.Substring(0, 8));
                    var yHex = HexArithmetic.HexToFloat(newData.Substring(8, 8));
                    var zHex = HexArithmetic.HexToFloat(newData.Substring(16, 8));
                    var remainingData = newData.Remove(0, 24);
                    Console.WriteLine(Name + " " + Id + " " + xHex + " " + yHex + " " + zHex + " " + remainingData);
                    break;
                default:
                    Console.WriteLine(Sender + "#" + Name);
                    break;
            }
        }

        public byte[] Send(string data)
        {
            var h = HexArithmetic.StringToByteArray(data);
            return h;
        }

        public static string InterceptPlayerCoordsPacket(float x, float y, float z, string oldData)
        {
            var s = oldData.Remove(4, 24).Insert(4,
                HexArithmetic.FloatToHex(x) + HexArithmetic.FloatToHex(y) + HexArithmetic.FloatToHex(z));
            Console.WriteLine(oldData);
            Console.WriteLine(s);
            return s;
        }
    }
}