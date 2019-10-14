using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Seanuts.Framework.Math;

namespace Seanuts.Framework
{
    public class SNFontData
    {
        public int GlyphSize { get; private set; }
        public int GlyphsHor { get; private set; }
        public int GlyphsVert { get; private set; }

        public Bitmap Bitmap { get; set; }
        public SNRectangle[] Bounds { get; set; }
        public Font Font { get; set; }
        public Color Color { get; set; }

        public SNFontData()
        {
            Bounds = new SNRectangle[255];
            Color = Color.White;
            Font = new Font("Arial", 12);
            GlyphSize = 24;

            GlyphsHor = 16;
            GlyphsVert = 16;
        }
        public SNFontData(string path) : this()
        {
            if (!path.EndsWith(".snfnt"))
            {
                throw new SeanutsException("Invalid font extension");
            }

            try
            {
                var bytes = File.ReadAllBytes(path);
                var index = 40;

                // Get all indices from the header
                var bmpBytesCount = int.Parse(Encoding.ASCII.GetString(bytes, 0, 8));
                var glyphBoundsDataBytesCount = int.Parse(Encoding.ASCII.GetString(bytes, 8, 8));
                var fontFamilyBytesCount = int.Parse(Encoding.ASCII.GetString(bytes, 16, 8));
                var fontSizeBytesCount = int.Parse(Encoding.ASCII.GetString(bytes, 24, 8));
                var argbBytesCount = int.Parse(Encoding.ASCII.GetString(bytes, 32, 8));

                // Get all the data
                var fontFamily = Encoding.ASCII.GetString(bytes, index, fontFamilyBytesCount);
                index += fontFamilyBytesCount;

                var fontSize = float.Parse(Encoding.ASCII.GetString(bytes, index, fontSizeBytesCount));
                index += fontSizeBytesCount;

                var argb = int.Parse(Encoding.ASCII.GetString(bytes, index, argbBytesCount));
                index += argbBytesCount;

                var bmpBytes = bytes.ToList().GetRange(index, bmpBytesCount).ToArray();
                index += bmpBytesCount;

                var glyphBoundsData = Encoding.ASCII.GetString(bytes, index, glyphBoundsDataBytesCount);

                // Generate the font and color property
                this.Font = new Font(fontFamily, fontSize);
                this.Color = Color.FromArgb(argb);

                // Fill the bounds array
                for (var i = 0; i < Bounds.Length; i++)
                {
                    var entry = glyphBoundsData.Split(';')[i];
                    var x = float.Parse(entry.Split(',')[0]);
                    var y = float.Parse(entry.Split(',')[1]);
                    var width = float.Parse(entry.Split(',')[2]);
                    var height = float.Parse(entry.Split(',')[3]);

                    Bounds[i] = new SNRectangle(x, y, width, height);
                }

                // Generate the bitmap
                using (var ms = new MemoryStream(bmpBytes))
                {
                    Bitmap = new Bitmap(ms);
                }

                // Set some remaining variables
                GlyphSize = (int)System.Math.Floor(fontSize * 2);
            }
            catch (Exception)
            {
                throw new SeanutsException("Unable to parse Seanuts Font file. Data is corrupt");
            }
        }

        public SNFontData(string fontFamily, float fontSize, int r, int g, int b, int a) : this()
        {
            GlyphSize = (int)System.Math.Floor(fontSize * 2);
            Color = Color.FromArgb(a, r, g, b);
            Font = new Font(fontFamily, fontSize);
            Bitmap = new Bitmap(GlyphSize * GlyphsHor, GlyphSize * GlyphsVert);
            
            GenerateBitmap();
            GenerateBounds();
        }

        public void Save(string filename)
        {
            if (!filename.EndsWith(".snfnt"))
            {
                throw new SeanutsException("Filename must end with .snfnt extension");
            }

            var bmpBytes = GenerateBitmapBytes();
            var glyphBoundsDataBytes = GenerateGlyphBoundsDataBytes();
            var headerBytes = GenerateHeaderBytes(bmpBytes.Length, glyphBoundsDataBytes.Length);

            var fs = new FileStream(filename, FileMode.Create);

            fs.Write(headerBytes, 0, headerBytes.Length);
            fs.Write(bmpBytes, 0, bmpBytes.Length);
            fs.Write(glyphBoundsDataBytes, 0, glyphBoundsDataBytes.Length);

            fs.Close();
        }

