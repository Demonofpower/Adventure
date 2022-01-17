using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Adv.Server.Util;

namespace Adv.Server.Packets
{
    public static class PacketProcessor
    {
        public static void SwitchPacketIdEndian(ref Span<byte> packet)
        {
            if (packet.Length < 2) return;

            var save = packet[0];

            packet[0] = packet[1];
            packet[1] = save;
        }

        public static byte Read8(ref Span<byte> packet)
        {
            var result = packet[0];
            packet = packet.Slice(1);

            return result;
        }

        public static short Read16(ref Span<byte> packet)
        {
            var result = BitConverter.ToInt16(packet);
            packet = packet.Slice(2);

            return result;
        }

        public static int Read32(ref Span<byte> packet)
        {
            var result = BitConverter.ToInt32(packet);
            packet = packet.Slice(4);

            return result;
        }

        public static float ReadFloat(ref Span<byte> packet)
        {
            var result = BitConverter.ToSingle(packet);
            packet = packet.Slice(4);

            return result;
        }

        public static string ReadString(ref Span<byte> packet)
        {
            var size = BitConverter.ToInt16(packet);
            var str = System.Text.Encoding.ASCII.GetString(packet.Slice(2, size));
            packet = packet.Slice(size + 2);

            return str;
        }

        public static Vector3 ReadVector3(ref Span<byte> packet)
        {
            var x = ReadFloat(ref packet);
            var y = ReadFloat(ref packet);
            var z = ReadFloat(ref packet);

            return new Vector3(x, y, z);
        }

        public static Rotation ReadRotation(ref Span<byte> packet)
        {
            var x = Read16(ref packet);
            var y = Read16(ref packet);
            var z = Read16(ref packet);

            return new Rotation(x, y, z);
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

        public static void WriteFloat(this IList<byte> packet, float val)
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

        public static void WriteVector3(this IList<byte> packet, Vector3 val)
        {
            packet.AddRange(BitConverter.GetBytes(val.X));
            packet.AddRange(BitConverter.GetBytes(val.Y));
            packet.AddRange(BitConverter.GetBytes(val.Z));
        }

        public static void WriteRotation(this IList<byte> packet, Rotation val)
        {
            packet.AddRange(BitConverter.GetBytes(val.Yaw));
            packet.AddRange(BitConverter.GetBytes(val.Pitch));
            packet.AddRange(BitConverter.GetBytes(val.Roll));
        }
    }
}