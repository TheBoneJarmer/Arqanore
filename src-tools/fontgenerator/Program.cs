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
            var fontFamily = "";
            var fontSize = 0f;
            var r = 0;
            var g = 0;
            var b = 0;
            var a = 0;

            if (RequestHelp(args))
            {
                DisplayHelp();
                return;
            }

            ParseArgs(args, out fontFamily, out fontSize, out r, out g, out b, out a);
            Run(fontFamily, fontSize, r, g, b, a);
        }

        static void ParseArgs(string[] args, out string fontFamily, out float fontSize, out int r, out int g, out int b, out int a)
        {
            fontFamily = "";
            fontSize = 0f;
            r = 0;
            g = 0;
            b = 0;
            a = 0;
            
            // Check the args
            if (args.Length != 2 && args.Length != 6)
            {
                Console.Error.WriteLine("Too few parameters. Please look at the help for more info.");
                Environment.Exit(1);
            }

            // Parse the arguments
            if (args.Length > 1) {
            	fontFamily = args[0];

		        if (!float.TryParse(args[1], out fontSize))
		        {
		            Console.Error.WriteLine($"Font size value {args[1]} is not a valid floating point decimal");
		            Environment.Exit(2);
		        }
            }
            
            if (args.Length == 2)
            {
            	r = 255;
            	g = 255;
            	b = 255;
            	a = 255;
            }
            
            if (args.Length == 6)
            {
            	if (!int.TryParse(args[2], out r))
		        {
		            Console.Error.WriteLine($"Red component value {args[2]} is not a valid integer decimal");
		            Environment.Exit(2);
		        }
		        if (!int.TryParse(args[3], out g))
		        {
		            Console.Error.WriteLine($"Red component value {args[3]} is not a valid integer decimal");
		            Environment.Exit(2);
		        }
		        if (!int.TryParse(args[4], out b))
		        {
		            Console.Error.WriteLine($"Red component value {args[4]} is not a valid integer decimal");
		            Environment.Exit(2);
		        }
		        if (!int.TryParse(args[5], out a))
		        {
		            Console.Error.WriteLine($"Red component value {args[5]} is not a valid integer decimal");
		            Environment.Exit(2);
		        }
            }
        }
        static void Run(string fontFamily, float fontSize, int r, int g, int b, int a)
        {
            var fontData = new FontData(fontFamily, fontSize, r, g, b, a);
            fontData.Save();
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Usage: font-gen <family> <size> (<r> <g> <b> <a>)");
            Console.WriteLine();
            Console.WriteLine("ABOUT");
            Console.WriteLine("This font generator can be used to generate bitmap fonts for the Seanuts framework");
            Console.WriteLine();
            Console.WriteLine("ARGUMENTS");
            Console.WriteLine("family    The font family name. This font must be installed on your system.");
            Console.WriteLine("size      The font size in floating point or integer notation.");
            Console.WriteLine("r         (Optional) The red component of the color. Default value is 255.");
            Console.WriteLine("g         (Optional) The green component of the color. Default value is 255.");
            Console.WriteLine("b         (Optional) The blue component of the color. Default value is 255.");
            Console.WriteLine("a         (Optional) The alpha component of the color. Default value is 255.");
        }

        static bool RequestHelp(string[] args)
        {
            return args.Contains("-h") || args.Contains("--help") || args.Length == 0;
        }
    }
}
