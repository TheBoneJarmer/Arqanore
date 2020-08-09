using System;
using System.IO;

namespace Arqanore.TexGenerator
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
            string filename = null;
            string output = null;

            for (int i=0; i<args.Length; i++)
            {
                string arg = args[i];

                if (arg == "-f")
                {
                    filename = args[i+1];
                    i++;
                }
                if (arg == "-o")
                {
                    output = args[i+1];
                    i++;
                }
            }

            if (filename == null || output == null)
            {
                Console.Error.WriteLine("Missing required parameters");
                Environment.Exit(1);
            }
            if (!File.Exists(filename))
            {
                Console.Error.WriteLine($"Unable to find image {filename}");
                Environment.Exit(1);
            }
            if (!Directory.Exists(output))
            {
                Console.Error.WriteLine($"Directory {output} not found");
                Environment.Exit(1);
            }

            GenerateTexture(filename, output);
        }

        static void GenerateTexture(string path, string output)
        {
            string extension = path.Substring(path.LastIndexOf("."));
            string filename = path.Substring(path.Replace("\\", "/").LastIndexOf("/") + 1).Replace(extension, "");

            // For now it will do if the file extension is changed
            File.Copy(path, $"{output}/{filename}.arqtex", true);
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Usage: arqanore-texgenerator -f <FILENAME> -o <OUTPUT_FOLDER>");
            Console.WriteLine();
            Console.WriteLine("ABOUT");
            Console.WriteLine("This tool generates an a texture file from an image");
        }
    }
}
