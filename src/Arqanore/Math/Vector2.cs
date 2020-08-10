using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Math
{
    public class Vector2
    {
        public static Vector2 ZERO = new Vector2(0, 0);

        public float X { get; set; }
        public float Y { get; set; }

        public Vector2()
        {
            X = 0;
            Y = 0;
        }
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(X*X + Y*Y);
        }
        public void Move(float angle, float speed)
        {
            X += MathHelper.MoveX(angle) * speed;
            Y += MathHelper.MoveY(angle) * speed;
        }

        public static float Angle(Vector2 v1, Vector2 v2)
        {
            double theta = System.Math.Atan2((v2.Y - v1.Y), (v2.X - v1.X));

            if (theta < 0)
            {
                theta += 2 * System.Math.PI;
            }

            return MathHelper.ToDegrees(theta);
        }
        public static float Distance(Vector2 v1, Vector2 v2)
        {
            float x = v1.X - v2.X;
            float y = v1.Y - v2.Y;

            return (float)System.Math.Sqrt((x * x) + (y * y));
        }
        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }
        public static Vector2 Normalize(Vector2 v)
        {
            return new Vector2(v.X / v.Length(), v.Y / v.Length());
        }
        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float by)
        {
            float l1 = MathHelper.Lerp(v1.X, v2.X, by);
            float l2 = MathHelper.Lerp(v1.Y, v2.Y, by);

            return new Vector2(l1, l2);
        }
        public static Vector2 Reflect(Vector2 v1, Vector2 v2)
        {
            Vector2 normal = Vector2.Normalize(v2);
            float dot = Vector2.Dot(v1, normal);

            return v1 - (2 * dot * normal);
        }

        public static Vector2 operator +(Vector2 v, float f)
        {
            return new Vector2(v.X + f, v.Y + f);
        }
        public static Vector2 operator +(float f, Vector2 v)
        {
            return new Vector2(v.X + f, v.Y + f);
        }
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static Vector2 operator -(Vector2 v, float f)
        {
            return new Vector2(v.X - f, v.Y - f);
        }
        public static Vector2 operator -(float f, Vector2 v)
        {
            return new Vector2(v.X - f, v.Y - f);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static Vector2 operator *(Vector2 v, float f)
        {
            return new Vector2(v.X * f, v.Y * f);
        }
        public static Vector2 operator *(float f, Vector2 v)
        {
            return new Vector2(v.X * f, v.Y * f);
        }
        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }
        public static Vector2 operator /(Vector2 v, float f)
        {
            return new Vector2(v.X / f, v.Y / f);
        }
        public static Vector2 operator /(float f, Vector2 v)
        {
            return new Vector2(v.X / f, v.Y / f);
        }
        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
        }
    }
}