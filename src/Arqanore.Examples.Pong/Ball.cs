using System;
using Arqanore.Math;

namespace Arqanore.Examples.Pong
{
    public class Ball
    {
        private Vector2 origin;
        private int timer;

        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }

        public Ball(float x, float y)
        {
            origin = new Vector2(x, y);

            Reset();
        }

        public void Tick(double delta, Window window)
        {
            float speed = (float)delta * 300;

            if (timer < 10)
            {
                return;
            }

            Position += Velocity * speed;
            
            if (Position.Y < 16)
            {
                Bounce(true);
                Position.Y = 20;
            }
            if (Position.Y > window.Height - 16)
            {
                Bounce(true);
                Position.Y = window.Height - 20;
            }
        }
        public void Update()
        {
            timer++;
        }
        public void Render()
        {
            Draw.Box(Position.X, Position.Y, 16, 16, 0, -8, -8, 255, 255, 255, 255, PolygonFillMode.Filled);
        }

        public void Bounce(bool horizontal)
        {
            if (horizontal)
            {
                Velocity.Y *= -1;
            }
            else
            {
                Velocity.X *= -1;
            }
        }
        public void Reset()
        {
            Random rand = new Random();

            Velocity = new Vector2(1, 1);

            if (rand.Next(0,100) > 50) Velocity.X = -1;
            if (rand.Next(0, 100) > 50) Velocity.Y = -1;

            Position = new Vector2(origin.X, origin.Y);

            timer = 0;
        }
    }
}