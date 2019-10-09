using System;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;

namespace Platformer
{
    class Program
    {
        static GameWindow Window { get; set; }
        static Player Player { get; set; }
        
        static void Main(string[] args)
        {
            Window = new GameWindow(800, 600, "Platformer");
            Window.OnLoad += Window_OnLoad;
            Window.OnUpdate += Window_OnUpdate;
            Window.OnRender += Window_OnRender;
            Window.Open();
        }

        static void Window_OnLoad()
        {
            Player = new Player(Window.Width / 2f, Window.Height - 32f);
        }
        static void Window_OnUpdate()
        {
            Player.Update();
        }
        static void Window_OnRender()
        {
            Player.Render();
        }
    }
}
