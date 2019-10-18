using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Graphics
{
    public class SNColor
    {
        public static readonly SNColor RED = new SNColor(255, 0, 0);
        public static readonly SNColor GREEN = new SNColor(0, 255, 0);
        public static readonly SNColor BLUE = new SNColor(0, 0, 255);
        public static readonly SNColor PURPLE = new SNColor(255, 0, 255);
        public static readonly SNColor CYAN = new SNColor(0, 255, 255);
        public static readonly SNColor YELLOW = new SNColor(255, 255, 0);
        public static readonly SNColor WHITE = new SNColor(255, 255, 255);
        public static readonly SNColor BLACK = new SNColor(0, 0, 0);

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public SNColor(int r, int g, int b) : this(r, g, b, 1)
        {

        }
        public SNColor(int r, int g, int b, int a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
    }
}
