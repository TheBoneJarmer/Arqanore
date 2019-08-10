using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.WebSockets
{
    public static class Extensions
    {
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
    }
}
