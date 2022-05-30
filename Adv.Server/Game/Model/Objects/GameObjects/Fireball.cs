using System.Numerics;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Model.Objects.GameObjects
{
    class Fireball : GameObject
    {
        public Fireball(Vector3 position, Rotation rotation) : base(ActorType.Fireball, position, rotation)
        {
            
        }

        public override void Tick()
        {
            base.Tick();

            position = position.AddToCoords(50);

            if (TicksAlive > 20)
            {
                IsFaded = true;
            }
        }
    }
}
