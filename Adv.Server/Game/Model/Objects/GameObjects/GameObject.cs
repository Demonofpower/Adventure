using System.Numerics;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Model.Objects.GameObjects
{
    abstract class GameObject : IObject
    {
        public ulong TicksAlive { get; set; }
        public bool IsFaded { get; set; }

        public int actorId;
        public ActorType actorType;
        public Vector3 position;
        public Rotation rotation;

        protected GameObject(ActorType actorType, Vector3 position, Rotation rotation)
        {
            this.TicksAlive = 0;
            this.IsFaded = false;

            this.actorId = IdHelper.RequestUniqueId();
            this.actorType = actorType;
            this.position = position;
            this.rotation = rotation;
        }

        public virtual void Tick()
        {
            TicksAlive += 1;
        }
    }
}
