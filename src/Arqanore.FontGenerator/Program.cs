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

        static void Main(string[] args)
        {
            tempFolder = ".temp" + Guid.NewGuid().ToString().Replace("-", "");
            int exitCode = 0;

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

            try
            {
                // Create a temp folder
                Directory.CreateDirectory(tempFolder);

                // Run the generator
                Run(args);
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
                // Delete the temp folder
                if (Directory.Exists(tempFolder))
                {
                    Directory.Delete(tempFolder, true);
                }
            }

            Environment.Exit(exitCode);
        }

        static void Run(string[] args)
        {
            string fontFile = null;
            int fontSize = 0;
            string outputFolder = null;

            // Fetch data from the args
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

            // Check if the user provided all info
            if (fontFile == null || fontSize == 0)
            {
                throw new Exception("Missing required parameters");
            }

            // Validate the input
            if (!File.Exists(fontFile))
            {
                throw new Exception($"Unable to find file {fontFile}");
            }
            if (fontSize < 4)
            {
                throw new Exception("Font size must be minimum 4 points");
            }

            ValidateRequirements();
            Generate(fontFile, fontSize, outputFolder);
        }

        static void ValidateRequirements()
        {
            // Just run fontbm to check if it runs at all
            // It will exit with a non-zero code because no arguments were provided but that is ok
            // If it does not run, the reason why is not relevant to us as it has to run or we can't continue.
            // But if the end-user can access fontbm from the command line, so can we
            // And if we can't, something else is wrong and the user should open up an issue
            try
            {
                Process prc = new Process();
                prc.StartInfo.FileName = "fontbm";
                prc.StartInfo.UseShellExecute = false;
                prc.StartInfo.RedirectStandardError = true;
                prc.StartInfo.RedirectStandardOutput = true;
                prc.Start();
                prc.WaitForExit();
            }
            catch (Exception)
            {
                throw new Exception("Unable to run fontbm. Please make sure fontbm is accessible from the command line.");
            }
        }

        static void FixPermissions()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(location);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return;
            }
            if (File.Exists($"{folder}/.fixedpermissions"))
            {
                return;
            }

            Process prc = new Process();
            prc.StartInfo.FileName = "chmod";
            prc.StartInfo.Arguments = "+x fontbm";
            prc.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
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
                throw new Exception("[CHMOD] " + stdError);
            }
            
            File.Create($"{folder}/.fixedpermissions");
        }

        static void Generate(string fontFile, int fontSize, string outputFolder)
        {
            string fontExtension = fontFile.Substring(fontFile.LastIndexOf("."));
            string fontFolder = fontFile.Substring(0, fontFile.Replace("\\", "/").LastIndexOf("/") + 1);
            string fontName = fontFolder.Length > 0 ? fontFile.Replace(fontFolder, "").Replace(fontExtension, "") : fontFile;

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
            // Generate the bitmap(s) and font data with fontbmp
            Process prc = new Process();
            prc.StartInfo.FileName = $"fontbm";
            prc.StartInfo.Arguments = $"--font-file {fontFile} --output {fontName} --font-size {fontSize}";
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
                throw new Exception("[FONTBM] " + stdError);
            }
        }

        static void GenerateFontFile(string fontFolder, string fontName)
        {
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
            Console.WriteLine("Usage: font-generator -f <FILENAME> -s <SIZE>");
            Console.WriteLine();
            Console.WriteLine("ABOUT");
            Console.Write("This tool generates an font file which can be used by the Arqanore framework.");
            Console.Write(" This tool uses the application fontbm to generate the bitmaps and font data and 'packs' that data into a single file");
            Console.Write(" in a format is being used by the Arqanore framework.\n");
            Console.WriteLine();
            Console.WriteLine("ARGUMENTS");
            Console.WriteLine("-f      Name of the font file with extension");
            Console.WriteLine("-s      Font size as integer. Cannot be lower than 4.");
            Console.WriteLine("-o      [optional] Output folder of the generated font files. If not provided the working folder will be used.");
        }
    }
}
