using Arqan;
using Arqanore.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Graphics
{
    public class Polygon
    {
        private Shader shader;
        private uint buffer;
        private float[] vertices;

        public float[] Vertices
        {
            get { return vertices; }
        }

        public Polygon(float[] vertices)
        {
            this.vertices = vertices;

            GenerateShader();
            GenerateVBO(vertices);
        }

        private void GenerateVBO(float[] vertices)
        {
            uint[] buffers = new uint[1];
            GL.glGenBuffers(1, buffers);
            buffer = buffers[0];

            uint positionAttribLocation = GL.glGetAttribLocation(shader.Id, "aposition");

            GL.glEnableVertexAttribArray(positionAttribLocation);
            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, buffer);
            GL.glBufferData(GL.GL_ARRAY_BUFFER, vertices.Length * 4, vertices, GL.GL_STATIC_DRAW);
            GL.glVertexAttribPointer(positionAttribLocation, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);
        }

        private void GenerateShader()
        {
            List<string> vSource = new List<string>();
            vSource.Add("attribute vec2 aposition;\n");
            vSource.Add("uniform vec2 uresolution;\n");
            vSource.Add("uniform vec2 urotation;\n");
            vSource.Add("uniform vec2 utranslation;\n");
            vSource.Add("\n");
            vSource.Add("void main() {\n");
            vSource.Add("vec2 rotatedPosition = vec2(aposition.x * urotation.y + aposition.y * urotation.x, aposition.y * urotation.y - aposition.x * urotation.x);\n");
            vSource.Add("vec2 zeroToOne = (rotatedPosition + utranslation) / uresolution;\n");
            vSource.Add("vec2 zeroToTwo = zeroToOne * 2.0;\n");
            vSource.Add("vec2 clipSpace = zeroToTwo - 1.0;\n");
            vSource.Add("gl_Position = vec4(clipSpace.x, -clipSpace.y, 0, 1);\n");
            vSource.Add("}\n");

            List<string> fSource = new List<string>();
            fSource.Add("#version 130\n");
            fSource.Add("\n");
            fSource.Add("precision mediump float;\n");
            fSource.Add("uniform vec4 ucolor;\n");
            fSource.Add("\n");
            fSource.Add("void main() {\n");
            fSource.Add("gl_FragColor = ucolor;\n");
            fSource.Add("}\n");

            shader = new Shader(vSource, fSource);
        }

        public void Render(float x, float y, float angle, int r, int g, int b, int a, DrawMode drawMode = DrawMode.Polygon, PolygonMode polygonMode = PolygonMode.Filled)
        {
            double cos = System.Math.Cos(MathHelper.ToRadians(angle + 90));
            double sin = System.Math.Sin(MathHelper.ToRadians(angle + 90));

            uint positionAttribLocation = GL.glGetAttribLocation(shader.Id, "aposition");
            uint rotationUniformLocation = GL.glGetUniformLocation(shader.Id, "urotation");
            uint translationUniformLocation = GL.glGetUniformLocation(shader.Id, "utranslation");
            uint resolutionUniformLocation = GL.glGetUniformLocation(shader.Id, "uresolution");
            uint colorUniformLocation = GL.glGetUniformLocation(shader.Id, "ucolor");

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, buffer);
            GL.glVertexAttribPointer(positionAttribLocation, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);
            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
            GL.glUseProgram(shader.Id);
            GL.glUniform2f(translationUniformLocation, x, y);
            GL.glUniform2f(rotationUniformLocation, (float)cos, (float)sin);
            GL.glUniform2f(resolutionUniformLocation, Window.Current.Width, Window.Current.Height);
            GL.glUniform4f(colorUniformLocation, r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);

            if (polygonMode == PolygonMode.Filled) GL.glPolygonMode(GL.GL_FRONT, GL.GL_FILL);
            if (polygonMode == PolygonMode.Lines) GL.glPolygonMode(GL.GL_FRONT, GL.GL_LINE);
            if (polygonMode == PolygonMode.Points) GL.glPolygonMode(GL.GL_FRONT, GL.GL_POINT);

            if (drawMode == DrawMode.Polygon) GL.glDrawArrays(GL.GL_POLYGON, 0, vertices.Length / 2);
            if (drawMode == DrawMode.Lines) GL.glDrawArrays(GL.GL_LINES, 0, vertices.Length / 2);
            if (drawMode == DrawMode.Points) GL.glDrawArrays(GL.GL_POINTS, 0, vertices.Length / 2);
        }

        /* SHAPES */
        public static Polygon Box(float x, float y, float width, float height, float offsetX, float offsetY)
        {
            float[] vertices = new float[8]
            {
                offsetX, offsetY,
                offsetX + width, offsetY,
                offsetX + width, offsetY + height,
                offsetX, offsetY + height
            };

            return new Polygon(vertices);
        }
        public static Polygon RoundedBox(float width, float height, float offsetX, float offsetY, float radius)
        {
            float[] vertices = new float[80];
            int i = 0;

            // Top left
            for (int j = 180; j < 270; j += 10)
            {
                vertices[i + 0] = radius + (MathHelper.MoveX(j) * radius);
                vertices[i + 1] = radius + (MathHelper.MoveY(j) * radius);

                i += 2;
            }

            // Top
            vertices[i + 0] = radius;
            vertices[i + 1] = 0;

            i += 2;

            // Top right
            for (int j = 270; j < 360; j += 10)
            {
                vertices[i + 0] = (width - radius) + (MathHelper.MoveX(j) * radius);
                vertices[i + 1] = radius + (MathHelper.MoveY(j) * radius);

                i += 2;
            }

            // Right
            vertices[i + 0] = width;
            vertices[i + 1] = radius;

            i += 2;

            // Bottom right
            for (var j = 0; j < 90; j += 10)
            {
                vertices[i + 0] = (width - radius) + (MathHelper.MoveX(j) * radius);
                vertices[i + 1] = (height - radius) + (MathHelper.MoveY(j) * radius);

                i += 2;
            }

            // Bottom
            vertices[i + 0] = width - radius;
            vertices[i + 1] = height;

            i += 2;

            // Bottom left
            for (var j = 90; j < 180; j += 10)
            {
                vertices[i + 0] = radius + (MathHelper.MoveX(j) * radius);
                vertices[i + 1] = (height - radius) + (MathHelper.MoveY(j) * radius);

                i += 2;
            }

            // Left
            vertices[i + 0] = 0;
            vertices[i + 1] = height - radius;

            return new Polygon(vertices);
        }
        public static Polygon Circle(int steps, int radius)
        {
            var vertices = new float[steps * 4];
            var i = 0;
            var step = 360 / steps;

            for (int j = 0; j < 360; j += step)
            {
                vertices[i + 0] = MathHelper.MoveX(j + 0) * radius;
                vertices[i + 1] = MathHelper.MoveY(j + 0) * radius;
                vertices[i + 2] = MathHelper.MoveX(j + step) * radius;
                vertices[i + 3] = MathHelper.MoveY(j + step) * radius;

                i += 4;
            }

            return new Polygon(vertices);
        }
    }
}
