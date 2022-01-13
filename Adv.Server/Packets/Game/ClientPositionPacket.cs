using System.Collections.Generic;
using System.Numerics;
using Adv.Server.Util;

namespace Adv.Server.Packets.Game
{
    class ClientPositionPacket : GamePacket
    {
        public Vector3 Position { get; set; }
        public Rotation Rotation { get; set; }
        public byte Forward { get; set; }
        public byte Strafe { get; set; }

        public float RealForward => Forward / 127.0f;
        public float RealStrafe => Forward / 127.0f;

        public ClientPositionPacket(List<byte> rawData) : base(rawData)
        {
        }
    }
}
