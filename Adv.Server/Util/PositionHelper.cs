using System.Numerics;

namespace Adv.Server.Util
{
    public static class PositionHelper
    {
        public static Vector3 ChangeCoords(this Vector3 vec, float? x = null, float? y = null, float? z = null)
        {
            if (x != null)
            {
                vec.X = (float) x;
            }

            if (y != null)
            {
                vec.Y = (float) y;
            }

            if (z != null)
            {
                vec.Z = (float) z;
            }

            return vec;
        }

        public static Vector3 AddToCoords(this Vector3 vec, float? x = null, float? y = null, float? z = null)
        {
            if (x != null)
            {
                vec.X += (float)x;
            }

            if (y != null)
            {
                vec.Y += (float)y;
            }

            if (z != null)
            {
                vec.Z += (float)z;
            }

            return vec;
        }
    }
}