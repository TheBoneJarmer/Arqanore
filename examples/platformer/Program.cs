using System;
using Seanuts.Framework;

namespace example3
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
