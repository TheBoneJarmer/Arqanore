using System;
using Arqanore;
using Arqanore.Graphics;
using Arqanore.Input;
using Arqanore.Math;

namespace Arqanore.Examples.Pong
{
    public class Game
    {
        private Window window;
        private Player player1;
        private Player player2;
        private Ball ball;
        private int score1;
        private int score2;
        private Font font;

        public Game()
        {
            window = new Window(800, 600, "Pong");
            window.OnLoad += Window_OnLoad;
            window.OnTick += Window_OnTick;
            window.OnUpdate += Window_OnUpdate;
            window.OnRender += Window_OnRender;
            window.OnResize += Window_OnResize;
        }

        public void Start()
        {
            window.Open();
        }
        public void Reset()
        {
            score1 = 0;
            score2 = 0;

            player1 = new Player(1, Color.BLUE, new Vector2(64, window.Height / 2), false, window.Height / 5);
            player2 = new Player(2, Color.RED, new Vector2(window.Width - 80, window.Height / 2), true, window.Height / 5);
            ball = new Ball(window.Width / 2, window.Height / 2);
        }

        private void Window_OnLoad()
        {
            font = new Font("assets/arial.arqfnt");

            Reset();
        }

        private void Window_OnTick(double deltaTime)
        {
            if (score1 == 10 || score2 == 10)
            {
                return;
            }

            player1.Tick(deltaTime, window, ball);
            player2.Tick(deltaTime, window, ball);
            ball.Tick(deltaTime, window);
        }

        private void Window_OnUpdate()
        {
            if (score1 == 10 || score2 == 10)
            {
                if (Keyboard.KeyPressed(KeyCode.SPACE))
                {
                    Reset();
                }

                return;
            }

            player1.Update(ball);
            player2.Update(ball);
            ball.Update();

            if (ball.Position.X < 0)
            {
                ball.Reset();
                score2++;
            }
            if (ball.Position.X > window.Width)
            {
                ball.Reset();
                score1++;
            }
        }

        private void Window_OnRender()
        {
            if (score1 == 10 || score2 == 10)
            {
                string text1 = null;
                string text2 = "Press space to play again";

                if (score1 == 10) text1 = "Victory!";
                if (score2 == 10) text1 = "Game Over";
                
                float width1 = font.MeasureText(text1) * 2;
                float text1X = (window.Width / 2) - (width1 / 2);
                float text1Y = (window.Height / 2) - font.BaseHeight;

                float width2 = font.MeasureText(text2);
                float text2X = (window.Width / 2) - (width2 / 2);
                float text2Y = text1Y + 64;

                Draw.Text(font, text1, text1X, text1Y, 255, 255, 255, 255, 1, 1, 2, 2);
                Draw.Text(font, text2, text2X, text2Y, 255, 255, 255, 255, 1, 1, 1, 1);

                return;
            }

            player1.Render();
            player2.Render();
            ball.Render();

            Draw.Text(font, score1.ToString(), 16, 16, 255, 255, 255, 255, 1, 1, 1, 1);
            Draw.Text(font, score2.ToString(), window.Width - 48, 16, 255, 255, 255, 255, 1, 1, 1, 1);
        }

        private void Window_OnResize(int width, int height)
        {
            player1.Height = height / 5;
            player2.Height = height / 5;
            player2.Position.X = width - 80;   
            
            ball.Origin = new Vector2(width / 2, height / 2);
        }
    }
}