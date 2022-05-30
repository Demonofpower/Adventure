using Adv.Server.Game.Processing;

namespace Adv.Server.Game.Model.Objects
{
    interface IObject
    {
        public PacketManager PacketManager => Controller.PacketManager;

        public ulong TicksAlive { get; set; }
        public bool IsFaded { get; set; }

        public void Tick();
    }
}
