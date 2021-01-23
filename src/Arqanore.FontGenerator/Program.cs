using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Arqanore.FontGenerator
{
    class Program
    {
        static string tempFolder;

        static int Main(string[] args)
        {
            tempFolder = ".temp" + Guid.NewGuid().ToString().Replace("-", "");
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
                Console.WriteLine($"Creating temp folder {tempFolder}");
                Directory.CreateDirectory(tempFolder);

                // Run the generator
                Run(args);
            }
            catch (FontGeneratorException ex)
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
            finally
            {
                Console.WriteLine("Cleaning up temp folder");

                // Delete the temp folder
                if (Directory.Exists(tempFolder))
                {
                    Directory.Delete(tempFolder, true);
                }
            }

            return exitCode;
        }

        static void Run(string[] args)
        {
            string fontFile = null;
            int fontSize = 0;
            string outputFolder = null;
            OperatingSystem os = Environment.OSVersion;

            // Fetch data from the args
            try
            {
                for (var i = 0; i < args.Length; i++)
                {
                    var arg = args[i];

                    if (arg == "-f")
                    {
                        fontFile = args[i + 1];
                        i++;
                    }
                    if (arg == "-s")
                    {
                        fontSize = int.Parse(args[i + 1]);
                        i++;
                    }
                    if(arg == "-o")
                    {
                        outputFolder = args[i + 1];
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                throw new FontGeneratorException("Invalid arguments provided");
            }

            // Check if the user provided all info
            if (fontFile == null || fontSize == 0)
            {
                throw new FontGeneratorException("Missing required arguments");
            }

            // Validate the input
            if (!File.Exists(fontFile))
            {
                throw new FontGeneratorException($"Unable to find file {fontFile}");
            }
            if (fontSize < 4)
            {
                throw new FontGeneratorException("Font size must be minimum 4 points");
            }
            
            if (os.Platform == PlatformID.Win32NT && !Regex.IsMatch(fontFile, @"[A-Z]\:.*"))
            {
                throw new FontGeneratorException("Filename is not absolute");
            }
            if (os.Platform == PlatformID.Unix && !Regex.IsMatch(fontFile, @"/.*"))
            {
                throw new FontGeneratorException("Filename is not absolute");
            }

            ValidateRequirements();
            Generate(fontFile, fontSize, outputFolder);
        }

        static void ValidateRequirements()
        {
            Console.WriteLine("Validating requirements");

            // Just run fontbm to check if it runs at all
            // It will exit with a non-zero code because no arguments were provided but that is ok
            // If it does not run, the reason why is not relevant to us as it has to run or we can't continue.
            // But if the end-user can access fontbm from the command line, so can we
            // And if we can't, something else is wrong and the user should open up an issue
            try
            {
                Console.Write("Checking accessibility fontbm: ");

                Process prc = new Process();
                prc.StartInfo.FileName = "fontbm";
                prc.StartInfo.UseShellExecute = false;
                prc.StartInfo.RedirectStandardError = true;
                prc.StartInfo.RedirectStandardOutput = true;
                prc.Start();
                prc.WaitForExit();

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("TRUE");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("]");
                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FALSE");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("]");
                Console.WriteLine();

                throw new FontGeneratorException("Fontbm does not seem to be accessible from command line. Please make sure the executable has the correct permissions and that the PATH variable contains the folder where arqanore-fontgenerator is located.");
            }

            Console.WriteLine();
        }

        static void Generate(string fontFile, int fontSize, string outputFolder)
        {
            string fontExtension = fontFile.Substring(fontFile.LastIndexOf("."));
            string fontFolder = fontFile.Substring(0, fontFile.Replace("\\", "/").LastIndexOf("/") + 1);
            string fontName = fontFolder.Length > 0 ? fontFile.Replace(fontFolder, "").Replace(fontExtension, "") : fontFile;

            Console.WriteLine($"Generating arqanore font from fontfile {fontFile} with size {fontSize}");

            if (outputFolder == null)
            {
                outputFolder = Directory.GetCurrentDirectory();
            }
            if (!outputFolder.EndsWith("/"))
            {
                outputFolder += "/";
            }

            GenerateFontData(fontFile, fontSize, fontName);
            GenerateFontFile(outputFolder, fontName);
        }
        
        static void GenerateFontData(string fontFile, int fontSize, string fontName)
        {
            string args = $"--font-file {fontFile} --output {fontName} --font-size {fontSize}";
            Console.WriteLine($"Executing fontbm with arguments {args}");

            // Generate the bitmap(s) and font data with fontbmp
            Process prc = new Process();
            prc.StartInfo.FileName = "fontbm";
            prc.StartInfo.Arguments = args;
            prc.StartInfo.WorkingDirectory = tempFolder;
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.RedirectStandardOutput = true;
            prc.StartInfo.RedirectStandardError = true;
            prc.Start();
            prc.WaitForExit();

            // Fetch the output
            string stdError = prc.StandardError.ReadToEnd();
            string stdOutput = prc.StandardOutput.ReadToEnd();

            // Check the output for errors
            if (prc.ExitCode != 0)
            {
                throw new FontGeneratorException($"fontbm exited with non-zero code {prc.ExitCode}: {stdError}");
            }
        }

        static void GenerateFontFile(string fontFolder, string fontName)
        {
            Console.WriteLine($"Merging fontbm output to {fontName}.arqfnt");

            var result = new List<byte>();
            var bitmapBytes = GenerateBitmapBytes(fontName);
            var dataBytes = GenerateDataBytes(fontName);
            var headerBytes = GenerateHeaderBytes(bitmapBytes, dataBytes);

            // Write down all header bytes first
            result.AddRange(headerBytes);

            // Continue with the font data bytes
            result.AddRange(dataBytes);

            // And end with all the bitmap bytes
            foreach (var arr in bitmapBytes)
            {
                result.AddRange(arr);
            }

            // Save the bytes to a file
            File.WriteAllBytes($"{fontFolder}{fontName}.arqfnt", result.ToArray());
        }

        static byte[] GenerateHeaderBytes(List<byte[]> bitmapBytes, byte[] dataBytes)
        {
            var result = new List<byte>();

            // The first byte is the amount of bitmaps in the file
            result.Add((byte)bitmapBytes.Count);

            // The next bytes will contain the length of the databytes
            result.AddRange(Encoding.ASCII.GetBytes(dataBytes.Length.ToString().PadLeft(8, '0')));

            // Then followed by the lenghts of each bitmap bytes
            foreach (var arr in bitmapBytes)
            {
                result.AddRange(Encoding.ASCII.GetBytes(arr.Length.ToString().PadLeft(8, '0')));
            }

            // And that's it!
            return result.ToArray();
        }

        static byte[] GenerateDataBytes(string fontName)
        {
            var lines = File.ReadAllLines($"{tempFolder}/{fontName}.fnt");

            // Data to include
            var lineHeight = 0;
            var baseHeight = 0;
            var charData = new List<int>();

            // Parse every line
            foreach (var line in lines)
            {
                var keyValues = line.Split(' ');
                var type = keyValues[0];

                if (type == "common")
                {
                    for (var i=1; i<keyValues.Length; i++)
                    {
                        var key = keyValues[i].Split('=')[0];
                        var value = int.Parse(keyValues[i].Split('=')[1]);

                        if (key == "lineHeight") lineHeight = value;
                        if (key == "base") baseHeight = value;
                    }
                }
                if (type == "char")
                {
                    var values = new int[9];

                    for (var i=1; i<keyValues.Length; i++)
                    {
                        var key = keyValues[i].Split('=')[0];
                        var value = int.Parse(keyValues[i].Split('=')[1]);

                        if (key == "id") values[0] = value;
                        if (key == "page") values[1] = value;
                        if (key == "x") values[2] = value;
                        if (key == "y") values[3] = value;
                        if (key == "width") values[4] = value;
                        if (key == "height") values[5] = value;
                        if (key == "xoffset") values[6] = value;
                        if (key == "yoffset") values[7] = value;
                        if (key == "xadvance") values[8] = value;
                    }

                    charData.AddRange(values);
                }
            }

            // Generate the data string
            var result = "";
            result += lineHeight + ";";
            result += baseHeight + ";";

            for (var i = 0; i < charData.Count; i += 9)
            {
                result += charData[i + 0].ToString() + ",";
                result += charData[i + 1].ToString() + ",";
                result += charData[i + 2].ToString() + ",";
                result += charData[i + 3].ToString() + ",";
                result += charData[i + 4].ToString() + ",";
                result += charData[i + 5].ToString() + ",";
                result += charData[i + 6].ToString() + ",";
                result += charData[i + 7].ToString() + ",";
                result += charData[i + 8].ToString();
                
                if (i < charData.Count - 9)
                {
                    result += ";";
                }
            }

            return Encoding.ASCII.GetBytes(result);
        }

        static List<byte[]> GenerateBitmapBytes(string fontName)
        {
            var files = Directory.GetFiles(tempFolder, "*.png");
            var result = new List<byte[]>();

            for (var i = 0; i < files.Length; i++)
            {
                var bmp = new Bitmap($"{tempFolder}/{fontName}_{i}.png");

                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    result.Add(ms.ToArray());
                }

                bmp.Dispose();
            }

            return result;
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Usage: arqanore-fontgenerator -f <FILENAME> -s <SIZE>");
            Console.WriteLine();
            Console.WriteLine("ABOUT");
            Console.WriteLine("This tool generates Arqanore font files from TrueType font files using fontbm");
	        Console.WriteLine();
            Console.WriteLine("ARGUMENTS");
            Console.WriteLine("-f      Absolute path to the font file");
            Console.WriteLine("-s      Font size as integer. Cannot be lower than 4.");
            Console.WriteLine("-o      [optional] Output folder of the generated font file. If not provided the working folder will be used.");
        }
    }
}
