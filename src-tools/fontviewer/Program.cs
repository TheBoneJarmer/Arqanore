using System;
using System.Linq;
using Seanuts;
using Seanuts.Framework;

namespace FontViewer
{
    class Program
    {
        static GameWindow Window { get; set; }
        static FontData Data { get; set; }

        static void Main(string[] args)
        {
            if (RequestsHelp(args))
            {
                DisplayHelp();
                return;
            }


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

        }
        static void Window_OnUpdate()
        {

        }
        static void Window_OnRender()
        {
            
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
