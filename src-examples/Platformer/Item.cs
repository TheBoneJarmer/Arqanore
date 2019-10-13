using System;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using Seanuts.Framework.Math;

namespace Platformer
{
    public class Item
    {
        public int Type { get; set; }
        public SNVector2 Position { get; set; }

        public Item(float x, float y)
        {
            Position = new SNVector2(x, y);
            Type = 0;

            if (new Random().Next(0, 100) > 50)
            {
                Type = 1;
            }
        }
        
        public void Update(Player player)
        {
            var rect1 = new SNRectangle(Position.X, Position.Y, 32, 32);
            var rect2 = new SNRectangle(player.Position.X, player.Position.Y, 32, 32);

            if (rect1.Intersect(rect2))
            {
                // Jump boost
                if (Type == 0)
                {
                    player.AddUpgrade("jump", 50.0f);
                }
                if (Type == 1)
                {
                    player.AddUpgrade("time", 1.0f);
                }

                Type = -1;
            }
        }

        public void Render()
        {
            if (Type == 0)
            {
                SNDraw.Box(Position.X, Position.Y, 16, 16, 45, 8, 8, 255, 0, 255, 255, SNPolygonFillMode.Filled);
            }
            if (Type == 1)
            {
                SNDraw.Box(Position.X, Position.Y, 16, 16, 45, 8, 8, 0, 255, 255, 255, SNPolygonFillMode.Filled);
            }
        }
    }
}
