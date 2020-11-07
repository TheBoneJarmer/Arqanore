using Arqanore.Graphics;
using Arqanore.Input;
using System;

namespace Arqanore.Examples.Example1
{
    class Program
    {
        private static Font font;
        private static Texture grass;
        private static Texture player;
        private static Window window;
        private static float playerX;
        private static float playerY;
        private static float frameTime;
        private static float frameHor;
        private static float frameVert;

        static void Main(string[] args)
        {
            window = new Window(1366, 768, "Example 1");
            window.OnRender += Window_OnRender;
            window.OnLoad += Window_OnLoad;
            window.OnTick += Window_OnTick;
            window.Open();
        }

        private static void Window_OnTick(double deltaTime)
        {
            float speed = (float)deltaTime * 3;

            if (Keyboard.KeyDown(KeyCode.LEFT))
            {
                playerX -= speed;
                frameTime += speed;
                frameVert = 3;
            }
            if (Keyboard.KeyDown(KeyCode.RIGHT))
            {
                playerX += speed;
                frameTime += speed;
                frameVert = 2;
            }
            if (Keyboard.KeyDown(KeyCode.UP))
            {
                playerY -= speed;
                frameTime += speed;
                frameVert = 1;
            }
            if (Keyboard.KeyDown(KeyCode.DOWN))
            {
                playerY += speed;
                frameTime += speed;
                frameVert = 0;
            }

            if (frameTime > (16f / speed / 1000f))
            {
                frameTime = 0;
                frameHor++;
            }

            if (frameHor > 3)
            {
                frameHor = 0;
            }
        }

        private static void Window_OnLoad()
        {
            font = new Font("assets/arial.arqfnt");
            grass = new Texture("assets/grass.arqtex");
            player = new Texture("assets/player.arqtex");
        }

        private static void Window_OnRender()
        {
            float scale = 3;
            float tilesHor = window.Width / scale / 16;
            float tilesVert = window.Height / scale / 16;

            Draw.Text(font, "Hello World", 32, 32, 255, 255, 255, 255);

            // Render a floor
            for (int x = 0; x < tilesHor; x++)
            {
                for (int y = 0; y < tilesVert; y++)
                {
                    //Draw.Texture(grass, x * 16 * scale, y * 16 * scale, 16 * scale, 16 * scale, 0, 0, 0, 0, 0, 16, 16, 255, 255, 255, 255);
                }
            }

            // Render the player
            Draw.Texture(player, playerX * 16 * scale, playerY * 16 * scale, 16 * scale, 16 * scale, 0, 0, 0, frameHor * 16, frameVert * 16, 16, 16, 255, 255, 255, 255);

            // Render text
            Draw.Text(font, "Hello World", 32, 32, 255, 255, 255, 255);
        }
    }
}
