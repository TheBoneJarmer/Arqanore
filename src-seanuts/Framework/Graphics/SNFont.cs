using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Seanuts.Framework.Math;
using TilarGL;

namespace Seanuts.Framework.Graphics
{
    public class SNFont
    {
        private SNFontData data;
        private SNImage image;
        private float[] ascent;

        public SNImage Image
        {
            get { return image; }
        }
        public SNRectangle[] Bounds
        {
            get { return data.Bounds; }
        }

        public string Family
        {
            get { return data.Font.FontFamily.Name; }
        }
        public float Size
        {
            get { return data.Font.Size; }
        }
        public float[] Ascents
        {
            get { return ascent; }
        }
        
        public SNFont(string filename)
        {
            data = new SNFontData(filename);
            image = new SNImage(data.Bitmap);

            GenerateAscents();
        }

        private void GenerateAscents()
        {
            var codes = new int[5]{ 103, 106, 112, 113, 121 };

            ascent = new float[Bounds.Length];

            for (var i = 0; i < ascent.Length; i++)
            {
                ascent[i] = 0;
            }
            for (var i = 0; i < codes.Length; i++)
            {
                var ascentPoints = (float)data.Font.FontFamily.GetCellAscent(FontStyle.Regular);
                var emHeight = (float)data.Font.FontFamily.GetEmHeight(FontStyle.Regular);
                var ascentPixels = Bounds[i].Height * (ascentPoints / emHeight);

                ascent[codes[i]] = ascentPixels;
            }
        }
    }
}