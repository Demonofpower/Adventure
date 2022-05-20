using System;
using System.Numerics;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Model.GameObjects
{
    class Fireball : GameObject
    {
        public Fireball(Vector3 position, Rotation rotation) : base(ActorType.Fireball, position, rotation)
        {
            
        }

        public override void Tick()
        {
            base.Tick();
        }
    }
}
