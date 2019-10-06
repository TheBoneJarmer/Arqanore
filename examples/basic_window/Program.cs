using System;
using Seanuts.Framework;

namespace example1
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new GameWindow(800, 600, "Basic Window");
            window.Open(false);
        }
    }
}
