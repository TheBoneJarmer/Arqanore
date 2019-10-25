using System;
using System.Linq;
using Arqanore.Math;
using Arqan;
using System.IO;
using System.Text;

namespace Arqanore.Graphics
{
    public class Font
    {
        public Rectangle[] GlyphBounds { get; private set; }
        public byte[] GlyphCodes { get; private set; }
        public int GlyphSize { get; private set; }
        public int GlyphsHor { get; private set; }
        public int GlyphsVert { get; private set; }

        public Image Image { get; set; }
        public string Family { get; private set; }
        public int Size { get; private set; }

        public Font()
        {
            GlyphCodes = new byte[94];
            GlyphBounds = new Rectangle[94];
            GlyphSize = 24;
            GlyphsHor = 10;
            GlyphsVert = 10;

            // Fill the list of codes
            for (byte i = 32; i < 127; i++)
            {
                GlyphCodes[i - 32] = i;
            }
        }

        public void Save(string filename)
        {

        }

        public void GenerateBounds()
        {
            for (var i = 0; i < GlyphBounds.Length; i++)
            {
                var y = (float)System.Math.Floor(i / (float)GlyphsVert);
                var x = (float)System.Math.Floor(((i / (float)GlyphsVert) - y) * (float)GlyphsHor);



                GlyphBounds[i] = CalculateGlyphBounds((int)x, (int)y, GlyphSize);
            }
        }

        private Rectangle CalculateGlyphBounds(int cellX, int cellY, int cellSize)
        {
            var result = new Rectangle();
            var smallest = new Vector2(-1, -1);
            var biggest = new Vector2(-1, -1);

            var x1 = cellX * cellSize;
            var y1 = cellY * cellSize;
            var x2 = (cellX + 1) * cellSize;
            var y2 = (cellY + 1) * cellSize;

            for (var x = x1; x < x2; x++)
            {
                for (var y = y1; y < y2; y++)
                {
                    var pixel = Image.Bitmap.GetPixel(x, y);

                    // Transparant pixels mean unexisting pixels
                    if (pixel.A == 0)
                    {
                        continue;
                    }

                    // Compare the pixel locations with one another
                    if (smallest.X == -1 && smallest.Y == -1)
                    {
                        smallest.X = x;
                        smallest.Y = y;
                    }
                    else if (biggest.X == -1 && biggest.Y == -1)
                    {
                        biggest.X = x;
                        biggest.Y = y;
                    }
                    else
                    {
                        if (x < smallest.X)
                        {
                            smallest.X = x;
                        }
                        if (x > biggest.X)
                        {
                            biggest.X = x;
                        }
                        if (y < smallest.Y)
                        {
                            smallest.Y = y;
                        }
                        if (y > biggest.Y)
                        {
                            biggest.Y = y;
                        }
                    }
                }
            }

            // Calculate the differences
            result.X = smallest.X;
            result.Y = smallest.Y;
            result.Width = biggest.X - smallest.X + 1;
            result.Height = biggest.Y - smallest.Y + 1;

            if (result.X < 0) result.X = 0;
            if (result.Y < 0) result.Y = 0;
            if (result.Width < 0) result.Width = 0;
            if (result.Height < 0) result.Height = 0;

            return result;
        }
    }
}