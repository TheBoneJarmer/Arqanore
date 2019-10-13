using System;
using System.Drawing;
using System.Linq;
using Seanuts.Framework;

namespace FontGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (RequestHelp(args))
            {
                DisplayHelp();
                Environment.Exit(0);
            }

            Run(args);
        }

        static void Run(string[] args)
        {
            var filename = "";
            var fontFamily = "";
            var fontSize = 0f;
            var r = 0;
            var g = 0;
            var b = 0;
            var a = 0;

            // Parse the arguments
            ParseArgs(args, out filename, out fontFamily, out fontSize, out r, out g, out b, out a);

            // Generate the fontdata and save it
            var fontData = new SNFontData(fontFamily, fontSize, r, g, b, a);
            fontData.Save(filename);
        }

        static void ParseArgs(string[] args, out string filename, out string fontFamily, out float fontSize, out int r, out int g, out int b, out int a)
        {
            filename = "";
            fontFamily = "";
            fontSize = 16f;
            r = 255;
            g = 255;
            b = 255;
            a = 255;

            // First argument have to be font family name
            fontFamily = args[0];
            filename = args[0] + ".snfnt";

            // Every other argument is optional
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if (arg == "-o")
                {
                    filename = args[i + 1];
                    i++;
                }
                if (arg == "-s")
                {
                    fontSize = float.Parse(args[i + 1]);
                    i++;
                }
                if (arg == "-r")
                {
                    r = int.Parse(args[i + 1]);
                    i++;
                }
                if (arg == "-g")
                {
                    g = int.Parse(args[i + 1]);
                    i++;
                }
                if (arg == "-b")
                {
                    b = int.Parse(args[i + 1]);
                    i++;
                }
                if (arg == "-a")
                {
                    a = int.Parse(args[i + 1]);
                    i++;
                }
            }
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Usage: font-gen <family> [options]");
            Console.WriteLine();
            Console.WriteLine("ABOUT");
            Console.WriteLine("This font generator can be used to generate bitmap fonts for the Seanuts framework");
            Console.WriteLine();
            Console.WriteLine("ARGUMENTS");
            Console.WriteLine("family    The font family name. This font must be installed on your system.");
            Console.WriteLine();
            Console.WriteLine("OPTIONS");
            Console.WriteLine("-o        The output filename. Must end with the .snfnt extension.");
            Console.WriteLine("-s        The font size in floating point or integer notation.");
            Console.WriteLine("-r        The red component of the color. Default value is 255.");
            Console.WriteLine("-g        The green component of the color. Default value is 255.");
            Console.WriteLine("-b        The blue component of the color. Default value is 255.");
            Console.WriteLine("-a        The alpha component of the color. Default value is 255.");
            Console.WriteLine();
            Console.WriteLine("EXAMPLES");
            Console.WriteLine("fontgen \"Arial \" -o \"Default.snfnt\" -s 12 -g 0");
        }

        static bool RequestHelp(string[] args)
        {
            return args.Contains("-h") || args.Contains("--help") || args.Length == 0;
        }
    }
}
