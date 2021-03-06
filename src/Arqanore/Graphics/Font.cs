using System;
using System.Linq;
using Arqanore.Math;
using Arqan;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Arqanore.Utils;

namespace Arqanore.Graphics
{
    public class Font
    {
        private const char UNDEFINED = '?';
        
        private Texture[] textures;
        private Glyph[] glyphs;
        private Shader shader;
        private uint vbuffer;
        private uint tcbuffer;
        
        public int LineHeight { get; private set; }
        public int BaseHeight { get; private set; }
        public int TabSize { get; set; }

        public Font()
        {
            TabSize = 4;
        }

        public Font(string path) : this()
        {
            Load(path);
            Generate();
        }

        private void Generate()
        {
            GenerateShader();
            GenerateBuffers();
        }

        private void GenerateBuffers()
        {
            var buffers = new uint[2];
            GL.glGenBuffers(2, buffers);
            vbuffer = buffers[0];
            tcbuffer = buffers[1];

            var totalVertices = new List<float>();
            var totalTexCoords = new List<float>();

            foreach (var glyph in glyphs)
            {
                var texture = textures[glyph.Page];

                var tcX = (1f / texture.Width) * glyph.X;
                var tcY = (1f / texture.Height) * glyph.Y;
                var tcWidth = 1f / ((float) texture.Width / glyph.Width);
                var tcHeight = 1f / ((float) texture.Height / glyph.Height);
                var vx = glyph.OffsetX;
                var vy = glyph.OffsetY;

                var vertices = new float[12]
                {
                    vx, vy,
                    vx + glyph.Width, vy,
                    vx, vy + glyph.Height,
                    vx + glyph.Width, vy,
                    vx, vy + glyph.Height,
                    vx + glyph.Width, vy + glyph.Height
                };

                var texcoords = new float[12]
                {
                    tcX, tcY,
                    tcX + tcWidth, tcY,
                    tcX, tcY + tcHeight,
                    tcX + tcWidth, tcY,
                    tcX, tcY + tcHeight,
                    tcX + tcWidth, tcY + tcHeight
                };

                totalVertices.AddRange(vertices);
                totalTexCoords.AddRange(texcoords);
            }

            var positionAttribLocation = GL.glGetAttribLocation(shader.Id, "aposition");
            var texcoordAttribLocation = GL.glGetAttribLocation(shader.Id, "atexcoord");

            GL.glEnableVertexAttribArray(positionAttribLocation);
            GL.glEnableVertexAttribArray(texcoordAttribLocation);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, vbuffer);
            GL.glBufferData(GL.GL_ARRAY_BUFFER, totalVertices.Count * 4, totalVertices.ToArray(), GL.GL_STATIC_DRAW);
            GL.glVertexAttribPointer(positionAttribLocation, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, tcbuffer);
            GL.glBufferData(GL.GL_ARRAY_BUFFER, totalTexCoords.Count * 4, totalTexCoords.ToArray(), GL.GL_STATIC_DRAW);
            GL.glVertexAttribPointer(texcoordAttribLocation, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);
        }

        private void GenerateShader()
        {
            var vSource = new List<string>();
            vSource.Add("attribute vec2 aposition;\n");
            vSource.Add("attribute vec2 atexcoord;\n");
            vSource.Add("uniform vec2 uresolution;\n");
            vSource.Add("uniform vec2 utranslation;\n");
            vSource.Add("varying vec2 vtexcoord;\n");
            vSource.Add("\n");
            vSource.Add("void main() {\n");
            vSource.Add("vec2 zeroToOne = (aposition + utranslation) / uresolution;\n");
            vSource.Add("vec2 zeroToTwo = zeroToOne * 2.0;\n");
            vSource.Add("vec2 clipSpace = zeroToTwo - 1.0;\n");
            vSource.Add("\n");
            vSource.Add("vtexcoord = atexcoord;\n");
            vSource.Add("\n");
            vSource.Add("gl_Position = vec4(clipSpace.x, -clipSpace.y, 0, 1);\n");
            vSource.Add("}\n");

            var fSource = new List<string>();
            fSource.Add("#version 130\n");
            fSource.Add("\n");
            fSource.Add("precision mediump float;\n");
            fSource.Add("uniform sampler2D uimage;\n");
            fSource.Add("uniform vec4 ucolor;\n");
            fSource.Add("varying vec2 vtexcoord;\n");
            fSource.Add("void main() {\n");
            fSource.Add("gl_FragColor = texture2D(uimage, vtexcoord) * ucolor;\n");
            fSource.Add("}");

            shader = new Shader(vSource, fSource);
        }

        private void Load(string path)
        {
            // Check the file
            if (!path.EndsWith(".arqfnt"))
            {
                throw new ArqanoreException("Not a valid Arqanore font");
            }

            if (!File.Exists(path))
            {
                throw new ArqanoreException($"Unable to find font {path}");
            }

            // Parse the data
            Parse(File.ReadAllBytes(path));
        }

