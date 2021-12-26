using System;
using System.Collections.Generic;
using System.Linq;

namespace Adv.Server.Packets
{
    public static class PacketReader
    {
        public static byte Read8(IList<byte> packet)
        {
            var result = packet[0];
            packet.RemoveAt(0);

            return result;
        }

        public static short Read16(IList<byte> packet)
        {
            var result = BitConverter.ToInt16(packet.ToArray(), 0);
            packet.RemoveAt(0);
            packet.RemoveAt(0);

            return result;
        }

        public static int Read32(IList<byte> packet)
        {
            var result = BitConverter.ToInt32(packet.ToArray(), 0);
            packet.RemoveAt(0);
            packet.RemoveAt(0);
            packet.RemoveAt(0);
            packet.RemoveAt(0);

            return result;
        }

        public static string ReadString(IList<byte> packet)
        {
            var size = BitConverter.ToInt16(packet.ToArray(), 0);
            var str = System.Text.Encoding.ASCII.GetString(packet.ToArray(), 2, size);

            packet.RemoveRange(0, size + 2);

            return str;
        }

        private static void RemoveRange<T>(this IList<T> iList, int start, int size)
        {
            for (int j = start; j < size + start; j++)
            {
                iList.RemoveAt(j);
            }
        }
    }
}