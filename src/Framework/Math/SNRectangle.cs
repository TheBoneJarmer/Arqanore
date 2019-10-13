using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Framework.Math
{
    public class SNRectangle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public SNRectangle()
        {

        }
        public SNRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool Intersect(SNRectangle other)
        {
            return (X + Width > other.X && X < other.X + other.Width && Y + Height > other.Y && Y < other.Y + other.Height);
        }
    }
}
