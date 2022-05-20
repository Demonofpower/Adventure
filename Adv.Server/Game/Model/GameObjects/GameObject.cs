using System.Numerics;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Model.GameObjects
{
    abstract class GameObject
    {
        private ulong ticksAlive;

        private int actorId;
        private ActorType actorType;
        private Vector3 position;
        private Rotation rotation;

        protected GameObject(ActorType actorType, Vector3 position, Rotation rotation)
        {
            this.ticksAlive = 0;

            this.actorId = IdHelper.RequestUniqueId();
            this.actorType = actorType;
            this.position = position;
            this.rotation = rotation;
        }

        public virtual void Tick()
        {
            ticksAlive += 1;
        }
    }
}
