using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Arqanore.TexGenerator
{
    class Program
    {
        static int Main(string[] args)
        {
            int exitCode = 0;

            if (args.Length == 1 && (args[0] == "-h" || args[0] == "--help"))
            {
                DisplayHelp();
                return 0;
            }
            if (args.Length == 1 && (args[0] == "-v" || args[0] == "--version"))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);

                Console.WriteLine(info.ProductVersion);
                return 0;
            }
            if (args.Length == 0)
            {
                DisplayHelp();
                return 0;
            }

            try
            {
                Run(args);
            }
            catch (TexGeneratorException ex)
            {
                Console.Error.WriteLine(ex.Message);
                exitCode = 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                ex = ex.InnerException;

                while (ex != null)
                {
                    Console.WriteLine("------------------------------------");
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);

                    ex = ex.InnerException;
                }

                exitCode = 1;
            }

            return exitCode;
        }

        static void Run(string[] args)
        {
            OperatingSystem os = Environment.OSVersion;
            string filename = null;
            string outputFolder = null;

            try
            {
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
                        outputFolder = args[i+1];
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                throw new TexGeneratorException("Invalid arguments provided");
            }

            if (filename == null)
            {
                throw new TexGeneratorException("Missing required arguments");
            }
            if (!File.Exists(filename))
            {
                throw new TexGeneratorException($"Unable to find image {filename}");
            }
            if (outputFolder != null && !Directory.Exists(outputFolder))
            {
                throw new TexGeneratorException($"Output folder {outputFolder} not found");
            }

            if (outputFolder == null)
            {
                outputFolder = Directory.GetCurrentDirectory();
            }
            if (!outputFolder.EndsWith("/"))
            {
                outputFolder += "/";
            }

            if (os.Platform == PlatformID.Win32NT && !Regex.IsMatch(filename, @"[A-Z]\:.*"))
            {
                throw new TexGeneratorException("Filename is not absolute");
            }
            if (os.Platform == PlatformID.Unix && !Regex.IsMatch(filename, @"/.*"))
            {
                throw new TexGeneratorException("Filename is not absolute");
            }

            GenerateTexture(filename, outputFolder);
        }

        static void GenerateTexture(string path, string outputFolder)
        {
            string extension = path.Substring(path.LastIndexOf("."));
            string filename = path.Substring(path.Replace("\\", "/").LastIndexOf("/") + 1).Replace(extension, "");

            Console.WriteLine($"Generating arqanore texture {filename}.arqtex from image {path}");

            // For now it will do if the file extension is changed
            File.Copy(path, $"{outputFolder}/{filename}.arqtex", true);
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Usage: arqanore-texgenerator -f <FILENAME>");
            Console.WriteLine();
            Console.WriteLine("ABOUT");
            Console.WriteLine("This tool generates an Arqanore texture file from a png, jpg, or bmp image file");
            Console.WriteLine();
            Console.WriteLine("ARGUMENTS");
            Console.WriteLine("-f    Absolute path to the image file");
            Console.WriteLine("-o    [optional] Output folder of the generated texture file. If not provided the working folder will be used.");
        }
    }
}
