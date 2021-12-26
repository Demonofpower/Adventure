using System;
using System.Collections.Generic;
using System.Linq;

namespace Adv.Server.Packets
{
    public static class PacketProcessor
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

        private static void AddRange(this IList<byte> iList, IList<byte> toAdd)
        {
            foreach (var b in toAdd)
            {
                iList.Add(b);
            }
        }

        private static void RemoveRange<T>(this IList<T> iList, int start, int size)
        {
            for (int j = start; j < size + start; j++)
            {
                iList.RemoveAt(j);
            }
        }

        public static void Write8(this IList<byte> packet, byte val)
        {
            packet.Add(val);
        }
        public static void Write8(this IList<byte> packet, bool val)
        {
            packet.Add(Convert.ToByte(val));
        }

        public static void Write16(this IList<byte> packet, short val)
        {
            packet.AddRange(BitConverter.GetBytes(val));
        }

        public static void Write32(this IList<byte> packet, int val)
        {
            packet.AddRange(BitConverter.GetBytes(val));
        }

        public static void WriteString(this IList<byte> packet, string val)
        {
            packet.Add((byte) val.Length);
            packet.Add(0x0);

            System.Text.Encoding.ASCII.GetBytes(val);
        }
    }
}