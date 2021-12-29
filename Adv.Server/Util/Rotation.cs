namespace Adv.Server.Util
{
    public struct Rotation
    {
        public short Yaw { get; set; }
        public short Pitch { get; set; }
        public short Roll { get; set; }

        public Rotation(short yaw, short pitch, short roll)
        {
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
        }
    }
}
