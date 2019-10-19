using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Graphics
{
    public class Color
    {
        public static readonly Color RED = new Color(255, 0, 0);
        public static readonly Color GREEN = new Color(0, 255, 0);
        public static readonly Color BLUE = new Color(0, 0, 255);
        public static readonly Color PURPLE = new Color(255, 0, 255);
        public static readonly Color CYAN = new Color(0, 255, 255);
        public static readonly Color YELLOW = new Color(255, 255, 0);
        public static readonly Color WHITE = new Color(255, 255, 255);
        public static readonly Color BLACK = new Color(0, 0, 0);

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public Color(int r, int g, int b) : this(r, g, b, 1)
        {

        }
        public Color(int r, int g, int b, int a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
    }
}