        private void Parse(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            var bmpCount = parser.GetByte();
            var dataLength = parser.GetInt(8);
            var bmpLengths = parser.GetInts(8, bmpCount);
            var data = parser.GetString(dataLength).Split(';');
            var images = parser.GetImages(bmpLengths, bmpCount);

            glyphs = new Glyph[data.Length - 2];
            textures = new Texture[images.Length];

            // Parse the font data
            for (var i = 0; i < data.Length; i++)
            {
                if (i == 0)
                {
                    LineHeight = int.Parse(data[i]);
                }
                else if (i == 1)
                {
                    BaseHeight = int.Parse(data[i]);
                }
                else
                {
                    var glyph = new Glyph();
                    glyph.Char = (char)int.Parse(data[i].Split(',')[0]);
                    glyph.Page = int.Parse(data[i].Split(',')[1]);
                    glyph.X = int.Parse(data[i].Split(',')[2]);
                    glyph.Y = int.Parse(data[i].Split(',')[3]);
                    glyph.Width = int.Parse(data[i].Split(',')[4]);
                    glyph.Height = int.Parse(data[i].Split(',')[5]);
                    glyph.OffsetX = int.Parse(data[i].Split(',')[6]);
                    glyph.OffsetY = int.Parse(data[i].Split(',')[7]);
                    glyph.Advance = int.Parse(data[i].Split(',')[8]);

                    glyphs[i - 2] = glyph;
                }
            }

            // Add all the textures
            for (var i = 0; i < images.Length; i++)
            {
                textures[i] = new Texture(images[i]);
            }
        }

        public void RenderText(string text, float x, float y, int r, int g, int b, int a)
        {
            var advance = 0;
            var line = 0;

            foreach (char c in text)
            {
                var glyph = GetGlyph(c);
                var glyphAdvance = GetGlyphAdvance(c);

                if (c == '\t')
                {
                    glyphAdvance = GetGlyphAdvance(' ');
                }

                if (c == '\n')
                {
                    line++;
                    advance = 0;
                    continue;
                }

                if (glyph == null)
                {
                    continue;
                }

                RenderGlyph(glyph, x + advance, y + (line * BaseHeight), r, g, b, a);
                advance += glyphAdvance;
            }
        }

        private void RenderGlyph(Glyph glyph, float x, float y, float r, float g, float b, float a)
        {
            int index = -1;

            for (var i = 0; i < glyphs.Length; i++)
            {
                if (glyphs[i].Char == glyph.Char)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                return;
            }

            uint positionAttribLocation = GL.glGetAttribLocation(shader.Id, "aposition");
            uint texcoordAttribLocation = GL.glGetAttribLocation(shader.Id, "atexcoord");
            uint translationUniformLocation = GL.glGetUniformLocation(shader.Id, "utranslation");
            uint resolutionUniformLocation = GL.glGetUniformLocation(shader.Id, "uresolution");
            uint colorUniformLocation = GL.glGetUniformLocation(shader.Id, "ucolor");

            GL.glUseProgram(shader.Id);
            GL.glBindTexture(GL.GL_TEXTURE_2D, textures[glyph.Page].Id);

            GL.glUniform2f(translationUniformLocation, x, y);
            GL.glUniform2f(resolutionUniformLocation, Window.Current.Width, Window.Current.Height);
            GL.glUniform4f(colorUniformLocation, r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, vbuffer);
            GL.glVertexAttribPointer(positionAttribLocation, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, tcbuffer);
            GL.glVertexAttribPointer(texcoordAttribLocation, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);

            GL.glPolygonMode(GL.GL_FRONT, GL.GL_FILL);
            GL.glDrawArrays(GL.GL_TRIANGLES, index * 6, 6);

            GL.glUseProgram(0);
            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
        }

        public int MeasureWidth(string text)
        {
            var advance = 0;
            var result = 0;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\t')
                {
                    advance += GetGlyphAdvance(c) * TabSize;
                }

                if (c == '\n')
                {
                    if (advance > result)
                    {
                        result = advance;
                    }

                    advance = 0;
                }

                advance += GetGlyphAdvance(c);
            }

            if (advance > result)
            {
                result = advance;
            }

            return result;
        }

        public int MeasureHeight(string text)
        {
            var result = 1;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\n')
                {
                    result++;
                }
            }

            return result * BaseHeight;
        }

        public Glyph GetGlyph(char c)
        {
            var glyph = glyphs.FirstOrDefault(x => x.Char == c);

            if (glyph == null)
            {
                glyph = glyphs.FirstOrDefault(x => x.Char == UNDEFINED);
            }

            return glyph;
        }

        public int GetGlyphAdvance(char c)
        {
            var glyph = GetGlyph(c);

            if (glyph == null)
            {
                return 0;
            }
            
            return glyph.Advance;
        }

        public class Glyph
        {
            public char Char { get; set; }
            public int Page { get; set; }
            public int OffsetX { get; set; }
            public int OffsetY { get; set; }
            public int Advance { get; set; }

            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}