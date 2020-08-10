using Arqanore.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore
{
    public static class Extensions
    {
        public static void Add(this List<Vector2> list, float x, float y)
        {
            list.Add(new Vector2(x, y));
        }
    }
}
