using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Adv.Sniffer
{
    public class HexArithmetic
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        public static float HexToFloat(string s)
        {
            uint num = uint.Parse(s, NumberStyles.AllowHexSpecifier);

            byte[] floatVals = BitConverter.GetBytes(num);
            float f = BitConverter.ToSingle(floatVals.Reverse().ToArray(), 0);

            return f;
        }

        public static string FloatToHex(float f)
        {
            var bytes = BitConverter.GetBytes(f);

            return ByteArrayToString(bytes);
        }
    }
}