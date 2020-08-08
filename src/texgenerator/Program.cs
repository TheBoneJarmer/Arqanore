using System;
using System.IO;

namespace TexGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && (args[0] == "-h" || args[0] == "--help"))
            {
                DisplayHelp();
                Environment.Exit(0);
            }
            if (args.Length == 0)
            {
                DisplayHelp();
                Environment.Exit(0);
            }

            Run(args);
        }

        static void Run(string[] args)
        {
            var filename = args[0];

            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"Unable to find image {filename}");
            }

            GenerateTexture(filename);
        }

        static void GenerateTexture(string path)
        {
            var extension = path.Substring(path.LastIndexOf("."));
            var filename = path.Substring(path.Replace("\\", "/").LastIndexOf("/") + 1).Replace(extension, "");

            // For now it will do if the file extension is changed
            File.Copy(path, $"{filename}.arqtex", true);
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Usage: tex-generator <FILENAME>");
            Console.WriteLine();
            Console.WriteLine("ABOUT");
            Console.WriteLine("This tool generates an a texture file from an image");
        }
    }
}
