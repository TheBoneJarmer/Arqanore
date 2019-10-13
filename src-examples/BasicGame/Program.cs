using System;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;

namespace BasicGame
{
    class Program
    {
        static SNWindow Window { get; set; }
        static Player Player { get; set; }

        static void Main(string[] args)
        {
            try
            {
                Window = new SNWindow(800, 600, "My Game");
                Window.OnLoad += Window_OnLoad;
                Window.OnUpdate += Window_OnUpdate;
                Window.OnRender += Window_OnRender;
                Window.OnRefresh += Window_OnRefresh;

                Window.Open(false);
            }
            catch (GLFWException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        static void Window_OnLoad()
        {
            // Render a white background
            Window.ClearColor = SNColor.WHITE;

            // Setup the player
            Player = new Player();
        }
        static void Window_OnUpdate()
        {
            if (SNKeyboard.KeyDown(SNKeyCode.ESCAPE))
            {
                Window.Close();
                return;
            }

            Player.Update();
        }
        static void Window_OnRender()
        {
            Player.Render();
        }
        static void Window_OnRefresh()
        {
            Console.Write("R");
        }
    }
}
