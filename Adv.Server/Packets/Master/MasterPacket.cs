namespace Adv.Server.Packets.Master
{
    class MasterPacket : Packet
    {
        public byte SmallId =>  (byte) Id;
    }
}
