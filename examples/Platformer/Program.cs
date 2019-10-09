using System;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Math;

namespace Platformer
{
    class Program
    {
        static GameWindow Window { get; set; }
        static Player Player { get; set; }
        static Block[] Blocks { get; set; }
        
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
            Window.ClearColor = Color.WHITE;
            Player = new Player(Window.Width / 2f, Window.Height);

            // Generate a level
            Blocks = new Block[100];

            for (var i=0; i<Blocks.Length; i++)
            {
                var x = (float)Math.Floor(new Random().Next(0, Window.Width) / 32.0f) * 32.0f;
                var y = (float)Math.Floor(new Random().Next(0, Window.Height) / 32.0f) * 32.0f;

                Blocks[i] = new Block(x, y);
            }
        }
        static void Window_OnUpdate()
        {
            Player.Update();

            for (var i=0; i<Blocks.Length; i++)
            {
                if (Player.Collides(Blocks[i]))
                {
                    if (Player.Position.Y < Blocks[i].Position.Y && Player.Velocity > 0)
                    {
                        Player.Velocity = 0;
                        Player.Position.Y = Blocks[i].Position.Y - 32;
                    }
                }
            }
        }
        static void Window_OnRender()
        {
            Player.Render();

            for (var i=0; i<Blocks.Length; i++)
            {
                Blocks[i].Render();
            }
        }
    }
}
