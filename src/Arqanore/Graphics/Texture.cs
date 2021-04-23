using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Arqan;
using Arqanore.Utils;

namespace Arqanore.Graphics
{
    public class Texture
    {
        public uint Id { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Bitmap Bitmap { get; private set; }
        public IntPtr Pixels { get; private set; }
        public PixelFormat PixelFormat { get; private set; }

        public Texture(Image img)
        {
            Load(new Bitmap(img));
        }
        public Texture(Bitmap bmp)
        {
            Load(bmp);
        }
        public Texture(string filename)
        {
            // Check the file
            if (!filename.EndsWith(".arqtex"))
            {
                throw new ArqanoreException("Not a valid Arqanore texture");
            }
            if(!File.Exists(filename))
            {
                throw new ArqanoreException($"File {filename} not found");
            }

            // Parse the data
            Parse(File.ReadAllBytes(filename));
        }
        public Texture(byte[] data)
        {
            Parse(data);
        }

        private void Parse(byte[] data)
        {
            var parser = new ByteParser(data);
            var img = parser.GetImage(data.Length);
            var bmp = new Bitmap(img);

            Load(bmp);
        }
        private void Load(Bitmap bmp)
        {
            var ids = new uint[1];
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Set properties
            Width = bmp.Width;
            Height = bmp.Height;
            Bitmap = bmp;
            Pixels = data.Scan0;
            PixelFormat = data.PixelFormat;

            // Generate 2D texture
            GL.glGenTextures(1, ids);
            GL.glBindTexture(GL.GL_TEXTURE_2D, ids[0]);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, GL.GL_REPEAT);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, GL.GL_REPEAT);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_NEAREST);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_NEAREST);
            GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_RGBA, bmp.Width, bmp.Height, 0, GL.GL_BGRA, GL.GL_UNSIGNED_BYTE, Pixels);

            // Cleanup
            bmp.UnlockBits(data);
            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);

            // Finish
            Id = ids[0];
        }

        public Color GetPixel(int x, int y)
        {
            return new Color(Bitmap.GetPixel(x, y));
        }
    }
}
