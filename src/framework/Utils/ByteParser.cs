using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Arqanore.Utils
{
    public class ByteParser
    {
        private int index;
        private List<byte> data;

        public ByteParser(byte[] data)
        {
            this.data = data.ToList();
        }
        public ByteParser(List<byte> data)
        {
            this.data = data;
        }

        public byte GetByte()
        {
            var result = data[index];
            index++;

            return result;
        }
        public byte[] GetBytes(int count)
        {
            var result = data.GetRange(index, count).ToArray();
            index += count;

            return result;
        }
        public string GetString(int count)
        {
            return Encoding.ASCII.GetString(GetBytes(count));
        }
        public int GetInt(int count)
        {
            return int.Parse(GetString(count));
        }
        public int[] GetInts(int count, int n)
        {
            var result = new int[n];

            for (var i=0; i<n; i++)
            {
                result[i] = GetInt(count);
            }

            return result;
        }
        public System.Drawing.Image GetImage(int count)
        {
            var bytes = GetBytes(count);
            var ms = new MemoryStream(bytes);
            var img = System.Drawing.Bitmap.FromStream(ms);

            return img;
        }
        public System.Drawing.Image[] GetImages(int[] count, int n)
        {
            var result = new System.Drawing.Image[n];

            for (var i=0; i<n; i++)
            {
                result[i] = GetImage(count[i]);
            }

            return result;
        }
    }
}