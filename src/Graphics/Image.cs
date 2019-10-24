using System;
using System.Collections.Generic;
using System.Text;
using Arqan;

namespace Arqanore.Graphics
{
    public class Image
    {
        public uint Id { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Image(System.Drawing.Bitmap bmp)
        {
            Load(bmp);
        }
        public Image(string filename)
        {
            Load(new System.Drawing.Bitmap(System.Drawing.Image.FromFile(filename)));
        }

        private void Load(System.Drawing.Bitmap bmp)
        {
            var ids = new uint[1];
            var data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

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
