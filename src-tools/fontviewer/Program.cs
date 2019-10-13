using System;
using System.Linq;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;

namespace FontViewer
{
    class Program
    {
        static GameWindow Window { get; set; }
        static FontData Data { get; set; }
        static Background Background { get; set; }

        static void Main(string[] args)
        {
            if (RequestsHelp(args))
            {
                DisplayHelp();
                return;
            }

            Run(args[0]);
        }

        static void Run(string filename)
        {
            Data = new FontData(filename);

            Window = new GameWindow(Data.Bitmap.Width, Data.Bitmap.Height, Data.Font.FontFamily.Name);
            Window.OnLoad += Window_Onload;
            Window.OnUpdate += Window_OnUpdate;
            Window.OnRender += Window_OnRender;
            Window.Open(false, true, false);
        }

        static void Window_Onload()
        {
            Background = new Background(Data.Bitmap);
        }
        static void Window_OnUpdate()
        {

        }
        static void Window_OnRender()
        {
            Draw.Background(Background, 0, 0, Background.Width, Background.Height, 1, 1, 0);

            foreach (var rect in Data.Bounds)
            {
                Draw.Box(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2, 0, 0, 0, 255, 0, 255, 255, PolygonFillMode.Lines);
            }
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Usage: font-viewer <filename>");
            Console.WriteLine();
            Console.WriteLine("ABOUT");
            Console.WriteLine("Displays seafont information");
            Console.WriteLine();
            Console.WriteLine("ARGUMENTS");
            Console.WriteLine("filename  The path to the .seafnt file");
        }

        static bool RequestsHelp(string[] args)
        {
            return args.Contains("-h") || args.Contains("--help") || args.Length == 0;
        }
    }
}
