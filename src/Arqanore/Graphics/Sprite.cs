using Arqan;
using Arqanore.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Graphics
{
    public class Sprite
    {
        private Texture texture;
        private Vector2 offset;
        private Vector2 scale;
        private Shader shader;
        private uint vbuffer;
        private uint tcbuffer;

        private uint positionAttribLocation;
        private uint texcoordAttribLocation;
        private uint rotationUniformLocation;
        private uint translationUniformLocation;
        private uint resolutionUniformLocation;
        private uint colorUniformLocation;

        private int framesHor;
        private int framesVert;

        public Texture Texture
        {
            get { return Texture; }
        }
        public float Width
        {
            get { return texture.Width; }
        }
        public float Height
        {
            get { return texture.Height; }
        }

        public Sprite(string path, Vector2 offset, Vector2 scale) : this(path, 1, 1, offset, scale)
        {

        }
        public Sprite(Texture texture, Vector2 offset, Vector2 scale) : this(texture, 1, 1, offset, scale)
        {

        }
        public Sprite(string path, int framesHor, int framesVert, Vector2 offset, Vector2 scale) : this(new Texture(path), framesHor, framesVert, offset, scale)
        {
            
        }
        public Sprite(Texture texture, int framesHor, int framesVert, Vector2 offset, Vector2 scale)
        {
            this.texture = texture;
            this.offset = offset;
            this.scale = scale;
            this.framesHor = framesHor;
            this.framesVert = framesVert;

            Generate();
        }

        private void Generate()
        {
            GenerateShader();
            GenerateAttributeLocations();
            GenerateVBO();
        }
        private void GenerateVBO()
        {
            uint[] buffers = new uint[2];
            GL15.glGenBuffers(2, buffers);
            vbuffer = buffers[0];
            tcbuffer = buffers[1];

            // Generate vertices and texture coords
            List<float> totalVertices = new List<float>();
            List<float> totalTexCoords = new List<float>();

            for (int frameHor = 0; frameHor < framesHor; frameHor++)
            {
                for (int frameVert = 0; frameVert < framesVert; frameVert++)
                {
                    float width = (texture.Width * scale.X) / framesHor;
                    float height = (texture.Height * scale.Y) / framesVert;

                    float clipWidth = texture.Width / framesHor;
                    float clipHeight = texture.Height / framesVert;
                    float clipX = clipWidth * frameHor;
                    float clipY = clipHeight * frameVert;

                    float tcX = (1f / texture.Width) * clipX;
                    float tcY = (1f / texture.Height) * clipY;
                    float tcWidth = 1f / (texture.Width / clipWidth);
                    float tcHeight = 1f / (texture.Height / clipHeight);

                    float[] vertices = new float[12]
                    {
                        offset.X, offset.Y,
                        offset.X + width, offset.Y,
                        offset.X, offset.Y + height,
                        offset.X + width, offset.Y,
                        offset.X, offset.Y + height,
                        offset.X + width, offset.Y + height
                    };

                    float[] texcoords = new float[12]
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
            }

            // Bind the vertices to the buffers
            GL20.glEnableVertexAttribArray(positionAttribLocation);
            GL20.glEnableVertexAttribArray(texcoordAttribLocation);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, totalVertices.Count * 4, totalVertices.ToArray(), GL15.GL_STATIC_DRAW);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, tcbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, totalTexCoords.Count * 4, totalTexCoords.ToArray(), GL15.GL_STATIC_DRAW);
            GL20.glVertexAttribPointer(texcoordAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);
        }
        private void GenerateShader()
        {
            List<string> vSource = new List<string>();
            vSource.Add("attribute vec2 aposition;\n");
            vSource.Add("attribute vec2 atexcoord;\n");
            vSource.Add("uniform vec2 uresolution;\n");
            vSource.Add("uniform vec2 urotation;\n");
            vSource.Add("uniform vec2 utranslation;\n");
            vSource.Add("varying vec2 vtexcoord;\n");
            vSource.Add("\n");
            vSource.Add("void main() {\n");
            vSource.Add("vec2 rotatedPosition = vec2(aposition.x * urotation.y + aposition.y * urotation.x, aposition.y * urotation.y - aposition.x * urotation.x);\n");
            vSource.Add("vec2 zeroToOne = (rotatedPosition + utranslation) / uresolution;\n");
            vSource.Add("vec2 zeroToTwo = zeroToOne * 2.0;\n");
            vSource.Add("vec2 clipSpace = zeroToTwo - 1.0;\n");
            vSource.Add("\n");
            vSource.Add("vtexcoord = atexcoord;\n");
            vSource.Add("\n");
            vSource.Add("gl_Position = vec4(clipSpace.x, -clipSpace.y, 0, 1);\n");
            vSource.Add("}\n");

            List<string> fSource = new List<string>();
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
        private void GenerateAttributeLocations()
        {
            positionAttribLocation = GL20.glGetAttribLocation(shader.Id, "aposition");
            texcoordAttribLocation = GL20.glGetAttribLocation(shader.Id, "atexcoord");
            rotationUniformLocation = GL20.glGetUniformLocation(shader.Id, "urotation");
            translationUniformLocation = GL20.glGetUniformLocation(shader.Id, "utranslation");
            resolutionUniformLocation = GL20.glGetUniformLocation(shader.Id, "uresolution");
            colorUniformLocation = GL20.glGetUniformLocation(shader.Id, "ucolor");
        }

        public void Render(Vector2 position)
        {
            Render(position, 0, 0, 0, Color.WHITE);
        }
        public void Render(Vector2 position, int frameHor, int frameVert)
        {
            Render(position, frameHor, frameVert, 0, Color.WHITE);
        }
        public void Render(Vector2 position, float angle)
        {
            Render(position, 0, 0, angle, Color.WHITE);
        }
        public void Render(Vector2 position, int frameHor, int frameVert, float angle)
        {
            Render(position, frameHor, frameVert, angle, Color.WHITE);
        }
        public void Render(Vector2 position, float angle, Color color)
        {
            Render(position, 0, 0, angle, color);
        }
        public void Render(Vector2 position, int frameHor, int frameVert, float angle, Color color)
        {
            int frameIndex = (frameHor * framesHor) + frameVert;
            double cos = System.Math.Cos(MathHelper.ToRadians(angle + 90));
            double sin = System.Math.Sin(MathHelper.ToRadians(angle + 90));

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);
            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, tcbuffer);
            GL20.glVertexAttribPointer(texcoordAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);
            GL11.glBindTexture(GL11.GL_TEXTURE_2D, texture.Id);
            GL20.glUseProgram(shader.Id);
            GL20.glUniform2f(translationUniformLocation, position.X, position.Y);
            GL20.glUniform2f(rotationUniformLocation, (float)cos, (float)sin);
            GL20.glUniform2f(resolutionUniformLocation, Window.Current.Width, Window.Current.Height);
            GL20.glUniform4f(colorUniformLocation, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            GL10.glPolygonMode(GL11.GL_FRONT, GL11.GL_FILL);
            GL11.glDrawArrays(GL11.GL_TRIANGLES, frameIndex * 6, 6);
            GL20.glUseProgram(0);
            GL11.glBindTexture(GL11.GL_TEXTURE_2D, 0);
        }
    }
}
