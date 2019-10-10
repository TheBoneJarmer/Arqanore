using System;
using System.Net.Sockets;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Sockets;

namespace Pong
{
    class Program
    {
        static GameWindow Window { get; set; }
        static Player1 Player1 { get; set; }
        static Player2 Player2 { get; set; }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+");
            Console.WriteLine("~             <++>             ~");
            Console.WriteLine("~           <>PONG<>           ~");
            Console.WriteLine("~             <++>             ~");
            Console.WriteLine("~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+");
            Console.WriteLine();

            if (args.Length == 0)
            {
                
            }
            else if (args.Length == 1)
            {
                
            }
            else
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            Start();
        }

        static void Start()
        {
            Window = new GameWindow(800, 600, "Pong");
            Window.OnLoad += Window_OnLoad;
            Window.OnUpdate += Window_OnUpdate;
            Window.OnRender += Window_OnRender;
            Window.OnClose += Window_OnClose;
            Window.Open();
        }

        static void Window_OnLoad()
        {

        }
        static void Window_OnUpdate()
        {
            
        }
        static void Window_OnRender()
        {

        }
        static void Window_OnClose()
        {
            
        }
    }
}
