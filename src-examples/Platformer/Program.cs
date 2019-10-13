using System;
using System.Linq;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using Seanuts.Framework.Math;

namespace Platformer
{
    class Program
    {
        static GameWindow Window { get; set; }
        static Player Player { get; set; }
        static Block[] Blocks { get; set; }
        static Item[] Items { get; set; }
        
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
            Player = new Player(Window.Width / 2f, (int)Math.Floor(Window.Height / 100.0 * 75.0));

            // Generate a level
            Blocks = new Block[40];
            Items = new Item[5];

            for (var i=0; i<Items.Length; i++)
            {
                var x = (float)Math.Floor(new Random().Next(0, Window.Width) / 32.0f) * 32.0f;
                var y = (float)Math.Floor(new Random().Next(0, Window.Height) / 32.0f) * 32.0f;

                Items[i] = new Item(x, y);
            }

            for (var i=0; i<Blocks.Length; i++)
            {
                var x = (float)Math.Floor(new Random().Next(0, Window.Width) / 32.0f) * 32.0f;
                var y = (float)Math.Floor(new Random().Next(0, Window.Height - 32) / 32.0f) * 32.0f;

                while (Items.Any(item => item.Position.X == x && item.Position.Y == y))
                {
                    x = (float)Math.Floor(new Random().Next(0, Window.Width) / 32.0f) * 32.0f;
                    y = (float)Math.Floor(new Random().Next(0, Window.Height - 32) / 32.0f) * 32.0f;
                }

                Blocks[i] = new Block(x, y);
            }
        }
        static void Window_OnUpdate()
        {
            Player.Update();

            for (var i=0; i<Blocks.Length; i++)
            {
                if (Blocks[i] == null)
                {
                    continue;
                }

                if (Blocks[i].HP <= 0)
                {
                    Blocks[i] = null;
                }
                else
                {
                    Blocks[i].Update(Player);
                }
            }

            for (var i=0; i<Items.Length; i++)
            {
                if (Items[i] == null)
                {
                    continue;
                }

                if (Items[i].Type == -1)
                {
                    Items[i] = null;
                }
                else
                {
                    Items[i].Update(Player);
                }
            }

            if (Keyboard.KeyDown(KeyCode.ESCAPE))
            {
                Window.Close();
            }
        }
        static void Window_OnRender()
        {
            Player.Render();

            for (var i=0; i<Blocks.Length; i++)
            {
                if (Blocks[i] == null)
                {
                    continue;
                }

                Blocks[i].Render();
            }

            for (var i=0; i<Items.Length; i++)
            {
                if (Items[i] == null)
                {
                    continue;
                }

                Items[i].Render();
            }
        }
    }
}
