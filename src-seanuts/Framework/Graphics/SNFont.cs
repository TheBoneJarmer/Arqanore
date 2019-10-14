using System;
using System.Drawing;
using System.Drawing.Imaging;
using Seanuts.Framework.Math;
using TilarGL;

namespace Seanuts.Framework.Graphics
{
    public class SNFont
    {
        private SNFontData data;
        private SNImage image;

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
        
        public SNFont(string filename)
        {
            data = new SNFontData(filename);
            image = new SNImage(data.Bitmap);
        }
    }
}