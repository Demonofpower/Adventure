using System.Numerics;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Model.GameObjects
{
    abstract class GameObject
    {
        public ulong ticksAlive;
        public bool isFaded;
        
        public int actorId;
        public ActorType actorType;
        public Vector3 position;
        public Rotation rotation;

        protected GameObject(ActorType actorType, Vector3 position, Rotation rotation)
        {
            this.ticksAlive = 0;
            this.isFaded = false;

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
