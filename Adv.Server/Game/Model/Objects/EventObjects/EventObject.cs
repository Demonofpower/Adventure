namespace Adv.Server.Game.Model.Objects.EventObjects
{
    class EventObject : IObject
    {
        public ulong TicksAlive { get; set; }
        public bool IsFaded { get; set; }

        public virtual void Tick()
        {
            TicksAlive += 1;
        }
    }
}
