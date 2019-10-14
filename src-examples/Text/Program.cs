using System;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;

namespace Text
{
    class Program
    {
        static SNWindow Window { get; set; }
        static SNFont Font { get; set; }

        static void Main(string[] args)
        {
            Window = new SNWindow(800, 600, "Fonts");
            Window.OnLoad += Window_OnLoad;
            Window.OnRender += Window_OnRender;
            Window.Open(false, true, true);
        }

        static void Window_OnLoad()
        {
            Font = new SNFont("Arial.snfnt");
        }
        static void Window_OnRender()
        {
            SNDraw.Text(Font, "Hello World", 32, 32);
        }
    }
}
