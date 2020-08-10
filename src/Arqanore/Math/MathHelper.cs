using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Arqanore.Math
{
    public static class MathHelper
    {
        public static float ToRadians(double deg)
        {
            return (float)(deg * System.Math.PI / 180.0f);
        }
        public static float ToDegrees(double rad)
        {
            return (float)(rad / System.Math.PI * 180.0f);
        }
        public static float MoveX(float deg)
        {
            return (float)System.Math.Cos(ToRadians(deg));
        }
        public static float MoveY(float deg)
        {
            return (float)System.Math.Sin(ToRadians(deg));
        }

        public static float Lerp(float f1, float f2, float by)
        {
            return f1 * (1 - by) + f2 * by;
        }
    }
}
