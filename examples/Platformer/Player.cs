using System;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using Seanuts.Framework.Math;

namespace Platformer
{
    public class Player
    {
        public Vector2 Position { get; set; }
        public float Velocity { get; set; }
        public float Friction { get; set; }

        public int HP { get; set; }

        public Player(float x, float y)
        {
            Position = new Vector2(x, y);
            Velocity = 0;
            Friction = 0;
            HP = 100;
        }

        public void Update()
        {
            if (Keyboard.KeyDown(KeyCode.LEFT))
            {
                this.Friction -= (float)Time.DeltaTime * 10.0f;
            }
            else if (Keyboard.KeyDown(KeyCode.RIGHT))
            {
                this.Friction += (float)Time.DeltaTime * 10.0f;
            }
            else
            {
                if (this.Friction > (float)Time.DeltaTime * 10.0f)
                {
                    this.Friction -= (float)Time.DeltaTime * 2.5f;
                }
                else if (this.Friction < -(float)Time.DeltaTime * 10.0f)
                {
                    this.Friction += (float)Time.DeltaTime * 2.5f;
                }
                else
                {
                    this.Friction = 0;
                }
            }

            if (Keyboard.KeyDown(KeyCode.UP) && this.Velocity == 0)
            {
                this.Velocity = -(float)Time.DeltaTime * 500.0f;
            }

            if (this.Friction > Time.DeltaTime * 200)
            {
                this.Friction = (float)Time.DeltaTime * 200;
            }
            if (this.Friction < -Time.DeltaTime * 200)
            {
                this.Friction = -(float)Time.DeltaTime * 200;
            }

            this.Position.X += Friction;
            this.Position.Y += this.Velocity;
            this.Velocity += (float)Time.DeltaTime * 25.0f;

            if (this.Position.Y >= 600)
            {
                this.Velocity = 0;
                this.Position.Y = 600;
            }
        }
        public void Render()
        {
            Draw.Box(Position.X, Position.Y, 32, 32, 0, 0, -32, 0, 0, 1, 1, PolygonFillMode.Filled);
        }

        public bool Collides(Block other)
        {
            var rect1 = new Rectangle(Position.X, Position.Y - 32, 32, 32);
            var rect2 = new Rectangle(other.Position.X, other.Position.Y - 32, 32, 32);

            return rect1.Intersect(rect2);
        }
    }
}