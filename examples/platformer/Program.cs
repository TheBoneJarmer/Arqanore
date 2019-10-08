using System;
using Seanuts.Framework;

namespace Platformer
{
    class Program
    {
        static GameWindow Window { get; set; }
        
        static void Main(string[] args)
        {
            Window = new GameWindow(800, 600, "Platformer");
            Window.Open();
        }
    }
}
