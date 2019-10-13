using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using TilarGL;

namespace Seanuts.Framework.Graphics
{
    public class Background
    {
        public uint Id { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Background(Bitmap bmp)
        {
            Load(bmp);
        }
        public Background(string src)
        {
            Load(new Bitmap(Image.FromFile(src)));
        }

        private void Load(Bitmap bmp)
        {
            var ids = new uint[1];
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Set properties
            Width = bmp.Width;
            Height = bmp.Height;

            // Generate 2D texture
            GL11.glGenTextures(1, ids);
            GL11.glBindTexture(GL11.GL_TEXTURE_2D, ids[0]);
            GL10.glTexParameteri(GL11.GL_TEXTURE_2D, GL11.GL_TEXTURE_WRAP_S, GL12.GL_CLAMP_TO_EDGE);
            GL10.glTexParameteri(GL11.GL_TEXTURE_2D, GL11.GL_TEXTURE_WRAP_T, GL12.GL_CLAMP_TO_EDGE);
            GL10.glTexParameteri(GL11.GL_TEXTURE_2D, GL11.GL_TEXTURE_MIN_FILTER, GL11.GL_NEAREST);
            GL10.glTexParameteri(GL11.GL_TEXTURE_2D, GL11.GL_TEXTURE_MAG_FILTER, GL11.GL_NEAREST);
            GL10.glTexImage2D(GL11.GL_TEXTURE_2D, 0, GL11.GL_RGBA, bmp.Width, bmp.Height, 0, GL12.GL_BGRA, GL11.GL_UNSIGNED_BYTE, data.Scan0);

            // Cleanup
            bmp.UnlockBits(data);
            GL11.glBindTexture(GL11.GL_TEXTURE_2D, 0);

            // Finish
            Id = ids[0];
        }
    }
}
