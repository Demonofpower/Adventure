using System;
using System.Collections.Generic;
using System.Linq;

namespace Adv.Server.Packets
{
    public static class PacketProcessor
    {
        public static byte Read8(ref byte[] packet)
        {
            var result = packet[0];
            packet = packet.Skip(1).ToArray();

            return result;
        }

        public static short Read16(ref byte[] packet)
        {
            var result = BitConverter.ToInt16(packet.ToArray(), 0);
            packet = packet.Skip(2).ToArray();

            return result;
        }

        public static int Read32(ref byte[] packet)
        {
            var result = BitConverter.ToInt32(packet.ToArray(), 0);
            packet = packet.Skip(4).ToArray();

            return result;
        }

        public static string ReadString(ref byte[] packet)
        {
            var size = BitConverter.ToInt16(packet.ToArray(), 0);
            var str = System.Text.Encoding.ASCII.GetString(packet.ToArray(), 2, size);
            packet = packet.Skip(size + 2).ToArray();

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

            if (val.Length == 0) return;

            packet.AddRange(System.Text.Encoding.ASCII.GetBytes(val));
        }
    }
}