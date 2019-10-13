using System;
using System.Collections.Generic;
using System.Linq;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using Seanuts.Framework.Math;

namespace Platformer
{
    public class Player
    {
        public SNVector2 Position { get; set; }
        public float Velocity { get; set; }
        public float Friction { get; set; }

        public int HP { get; set; }
        private Dictionary<string, float> Upgrades { get; set; }

        public Player(float x, float y)
        {
            Upgrades = new Dictionary<string, float>();
            Position = new SNVector2(x, y);
            Velocity = 0;
            Friction = 0;
            HP = 100;
        }

        public void Update()
        {
            Move();
            Jump();
            Physics();
        }

        private void Move()
        {
            if (SNKeyboard.KeyDown(SNKeyCode.LEFT))
            {
                this.Friction -= (float)SNTime.DeltaTime * 10.0f;
            }
            else if (SNKeyboard.KeyDown(SNKeyCode.RIGHT))
            {
                this.Friction += (float)SNTime.DeltaTime * 10.0f;
            }
            else
            {
                if (this.Friction > (float)SNTime.DeltaTime * 10.0f)
                {
                    this.Friction -= (float)SNTime.DeltaTime * 2.5f;
                }
                else if (this.Friction < -(float)SNTime.DeltaTime * 10.0f)
                {
                    this.Friction += (float)SNTime.DeltaTime * 2.5f;
                }
                else
                {
                    this.Friction = 0;
                }
            }
        }
        public void Jump()
        {
            float speed = 500.0f;

            if (Upgrades.Any(x => x.Key == "jump"))
            {
                speed += Upgrades["jump"];
            }

            if (SNKeyboard.KeyDown(SNKeyCode.UP) && this.Velocity == 0)
            {
                this.Velocity = -(float)SNTime.DeltaTime * speed;
            }
        }
        public void Physics()
        {
            if (this.Friction > SNTime.DeltaTime * 200)
            {
                this.Friction = (float)SNTime.DeltaTime * 200;
            }
            if (this.Friction < -SNTime.DeltaTime * 200)
            {
                this.Friction = -(float)SNTime.DeltaTime * 200;
            }

            this.Position.X += Friction;
            this.Position.Y += this.Velocity;
            this.Velocity += (float)SNTime.DeltaTime * 25.0f;

            if (this.Position.Y >= 600 - 32)
            {
                this.Velocity = 0;
                this.Position.Y = 600 - 32;
            }
        }

        public void AddUpgrade(string name, float value)
        {
            if (Upgrades.ContainsKey(name))
            {
                Upgrades[name] += value;
            }
            else
            {
                Upgrades.Add(name, value);
            }
        }
        public bool HasUpgrade(string name)
        {
            return Upgrades.ContainsKey(name);
        }
        public float GetUpgrade(string name)
        {
            if (Upgrades.ContainsKey(name))
            {
                return 0;
            }
            else
            {
                return Upgrades[name];
            }
        }

        public void Render()
        {
            SNDraw.Box(Position.X, Position.Y, 32, 32, 0, 0, 0, 0, 0, 255, 255, SNPolygonFillMode.Filled);
        }
    }
}