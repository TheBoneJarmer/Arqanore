using Arqanore.Graphics;
using Arqanore.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Arqanore.Compression
{
    public class AssetPack
    {
        public List<Texture> Textures { get; private set; }
        public List<Font> Fonts { get; private set; }

        public AssetPack(string filename)
        {
            Textures = new List<Texture>();
            Fonts = new List<Font>();
        }

        private void Load(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File {filename} not found");
            }

            Parse(File.ReadAllBytes(filename));
        }

        private void Parse(byte[] data)
        {
            var parser = new ByteParser(data);
            var texCount = parser.GetInt(8);
            var fontCount = parser.GetInt(8);
        }
    }
}
