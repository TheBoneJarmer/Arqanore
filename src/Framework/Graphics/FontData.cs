using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Seanuts.Framework
{
    public class FontData
    {
        private int CellSize { get; set; }
        private int GlyphsHor { get; set; }
        private int GlyphsVert { get; set; }

        public System.Drawing.Bitmap Bitmap { get; private set; }
        public System.Drawing.RectangleF[] Bounds { get; private set; }
        public System.Drawing.Font Font { get; private set; }
        public System.Drawing.Brush Brush { get; private set; }

        public FontData(string fontFamily, float fontSize, System.Drawing.Color color)
        {
            CellSize = (int)System.Math.Floor(fontSize * 2);
            GlyphsHor = 16;
            GlyphsVert = 16;

            Bounds = new System.Drawing.RectangleF[255];
            Font = new System.Drawing.Font(fontFamily, fontSize);
            Brush = new System.Drawing.SolidBrush(color);
            Bitmap = new System.Drawing.Bitmap(CellSize * GlyphsHor, CellSize * GlyphsVert);

            GenerateBitmap();
            GenerateBounds();
        }

        public void Save()
        {
            var bmpBytes = GenerateBitmapBytes();
            var dataBytes = GenerateDataBytes();
            var headerBytes = GenerateHeaderBytes(bmpBytes.Length, dataBytes.Length);

            var fs = new FileStream(Font.FontFamily.Name + ".seafnt", FileMode.Create);

            fs.Write(headerBytes, 0, headerBytes.Length);
            fs.Write(bmpBytes, 0, bmpBytes.Length);
            fs.Write(dataBytes, 0, dataBytes.Length);

            fs.Close();
        }

        private byte[] GenerateHeaderBytes(int bmpBytesCount, int dataBytesCount)
        {
            var result = new List<byte>();

            result.AddRange(Encoding.ASCII.GetBytes(bmpBytesCount.ToString().PadLeft(8, '0')));
            result.AddRange(Encoding.ASCII.GetBytes(dataBytesCount.ToString().PadLeft(8, '0')));

            return result.ToArray();
        }
        private byte[] GenerateBitmapBytes()
        {
            var result = new List<byte>();
            var ms = new MemoryStream();
            
            Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            result.AddRange(ms.ToArray());

            ms.Close();

            return result.ToArray();
        }
        private byte[] GenerateDataBytes()
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

        private void GenerateBitmap()
        {
            var grp = System.Drawing.Graphics.FromImage(Bitmap);

            for (var i=0; i<Bounds.Length; i++)
            {
                var chr = (char)i;
                var y = (float)System.Math.Floor(i / (float)GlyphsVert);
                var x = (float)System.Math.Floor(((i / (float)GlyphsVert) - y) * (float)GlyphsHor);
                var point = new System.Drawing.PointF(x * CellSize, y * CellSize);
                
                grp.DrawString(chr.ToString(), Font, Brush, point);
            }
        }
        private void GenerateBounds()
        {
            for (var i=0; i<Bounds.Length; i++)
            {
                var y = (float)System.Math.Floor(i / (float)GlyphsVert);
                var x = (float)System.Math.Floor(((i / (float)GlyphsVert) - y) * (float)GlyphsHor);

                Bounds[i] = CalculateGlyphBounds((int)x, (int)y, CellSize);
            }
        }

        private System.Drawing.RectangleF CalculateGlyphBounds(int cellX, int cellY, int cellSize)
        {
            var result = new System.Drawing.RectangleF();
            var smallest = new System.Drawing.PointF(-1, -1);
            var biggest = new System.Drawing.PointF(-1, -1);

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