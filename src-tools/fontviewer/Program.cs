using System;
using System.Linq;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Graphics;

namespace FontViewer
{
    class Program
    {
        static SNWindow Window { get; set; }
        static SNFontData Data { get; set; }
        static SNImage Image { get; set; }

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
            Data = new SNFontData(filename);

            Window = new SNWindow(Data.Bitmap.Width, Data.Bitmap.Height, Data.Font.FontFamily.Name);
            Window.OnLoad += Window_Onload;
            Window.OnUpdate += Window_OnUpdate;
            Window.OnRender += Window_OnRender;
            Window.Open(false, true, false);
        }

        static void Window_Onload()
        {
            Image = new SNImage(Data.Bitmap);
        }
        static void Window_OnUpdate()
        {

        }
        static void Window_OnRender()
        {
            SNDraw.Image(Image, 0, 0, Image.Width, Image.Height, 0, 0, 0, 0, 0, Image.Width, Image.Height, 1, 1);

            foreach (var rect in Data.Bounds)
            {
                SNDraw.Box(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2, 0, 0, 0, 255, 0, 255, 255, SNPolygonFillMode.Lines);
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
            Console.WriteLine("filename  The path to the .snfnt file");
        }

        static bool RequestsHelp(string[] args)
        {
            return args.Contains("-h") || args.Contains("--help") || args.Length == 0;
        }
    }
}
