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
        public Vector2 Position { get; set; }

        public Item(float x, float y)
        {
            Position = new Vector2(x, y);
            Type = 0;

            if (new Random().Next(0, 100) > 50)
            {
                Type = 1;
            }
        }
        
        public void Update(Player player)
        {
            var rect1 = new Rectangle(Position.X, Position.Y, 32, 32);
            var rect2 = new Rectangle(player.Position.X, player.Position.Y, 32, 32);

            if (rect1.Intersect(rect2))
            {
                // Jump boost
                if (Type == 0)
                {
                    player.AddUpgrade("jump", 25);
                }
                if (Type == 1)
                {
                    player.AddUpgrade("time", 0.5f);
                }

                Type = -1;
            }
        }

        public void Render()
        {
            if (Type == 0)
            {
                Draw.Box(Position.X, Position.Y, 16, 16, 45, 8, 8, 1, 0, 1, 1, PolygonFillMode.Filled);
            }
            if (Type == 1)
            {
                Draw.Box(Position.X, Position.Y, 16, 16, 45, 8, 8, 0, 1, 1, 1, PolygonFillMode.Filled);
            }
        }
    }
}