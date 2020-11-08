using System;
using Arqanore.Graphics;
using Arqanore.Input;
using Arqanore.Math;

namespace Arqanore.Examples.Pong
{
    public class Player
    {
        private Polygon polygon;
        private int height;

        public int Id { get; private set; }
        public Color Color { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public bool Computer { get; private set; }

        public Player(int id, Color color, Vector2 position, bool computer, int height)
        {
            this.Id = id;
            this.Color = color;
            this.Position = position;
            this.Computer = computer;
            this.Velocity = new Vector2();
            
            this.height = height;
            this.polygon = Polygon.Box(new Vector2(16, height), new Vector2(-8, -height / 2));
        }

        public void Tick(double delta, Window window)
        {
            if (Computer)
            {
                
            }
            else
            {
                TickHuman(delta);
            }

            Position += Velocity * (float)delta * 300;

            if (Position.Y < height / 2) Position.Y = height / 2;
            if (Position.Y > window.Height - (height / 2)) Position.Y = window.Height - (height / 2);
        }

        private void TickHuman(double delta)
        {
            if (Keyboard.KeyDown(KeyCode.DOWN))
            {
                Velocity.Y = 1;
            }
            if (Keyboard.KeyDown(KeyCode.UP))
            {
                Velocity.Y = -1;
            }
        }

        public void Update(Ball ball)
        {
            if (ball.Position.Y > Position.Y - (height / 2) && ball.Position.Y < Position.Y + (height / 2) && ball.Position.X < Position.X + 8 && ball.Position.X > Position.X - 8)
            {
                if (ball.Velocity.X > 0)
                {
                    ball.Position.X = Position.X - 10;
                }
                else
                {
                    ball.Position.X = Position.X + 10;
                }

                ball.Bounce(false);
            }

            if (Computer)
            {
                if (ball.Position.Y > Position.Y)
                {
                    Velocity.Y = 1;
                }
                else
                {
                    Velocity.Y = -1;
                }
            }
        }

        public void Render()
        {
            polygon.Render(Position, 0, Color);
        }
    }
}