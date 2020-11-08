using Arqanore.Graphics;
using Arqanore.Input;
using Arqanore.Math;
using System;

namespace Arqanore.Examples.Example1
{
    class Program
    {
        private static Window window;

        private static Font font;
        private static Texture texGrass;
        private static Texture texPlayer;
        private static Sprite sprPlayer;
        private static Sprite sprGrass;
        private static Polygon box;

        private static int scale;
        private static Vector2 playerPos;
        private static int frameHor;
        private static int frameVert;
        private static float frameTime;

        static void Main(string[] args)
        {
            window = new Window(1600, 900, "Example 1");
            window.OnRender += Window_OnRender;
            window.OnLoad += Window_OnLoad;
            window.OnTick += Window_OnTick;
            window.Open();
        }

        private static void Window_OnLoad()
        {
            scale = 3;
            font = new Font("assets/arial.arqfnt");
            texGrass = new Texture("assets/grass.arqtex");
            texPlayer = new Texture("assets/player.arqtex");
            sprGrass = new Sprite(texGrass, new Vector2(0, 0), new Vector2(scale, scale));
            sprPlayer = new Sprite(texPlayer, 4, 4, new Vector2(0, 0), new Vector2(scale, scale));
            box = Polygon.Box(new Vector2(64, 64), new Vector2(0, 0));
            playerPos = new Vector2(5, 5);
        }

        private static void Window_OnTick(double deltaTime)
        {
            float speed = (float)deltaTime * 3;

            if (Keyboard.KeyDown(KeyCode.LEFT))
            {
                playerPos.X -= speed;
                frameTime += speed;
                frameVert = 3;
            }
            if (Keyboard.KeyDown(KeyCode.RIGHT))
            {
                playerPos.X += speed;
                frameTime += speed;
                frameVert = 2;
            }
            if (Keyboard.KeyDown(KeyCode.UP))
            {
                playerPos.Y -= speed;
                frameTime += speed;
                frameVert = 1;
            }
            if (Keyboard.KeyDown(KeyCode.DOWN))
            {
                playerPos.Y += speed;
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

        private static void Window_OnRender()
        {
            float scale = 3;
            float tilesHor = (float)System.Math.Ceiling(window.Width / scale / 16);
            float tilesVert = (float)System.Math.Ceiling(window.Height / scale / 16);

            window.Title = $"Tiles Hor: {tilesHor} | Tiles Vert: {tilesVert} | Total: {tilesHor * tilesVert}";

            // Render a floor
            for (int x = 0; x < tilesHor; x++)
            {
                for (int y = 0; y < tilesVert; y++)
                {
                    sprGrass.Render(new Vector2(x * 16 * scale, y * 16 * scale));
                }
            }

            // Render the player
            sprPlayer.Render(playerPos * 16 * scale, frameHor, frameVert);

            // Render all polygons
            box.Render(new Vector2(32, 32), 0, Color.RED);

            // Render text
            //Draw.Text(font, "Hello World", 32, 32, 255, 255, 255, 255);
        }
    }
}
