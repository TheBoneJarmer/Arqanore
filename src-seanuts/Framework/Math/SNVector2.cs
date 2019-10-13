using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Framework.Math
{
    public class SNVector2
    {
        public static SNVector2 ZERO = new SNVector2(0, 0);

        public float X { get; set; }
        public float Y { get; set; }

        public SNVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static SNVector2 operator +(SNVector2 v, float f)
        {
            return new SNVector2(v.X + f, v.Y + f);
        }
        public static SNVector2 operator +(SNVector2 v1, SNVector2 v2)
        {
            return new SNVector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static SNVector2 operator -(SNVector2 v, float f)
        {
            return new SNVector2(v.X - f, v.Y - f);
        }
        public static SNVector2 operator -(SNVector2 v1, SNVector2 v2)
        {
            return new SNVector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static SNVector2 operator *(SNVector2 v, float f)
        {
            return new SNVector2(v.X * f, v.Y * f);
        }
        public static SNVector2 operator *(SNVector2 v1, SNVector2 v2)
        {
            return new SNVector2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static SNVector2 operator /(SNVector2 v, float f)
        {
            return new SNVector2(v.X / f, v.Y / f);
        }
        public static SNVector2 operator /(SNVector2 v1, SNVector2 v2)
        {
            return new SNVector2(v1.X / v2.X, v1.Y / v2.Y);
        }
    }
}
