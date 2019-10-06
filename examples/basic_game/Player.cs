using System;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using Seanuts.Framework.Math;

namespace example2
{
    public class Player
    {
        public Vector2 Position { get; private set; }
        public int HP { get; private set; }

        public Player()
        {
            this.Position = new Vector2(400, 300);
            this.HP = 100;
        }

        public void Update()
        {
            var speed = (float)(Time.DeltaTime / 10.0);

            if (Keyboard.KeyDown(KeyCode.LEFT))
            {
                this.Position.X -= speed;
            }
            if (Keyboard.KeyDown(KeyCode.RIGHT))
            {
                this.Position.X += speed;
            }
            if (Keyboard.KeyDown(KeyCode.UP))
            {
                this.Position.Y -= speed;
            }
            if (Keyboard.KeyDown(KeyCode.DOWN))
            {
                this.Position.Y += speed;
            }
        }

        public void Render()
        {
            Draw.Box(Position.X, Position.Y, 32, 32, 0, -16, -16, 0, 0, 1.0f,1.0f, PolygonFillMode.Filled);        
        }
    }
}