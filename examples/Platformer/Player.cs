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
                this.Friction -= (float)Time.DeltaTime / 10.0f;
            }
            else if (Keyboard.KeyDown(KeyCode.RIGHT))
            {
                this.Friction += (float)Time.DeltaTime / 10.0f;
            }
            else
            {
                if (this.Friction > (float)Time.DeltaTime / 100.0f)
                {
                    this.Friction -= (float)Time.DeltaTime / 100.0f;
                }
                else if (this.Friction < (float)Time.DeltaTime / 100.0f)
                {
                    this.Friction += (float)Time.DeltaTime / 100.0f;
                }
                else
                {
                    this.Friction = 0;
                }
            }

            if (Keyboard.KeyDown(KeyCode.UP))
            {
                this.Velocity = -(float)Time.DeltaTime * 1000.0f;
            }

            if (this.Friction > Time.DeltaTime * 100)
            {
                this.Friction = (float)Time.DeltaTime * 100;
            }
            if (this.Friction < -Time.DeltaTime * 100)
            {
                this.Friction = -(float)Time.DeltaTime * 100;
            }

            if (this.Velocity < 0)
            {
                this.Velocity += (float)Time.DeltaTime * 100.0f;
            }
            else
            {
                this.Velocity = 0;
            }

            this.Position.X += Friction;
            this.Position.Y += this.Velocity;
        }
        public void Render()
        {
            Draw.Box(Position.X, Position.Y, 32, 32, 0, 0, 0, 0, 0, 1, 1, PolygonFillMode.Filled);
        }
    }
}