        private byte[] GenerateHeaderBytes(int bmpBytesCount, int glyphBoundsDataBytesCount)
        {
            var result = new List<byte>();

            // Add all lengths at the begin of the header
            result.AddRange(Encoding.ASCII.GetBytes(bmpBytesCount.ToString().PadLeft(8, '0')));
            result.AddRange(Encoding.ASCII.GetBytes(glyphBoundsDataBytesCount.ToString().PadLeft(8, '0')));
            result.AddRange(Encoding.ASCII.GetBytes(Font.FontFamily.Name.Length.ToString().PadLeft(8, '0')));
            result.AddRange(Encoding.ASCII.GetBytes(Font.Size.ToString().Length.ToString().PadLeft(8, '0')));
            result.AddRange(Encoding.ASCII.GetBytes(Color.ToArgb().ToString().Length.ToString().PadLeft(8, '0')));

            // Add the font data
            result.AddRange(Encoding.ASCII.GetBytes(Font.FontFamily.Name));
            result.AddRange(Encoding.ASCII.GetBytes(Font.Size.ToString()));
            result.AddRange(Encoding.ASCII.GetBytes(Color.ToArgb().ToString()));

            return result.ToArray();
        }
        private byte[] GenerateBitmapBytes()
        {
            var result = new List<byte>();
            var ms = new MemoryStream();
            
            Bitmap.Save(ms, ImageFormat.Png);
            result.AddRange(ms.ToArray());

            ms.Close();

            return result.ToArray();
        }
        private byte[] GenerateGlyphBoundsDataBytes()
        {
            var result = new List<byte>();

            // Write down all the bounds
            for (var i = 0; i < Bounds.Length; i++)
            {
                result.AddRange(Encoding.ASCII.GetBytes(Bounds[i].X.ToString() + ","));
                result.AddRange(Encoding.ASCII.GetBytes(Bounds[i].Y.ToString() + ","));
                result.AddRange(Encoding.ASCII.GetBytes(Bounds[i].Width.ToString() + ","));
                result.AddRange(Encoding.ASCII.GetBytes(Bounds[i].Height.ToString()));
                
                if (i < Bounds.Length - 1)
                {
                    result.AddRange(Encoding.ASCII.GetBytes(";"));
                }
            }

            return result.ToArray();
        }

        public void GenerateBitmap()
        {
            if (Bitmap == null)
            {
                Bitmap = new Bitmap(GlyphSize * GlyphsHor, GlyphSize * GlyphsVert);
            }

            var brush = new SolidBrush(Color);            
            var grp = System.Drawing.Graphics.FromImage(Bitmap);

            grp.Clear(Color.FromArgb(0, 0, 0, 0));

            for (var i=0; i<Bounds.Length; i++)
            {
                var chr = (char)i;
                var y = (float)System.Math.Floor(i / (float)GlyphsVert);
                var x = (float)System.Math.Floor(((i / (float)GlyphsVert) - y) * (float)GlyphsHor);
                var point = new PointF(x * GlyphSize, y * GlyphSize);
                
                grp.DrawString(chr.ToString(), Font, brush, point);
            }
        }
        public void GenerateBounds()
        {
            for (var i=0; i<Bounds.Length; i++)
            {
                var y = (float)System.Math.Floor(i / (float)GlyphsVert);
                var x = (float)System.Math.Floor(((i / (float)GlyphsVert) - y) * (float)GlyphsHor);

                Bounds[i] = CalculateGlyphBounds((int)x, (int)y, GlyphSize);
            }
        }

        private SNRectangle CalculateGlyphBounds(int cellX, int cellY, int cellSize)
        {
            var result = new SNRectangle();
            var smallest = new PointF(-1, -1);
            var biggest = new PointF(-1, -1);

            var x1 = cellX * cellSize;
            var y1 = cellY * cellSize;
            var x2 = (cellX + 1) * cellSize;
            var y2 = (cellY + 1) * cellSize;
            
            for (var x=x1; x<x2; x++)
            {
                for (var y=y1; y<y2; y++)
                {
                    var pixel = Bitmap.GetPixel(x, y);

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
            result.Width = biggest.X - smallest.X;
            result.Height = biggest.Y - smallest.Y;

            if (result.X < 0) result.X = 0;
            if (result.Y < 0) result.Y = 0;
            if (result.Width < 0) result.Width = 0;
            if (result.Height < 0) result.Height = 0;

            return result;
        }
    }
}