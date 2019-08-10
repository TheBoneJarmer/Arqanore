using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Framework.Graphics
{
    public class Color
    {
        public static readonly Color RED = new Color(1, 0, 0);
        public static readonly Color GREEN = new Color(0, 1, 0);
        public static readonly Color BLUE = new Color(0, 0, 1);
        public static readonly Color PURPLE = new Color(1, 0, 1);
        public static readonly Color CYAN = new Color(0, 1, 1);
        public static readonly Color YELLOW = new Color(1, 1, 0);
        public static readonly Color WHITE = new Color(1, 1, 1);
        public static readonly Color BLACK = new Color(0, 0, 0);

        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public Color(float r, float g, float b) : this(r, g, b, 1)
        {

        }
        public Color(float r, float g, float b, float a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
    }
}
