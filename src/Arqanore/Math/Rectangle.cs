using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Arqanore.Math
{
    public class Rectangle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Rectangle()
        {

        }
        public Rectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool Intersect(Rectangle other)
        {
            return X + Width > other.X && X < other.X + other.Width && Y + Height > other.Y && Y < other.Y + other.Height;
        }
        public bool Contains(Vector2 other)
        {
            return other.X >= X && other.Y >= Y && other.X < X + Width && other.Y < Y + Height;
        }
    }
}
