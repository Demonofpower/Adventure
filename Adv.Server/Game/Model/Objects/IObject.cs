namespace Adv.Server.Game.Model.Objects
{
    interface IObject
    {
        public ulong TicksAlive { get; set; }
        public bool IsFaded { get; set; }

        public void Tick();
    }
}
