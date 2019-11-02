using System;
using System.Linq;
using Arqanore.Math;
using Arqan;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Arqanore.Utils;

namespace Arqanore.Graphics
{
    public class Font
    {
        public List<Texture> Textures { get; private set; }
        public List<Glyph> Glyphs { get; private set; }
        public int LineHeight { get; private set; }
        public int BaseHeight { get; private set; }

        public Font()
        {
            Textures = new List<Texture>();
            Glyphs = new List<Glyph>();
        }
        public Font(string path) : this()
        {
            Load(path);
        }

        private void Load(string path)
        {
            var data = new byte[0];

            // Check the file
            if (!path.EndsWith(".arqfnt"))
            {
                throw new ArqanoreException("Not a valid Arqanore font");
            }

            // Load the data
            data = File.ReadAllBytes(path);

            // Parse the data
            Parse(data.ToList());
        }
        private void Parse(List<byte> bytes)
        {
            var parser = new ByteParser(bytes);
            var bmpCount = parser.GetInt(8);
            var dataLength = parser.GetInt(8);
            var bmpLengths = parser.GetInts(8, bmpCount);
            var data = parser.GetString(dataLength).Split(';');
            var images = parser.GetImages(bmpLengths, bmpCount);

            // Parse the font data
            for (var i = 0; i < data.Length; i++)
            {
                if (i == 0)
                {
                    LineHeight = int.Parse(data[i]);
                }
                else if (i == 1)
                {
                    BaseHeight = int.Parse(data[i]);
                }
                else
                {
                    var glyph = new Glyph();
                    glyph.Id = short.Parse(data[i].Split(',')[0]);
                    glyph.Page = short.Parse(data[i].Split(',')[1]);
                    glyph.X = int.Parse(data[i].Split(',')[2]);
                    glyph.Y = int.Parse(data[i].Split(',')[3]);
                    glyph.Width = int.Parse(data[i].Split(',')[4]);
                    glyph.Height = int.Parse(data[i].Split(',')[5]);
                    glyph.OffsetX = int.Parse(data[i].Split(',')[6]);
                    glyph.OffsetY = int.Parse(data[i].Split(',')[7]);
                    glyph.Advance = int.Parse(data[i].Split(',')[8]);

                    Glyphs.Add(glyph);
                }
            }

            // Add all the textures
            foreach (var img in images)
            {
                Textures.Add(new Texture(img));
            }
        }

        public class Glyph
        {
            public short Id { get; set; }
            public short Page { get; set; }
            public int OffsetX { get; set; }
            public int OffsetY { get; set; }
            public int Advance { get; set; }

            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}