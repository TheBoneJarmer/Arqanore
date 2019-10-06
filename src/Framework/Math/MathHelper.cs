using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Framework.Math
{
    public static class MathHelper
    {
        public static float ToRadians(double deg)
        {
            return (float)(deg * (System.Math.PI / 180.0f));
        }
        public static float ToDegrees(double rad)
        {
            return (float)((rad / System.Math.PI) * 180.0f);
        }
        public static float RadiansBetweenVectors(float x1, float y1, float x2, float y2)
        {
            float theta = (float)System.Math.Atan2((y2 - y1), (x2 - x1));

            if (theta < 0)
            {
                theta += 2 * (float)System.Math.PI;
            }

            return theta;
        }
        public static float RadiansBetweenVectors(Vector2 v1, Vector2 v2)
        {
            return RadiansBetweenVectors(v1.X, v1.Y, v2.X, v2.Y);
        }
        public static float DegreesBetweenVectors(float x1, float y1, float x2, float y2)
        {
            return ToDegrees(RadiansBetweenVectors(x1, y1, x2, y2));
        }
        public static float DegreesBetweenVectors(Vector2 v1, Vector2 v2)
        {
            return ToDegrees(RadiansBetweenVectors(v1, v2));
        }
        public static float DistanceBetweenVectors(float x1, float y1, float x2, float y2)
        {
            var x = x1 - x2;
            var y = y1 - y2;

            return (float)System.Math.Sqrt((x * x) + (y * y));
        }
        public static float DistanceBetweenVectors(Vector2 v1, Vector2 v2)
        {
            return DistanceBetweenVectors(v1.X, v1.Y, v2.X, v2.Y);
        }
        public static float XSpeed(int deg)
        {
            return (float)System.Math.Cos(ToRadians(deg));
        }
        public static float YSpeed(int deg)
        {
            return (float)System.Math.Sin(ToRadians(deg));
        }
    }
}
