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

        public Block(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public void Render()
        {
            Draw.Box(Position.X, Position.Y, 32, 32, 0, 0, -32, 0, 0.75f, 0.1f, 1.0f, PolygonFillMode.Filled);
        }
    }
}