using System;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using Seanuts.Framework.Math;

namespace Platformer
{
    public class Block
    {
        public Vector2 Position { get; set; }
        public float HP { get; set; }

        public Block(float x, float y)
        {
            Position = new Vector2(x, y);
            HP = 100;

            if (new Random().Next(0, 100) < 20)
            {
                HP = 50;
            }
            if (new Random().Next(0, 100) < 5)
            {
                HP = 25;
            }
        }

        public void Update(Player player)
        {
            var rect1 = new Rectangle(Position.X, Position.Y, 32, 32);
            var rect2 = new Rectangle(player.Position.X, player.Position.Y, 32, 32);
            var damage = 5.0f;

            if (player.HasUpgrade("time"))
            {
                damage -= player.GetUpgrade("time");
            }

            if (damage < 1)
            {
                damage = 1;
            }

            if (rect1.Intersect(rect2))
            {
                if (player.Position.Y + 8 < Position.Y && player.Velocity > 0)
                {
                    HP -= damage;

                    player.Velocity = 0;
                    player.Position.Y = Position.Y - 32;
                }
            }
        }

        public void Render()
        {
            if (HP >= 75)
            {
                Draw.Box(Position.X, Position.Y, 32, 32, 0, 0, 0, 0, 1f, 0f, 1.0f, PolygonFillMode.Filled);
            }
            if (HP < 75 && HP > 25)
            {
                Draw.Box(Position.X, Position.Y, 32, 32, 0, 0, 0, 0f, 0f, 1f, 1.0f, PolygonFillMode.Filled);
            }
            if (HP <= 25)
            {
                Draw.Box(Position.X, Position.Y, 32, 32, 0, 0, 0, 1f, 0f, 0f, 1.0f, PolygonFillMode.Filled);
            }
        }
    }
}