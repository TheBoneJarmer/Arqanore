using System;
using System.Linq;
using Seanuts.Math;
using TilarGL;

namespace Seanuts.Graphics
{
    public class Font
    {
        private FontData data;
        private Image image;

        public Image Image
        {
            get { return image; }
        }
        public Rectangle[] Bounds
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

        public Font(string filename)
        {
            data = new FontData(filename);
            image = new Image(data.Bitmap);
        }
    }
}