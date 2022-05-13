using System.Numerics;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Model
{
    class Actor
    {
        public int Id { get; set; }
        public ActorType ActorType { get; set; }
        public Vector3 Position { get; set; }
        public Rotation Rotation { get; set; }

        public Actor(int id, ActorType actorType, Vector3 position, Rotation rotation)
        {
            Id = id;
            ActorType = actorType;
            Position = position;
            Rotation = rotation;
        }
    }
}
