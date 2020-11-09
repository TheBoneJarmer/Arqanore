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

        public Polygon(float[] vertices)
        {
            this.vertices = vertices;

            GenerateShader();
            GenerateVBO(vertices);
        }

        private void GenerateVBO(float[] vertices)
        {
            uint[] buffers = new uint[1];
            GL15.glGenBuffers(1, buffers);
            buffer = buffers[0];

            uint positionAttribLocation = GL20.glGetAttribLocation(shader.Id, "aposition");

            GL20.glEnableVertexAttribArray(positionAttribLocation);
            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, buffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, vertices.Length * 4, vertices, GL15.GL_STATIC_DRAW);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);
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

        public void Render(Vector2 position, float angle, Color color, DrawMode drawMode = DrawMode.Polygon, PolygonMode polygonMode = PolygonMode.Filled)
        {
            Render(position.X, position.Y, angle, color.R, color.G, color.B, color.A, drawMode, polygonMode);
        }
        public void Render(float x, float y, float angle, int r, int g, int b, int a, DrawMode drawMode = DrawMode.Polygon, PolygonMode polygonMode = PolygonMode.Filled)
        {
            double cos = System.Math.Cos(MathHelper.ToRadians(angle + 90));
            double sin = System.Math.Sin(MathHelper.ToRadians(angle + 90));

            uint positionAttribLocation = GL20.glGetAttribLocation(shader.Id, "aposition");
            uint rotationUniformLocation = GL20.glGetUniformLocation(shader.Id, "urotation");
            uint translationUniformLocation = GL20.glGetUniformLocation(shader.Id, "utranslation");
            uint resolutionUniformLocation = GL20.glGetUniformLocation(shader.Id, "uresolution");
            uint colorUniformLocation = GL20.glGetUniformLocation(shader.Id, "ucolor");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, buffer);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);
            GL11.glBindTexture(GL11.GL_TEXTURE_2D, 0);
            GL20.glUseProgram(shader.Id);
            GL20.glUniform2f(translationUniformLocation, x, y);
            GL20.glUniform2f(rotationUniformLocation, (float)cos, (float)sin);
            GL20.glUniform2f(resolutionUniformLocation, Window.Current.Width, Window.Current.Height);
            GL20.glUniform4f(colorUniformLocation, r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);

            if (polygonMode == PolygonMode.Filled) GL10.glPolygonMode(GL11.GL_FRONT, GL11.GL_FILL);
            if (polygonMode == PolygonMode.Lines) GL10.glPolygonMode(GL11.GL_FRONT, GL11.GL_LINE);
            if (polygonMode == PolygonMode.Points) GL10.glPolygonMode(GL11.GL_FRONT, GL11.GL_POINT);

            if (drawMode == DrawMode.Polygon) GL11.glDrawArrays(GL11.GL_POLYGON, 0, vertices.Length / 2);
            if (drawMode == DrawMode.Lines) GL11.glDrawArrays(GL11.GL_LINES, 0, vertices.Length / 2);
            if (drawMode == DrawMode.Points) GL11.glDrawArrays(GL11.GL_POINTS, 0, vertices.Length / 2);
        }

        /* SHAPES */
        public static Polygon Box(Vector2 size, Vector2 offset)
        {
            float[] vertices = new float[8]
            {
                offset.X, offset.Y,
                offset.X + size.X, offset.Y,
                offset.X + size.X, offset.Y + size.Y,
                offset.X, offset.Y + size.Y
            };

            return new Polygon(vertices);
        }
        public static Polygon RoundedBox(Vector2 size, Vector2 offset, float radius)
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
                vertices[i + 0] = (size.X - radius) + (MathHelper.MoveX(j) * radius);
                vertices[i + 1] = radius + (MathHelper.MoveY(j) * radius);

                i += 2;
            }

            // Right
            vertices[i + 0] = size.X;
            vertices[i + 1] = radius;

            i += 2;

            // Bottom right
            for (var j = 0; j < 90; j += 10)
            {
                vertices[i + 0] = (size.X - radius) + (MathHelper.MoveX(j) * radius);
                vertices[i + 1] = (size.Y - radius) + (MathHelper.MoveY(j) * radius);

                i += 2;
            }

            // Bottom
            vertices[i + 0] = size.X - radius;
            vertices[i + 1] = size.Y;

            i += 2;

            // Bottom left
            for (var j = 90; j < 180; j += 10)
            {
                vertices[i + 0] = radius + (MathHelper.MoveX(j) * radius);
                vertices[i + 1] = (size.Y - radius) + (MathHelper.MoveY(j) * radius);

                i += 2;
            }

            // Left
            vertices[i + 0] = 0;
            vertices[i + 1] = size.Y - radius;

            return new Polygon(vertices);
        }
        public static Polygon Circle(int steps, int radius)
        {
            float[] vertices = new float[(360 / steps) * 2];
            int i = 0;

            for (int j = 0; j < 360; j += steps)
            {
                vertices[i + 0] = MathHelper.MoveX(j) * radius;
                vertices[i + 1] = MathHelper.MoveY(j) * radius;

                i += 2;
            }

            return new Polygon(vertices);
        }
    }
}
