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
        private Shader shader;
        private float offsetX;
        private float offsetY;
        private float scaleX;
        private float scaleY;
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
            get { return (texture.Width / framesHor) * scaleX; }
        }
        public float Height
        {
            get { return (texture.Height / framesVert) * scaleY; }
        }
        public int FramesHor
        {
            get { return framesHor; }
        }
        public int FramesVert
        {
            get { return framesVert; }
        }

        public Sprite(string path, float scaleX, float scaleY)
        {
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.offsetX = 0;
            this.offsetY = 0;
            this.framesHor = 1;
            this.framesVert = 1;
            this.texture = new Texture(path);

            Generate();
        }
        public Sprite(string path, float frameWidth, float frameHeight, float offsetX, float offsetY, float scaleX, float scaleY)
        {
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.offsetX = offsetX;
            this.offsetY = offsetY;           
            this.framesHor = (int)(texture.Width / frameWidth);
            this.framesVert = (int)(texture.Height / frameHeight);
            this.texture = new Texture(path);   

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
            GL.glGenBuffers(2, buffers);
            vbuffer = buffers[0];
            tcbuffer = buffers[1];

            // Generate vertices and texture coords
            List<float> totalVertices = new List<float>();
            List<float> totalTexCoords = new List<float>();

            for (int frameVert = 0; frameVert < framesVert; frameVert++)
            {
                for (int frameHor = 0; frameHor < framesHor; frameHor++)
                {
                    float width = (texture.Width * scaleX) / framesHor;
                    float height = (texture.Height * scaleY) / framesVert;

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
                        offsetX, offsetY,
                        offsetX + width, offsetY,
                        offsetX, offsetY + height,
                        offsetX + width, offsetY,
                        offsetX, offsetY + height,
                        offsetX + width, offsetY + height
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
            positionAttribLocation = GL.glGetAttribLocation(shader.Id, "aposition");
            texcoordAttribLocation = GL.glGetAttribLocation(shader.Id, "atexcoord");
            rotationUniformLocation = GL.glGetUniformLocation(shader.Id, "urotation");
            translationUniformLocation = GL.glGetUniformLocation(shader.Id, "utranslation");
            resolutionUniformLocation = GL.glGetUniformLocation(shader.Id, "uresolution");
            colorUniformLocation = GL.glGetUniformLocation(shader.Id, "ucolor");
        }

        public void Render(Vector2 position)
        {
            Render(position, 0, 0, 0, Color.WHITE);
        }
        public void Render(float x, float y)
        {
            Render(x, y, 0, 0, 0, 255, 255, 255, 255);
        }
        public void Render(Vector2 position, int frameHor, int frameVert)
        {
            Render(position, frameHor, frameVert, 0, Color.WHITE);
        }
        public void Render(float x, float y, int frameHor, int frameVert)
        {
            Render(x, y, frameHor, frameVert, 0, 255, 255, 255, 255);
        }
        public void Render(Vector2 position, float angle)
        {
            Render(position, 0, 0, angle, Color.WHITE);
        }
        public void Render(float x, float y, float angle)
        {
            Render(x, y, 0, 0, angle, 255, 255, 255, 255);
        }
        public void Render(Vector2 position, int frameHor, int frameVert, float angle)
        {
            Render(position, frameHor, frameVert, angle, Color.WHITE);
        }
        public void Render(float x, float y, int frameHor, int frameVert, float angle)
        {
            Render(x, y, frameHor, frameVert, angle, 255, 255, 255, 255);
        }
        public void Render(Vector2 position, float angle, Color color)
        {
            Render(position, 0, 0, angle, color);
        }
        public void Render(float x, float y, float angle, int r, int g, int b, int a)
        {
            Render(x, y, 0, 0, angle, r, g, b, a);
        }
        public void Render(Vector2 position, int frameHor, int frameVert, float angle, Color color)
        {
            Render(position.X, position.Y, frameHor, frameVert, angle, color.R, color.G, color.B, color.A);
        }
        public void Render(float x, float y, int frameHor, int frameVert, float angle, int r, int g, int b, int a)
        {
            //int frameIndex = (framesVert * frameHor) + frameVert;
            int frameIndex = (framesHor * frameVert) + frameHor;
            double cos = System.Math.Cos(MathHelper.ToRadians(angle + 90));
            double sin = System.Math.Sin(MathHelper.ToRadians(angle + 90));

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, vbuffer);
            GL.glVertexAttribPointer(positionAttribLocation, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);
            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, tcbuffer);
            GL.glVertexAttribPointer(texcoordAttribLocation, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);

            GL.glBindTexture(GL.GL_TEXTURE_2D, texture.Id);
            GL.glUseProgram(shader.Id);

            GL.glUniform2f(translationUniformLocation, x, y);
            GL.glUniform2f(rotationUniformLocation, (float)cos, (float)sin);
            GL.glUniform2f(resolutionUniformLocation, Window.Current.Width, Window.Current.Height);
            GL.glUniform4f(colorUniformLocation, r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);

            GL.glPolygonMode(GL.GL_FRONT, GL.GL_FILL);
            GL.glDrawArrays(GL.GL_TRIANGLES, frameIndex * 6, 6);

            GL.glUseProgram(0);
            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
        }
    }
}
