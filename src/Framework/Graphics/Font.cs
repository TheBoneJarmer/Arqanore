using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TilarGL;

namespace Seanuts.Framework.Graphics
{
    public class Font
    {
        public Sprite Bitmap { get; private set; }
        public string Family { get; private set; }
        public int Size { get; private set; }

        public List<Glyph> Glyphs { get; private set; }

        public Font()
        {
            Glyphs = new List<Glyph>();
        }
        public Font(string name) : this()
        {
            Load(name);
        }

        private void Load(string path)
        {
            // Search for the tga and csv files
            if (!File.Exists($"{path}.png"))
            {
                throw new FileNotFoundException($"Unable to find bitmap image {path}.png");
            }
            if (!File.Exists($"{path}.csv"))
            {
                throw new FileNotFoundException($"Unable to find bitmap data {path}.csv");
            }

            // Load the tga file
            LoadPNG(path);

            // Load the csv file
            LoadCSV(path);
        }

        private void LoadPNG(string path)
        {
            this.Bitmap = new Sprite($"{path}.png");
        }
        private void LoadCSV(string path)
        {
            var lines = File.ReadAllLines($"{path}.csv");
            var firstAsciiCode = 0;

            // Parse each line in the CSV file
            foreach (var line in lines)
            {
                var key = line.Split(',')[0];
                var value = line.Split(',')[1];

                if (key == "Font Name")
                {
                    this.Family = value;
                }
                if (key == "Font Height")
                {
                    this.Size = value.ToInt();
                }
                if (key == "Start Char")
                {
                    firstAsciiCode = value.ToInt();
                }
                if (Regex.IsMatch(key, "Char ([0-9]*) Base Width"))
                {
                    var asciiCode = key.Replace(" ", "").Replace("Char", "").Replace("BaseWidth", "").ToInt();
                    var chr = (char)asciiCode;

                    if (Glyphs.Any(x => x.Char == chr))
                    {
                        continue;
                    }

                    var glyph = new Glyph();
                    glyph.Id = asciiCode;
                    glyph.Char = chr;
                    glyph.Width = value.ToInt();

                    Glyphs.Add(glyph);
                }
            }

            // Update all X and Y positions from the bitmap for all glyphs
            UpdateGlyphsPosition(firstAsciiCode);
        }

        private void UpdateGlyphsPosition(int firstAsciiCode)
        {
            for (var y = 0; y < 16; y++)
            {
                for (var x = 0; x < 16; x++)
                {
                    var index = (y * 16) + x;
                    var asciiCode = (byte)(index + firstAsciiCode);
                    var chr = Convert.ToChar(asciiCode);
                    var glyph = Glyphs.FirstOrDefault(g => g.Char == chr);

                    if (glyph == null)
                    {
                        continue;
                    }

                    glyph.X = x;
                    glyph.Y = y;
                }
            }
        }

        public int MeasureText(string text)
        {
            var result = 0;

            foreach (var c in text)
            {
                result += Glyphs.First(g => g.Char == c).Width;
            }

            return result;
        }
    }

    public class Glyph
    {
        public int Id { get; set; }

        public char Char { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
