using System;
using Arqanore.Graphics;
using Arqanore.Input;
using Arqanore.Math;

namespace Arqanore.Examples.Pong
{
    public class Player
    {
        public int Id { get; private set; }
        public Color Color { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public bool Computer { get; private set; }
        public int Height { get; set; }

        public Player(int id, Color color, Vector2 position, bool computer, int height)
        {
            this.Id = id;
            this.Color = color;
            this.Position = position;
            this.Computer = computer;
            this.Velocity = new Vector2();
            this.Height = height;
        }

        public void Tick(double delta, Window window, Ball ball)
        {
            if (Computer)
            {
                TickComputer(delta);
            }
            else
            {
                TickHuman(delta);
            }

            Position += Velocity * (float)delta * 300;

            if (Position.Y < Height / 2) Position.Y = Height / 2;
            if (Position.Y > window.Height - (Height / 2)) Position.Y = window.Height - (Height / 2);
        }

        private void TickComputer(double delta)
        {

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
            if (ball.Position.Y > Position.Y - (Height / 2) && ball.Position.Y < Position.Y + (Height / 2) && ball.Position.X < Position.X + 8 && ball.Position.X > Position.X - 8)
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
            Draw.Box(Position.X, Position.Y, 16, Height, 0, -8, -(Height / 2), Color.R, Color.G, Color.B, Color.A, PolygonMode.Filled);
        }
    }
}