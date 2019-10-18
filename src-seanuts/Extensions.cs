using Seanuts.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts
{
    public static class SNExtensions
    {
        public static byte ToByte(this string str)
        {
            return byte.Parse(str);
        }
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
        public static int ToInt(this float f)
        {
            return (int)f;
        }
        public static float ToFloat(this string str)
        {
            return float.Parse(str);
        }
        public static float ToFloat(this int i)
        {
            return (float)i;
        }
        public static double ToDouble(this string str)
        {
            return double.Parse(str);
        }
        public static double ToDouble(this int i)
        {
            return (double)i;
        }

        public static float[] ToFloatArray(this SNVector2 v)
        {
            return new float[2] { v.X, v.Y };
        }
        public static float[] ToFloatArray(this SNVector2[] array)
        {
            List<float> result = new List<float>();

            foreach (var v in array)
            {
                result.Add(v.X);
                result.Add(v.Y);
            }

            return result.ToArray();
        }
        public static float[] ToFloatArray(this string[] array)
        {
            List<float> result = new List<float>();

            foreach (var v in array)
            {
                result.Add(float.Parse(v));
            }

            return result.ToArray();
        }

        public static float Width(this SNVector2[] array)
        {
            float biggestNegative = 0;
            float biggestPositive = 0;

            foreach (var v in array)
            {
                if (v.X > 0 && v.X > biggestPositive) biggestPositive = v.X;
                if (v.X < 0 && v.X < biggestNegative) biggestNegative = v.X;
            }

            return SNMath.DistanceBetweenVectors(biggestNegative, 0, biggestPositive, 0);
        }
        public static float Height(this SNVector2[] array)
        {
            float biggestNegative = 0;
            float biggestPositive = 0;

            foreach (var v in array)
            {
                if (v.Y > 0 && v.Y > biggestPositive) biggestPositive = v.Y;
                if (v.Y < 0 && v.Y < biggestNegative) biggestNegative = v.Y;
            }

            return SNMath.DistanceBetweenVectors(biggestNegative, 0, biggestPositive, 0);
        }

        public static T[] Push<T>(this T[] source, T[] dest)
        {
            var result = new List<T>();
            result.AddRange(source);
            result.AddRange(dest);

            return result.ToArray();
        }
        public static T[] Slice<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);

            return result;
        }
        
        public static void Add(this List<SNVector2> list, float x, float y)
        {
            list.Add(new SNVector2(x, y));
        }
    }
}
