using System;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using Seanuts.Framework.Math;

namespace BasicGame
{
    public class Player
    {
        public SNVector2 Position { get; private set; }
        public int HP { get; private set; }

        public Player()
        {
            this.Position = new SNVector2(400, 300);
            this.HP = 100;
        }

        public void Update()
        {
            var speed = (float)SNTime.DeltaTime * 100f;
            var speedX = 0f;
            var speedY = 0f;

            if (SNKeyboard.KeyDown(SNKeyCode.LEFT))
            {
                speedX = -speed;
            }
            else if (SNKeyboard.KeyDown(SNKeyCode.RIGHT))
            {
                speedX = speed;
            }
            else
            {
                speedX = 0;
            }

            if (SNKeyboard.KeyDown(SNKeyCode.UP))
            {
                speedY = -speed;
            }
            else if (SNKeyboard.KeyDown(SNKeyCode.DOWN))
            {
                speedY = speed;
            }
            else
            {
                speedY = 0;
            }

            this.Position.X += speedX;
            this.Position.Y += speedY;
        }

        public void Render()
        {
            SNDraw.Box(Position.X, Position.Y, 32, 32, 0, -16, -16, 0, 0, 255,255, SNPolygonFillMode.Filled);        
        }
    }
}