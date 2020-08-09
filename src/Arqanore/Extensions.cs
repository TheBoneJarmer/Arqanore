using Arqanore.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore
{
    public static class Extensions
    {
        public static string ToBinaryString(this byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }
        public static int[] ToBinaryIntArray(this byte b)
        {
            var binaryCharArray = ToBinaryString(b);
            var output = new int[8];

            for (int i = 0; i < 8; i++)
            {
                output[i] = int.Parse(binaryCharArray[i].ToString());
            }

            return output;
        }
        public static string ToBinaryString(this int i)
        {
            return Convert.ToString(i, 2);
        }
        public static int BinaryToInt(this string s)
        {
            int output = 0;
            int x = 1;

            for (int i = s.Length - 1; i > -1; i--)
            {
                int value = (int)s[i];

                if (value > 1)
                {
                    throw new InvalidOperationException($"Invalid binary string {s}");
                }

                if (value == 1)
                {
                    output += x;
                }

                x *= 2;
            }

            return output;
        }
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

        public static float[] ToFloatArray(this Vector2 v)
        {
            return new float[2] { v.X, v.Y };
        }
        public static float[] ToFloatArray(this Vector2[] array)
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

        public static float Width(this Vector2[] array)
        {
            float biggestNegative = 0;
            float biggestPositive = 0;

            foreach (var v in array)
            {
                if (v.X > 0 && v.X > biggestPositive) biggestPositive = v.X;
                if (v.X < 0 && v.X < biggestNegative) biggestNegative = v.X;
            }

            return MathHelper.DistanceBetweenVectors(biggestNegative, 0, biggestPositive, 0);
        }
        public static float Height(this Vector2[] array)
        {
            float biggestNegative = 0;
            float biggestPositive = 0;

            foreach (var v in array)
            {
                if (v.Y > 0 && v.Y > biggestPositive) biggestPositive = v.Y;
                if (v.Y < 0 && v.Y < biggestNegative) biggestNegative = v.Y;
            }

            return MathHelper.DistanceBetweenVectors(biggestNegative, 0, biggestPositive, 0);
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
        
        public static void Add(this List<Vector2> list, float x, float y)
        {
            list.Add(new Vector2(x, y));
        }
    }
}
