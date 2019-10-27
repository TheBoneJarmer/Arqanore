using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arqanore.Graphics;
using Arqanore.Math;
using Arqan;

namespace Arqanore
{
    public static class Draw
    {
        private static uint vbuffer;
        private static uint tcbuffer;
        private static Window gameWindow;

        internal static void Init(Window gameWindow)
        {
            uint[] buffers = new uint[2];
            GL15.glGenBuffers(2, buffers);

            Draw.vbuffer = buffers[0];
            Draw.tcbuffer = buffers[1];
            Draw.gameWindow = gameWindow;
        }

        public static void Box(float x, float y, float width, float height, float angle, float offsetX, float offsetY, int r, int g, int b, int a, PolygonFillMode fillMode)
        {
            float[] vertices = new float[8]
            {
                offsetX, offsetY,
                offsetX + width, offsetY,
                offsetX + width, offsetY + height,
                offsetX, offsetY + height
            };

            Polygon(vertices, x, y, angle, r, g, b, a, fillMode);
        }
        public static void Box(float x, float y, float width, float height, float angle, float offsetX, float offsetY, Color color, PolygonFillMode fillMode)
        {
            Box(x, y, width, height, angle, offsetX, offsetY, color.R, color.G, color.B, color.A, fillMode);
        }
        public static void Box(Rectangle bounds, float angle, float offsetX, float offsetY, Color color, PolygonFillMode fillMode)
        {
            Box(bounds.X, bounds.Y, bounds.Width, bounds.Height, angle, offsetX, offsetY, color, fillMode);
        }
        public static void Box(Rectangle bounds, float angle, Vector2 offset, Color color, PolygonFillMode fillMode)
        {
            Box(bounds, angle, offset.X, offset.Y, color, fillMode);
        }

        public static void RoundedBox(float x, float y, float width, float height, int radius, float angle, int r, int g, int b, int a, PolygonFillMode fillMode)
        {
            // var vertices = new float[(36 * 2) + 8];
            var vertices = new float[80];
            var i = 0;

            // Top left
            for (var j = 180; j < 270; j += 10)
            {
                vertices[i + 0] = radius + (MathHelper.XSpeed(j) * radius);
                vertices[i + 1] = radius + (MathHelper.YSpeed(j) * radius);

                i += 2;
            }

            // Top
            vertices[i + 0] = radius;
            vertices[i + 1] = 0;

            i += 2;

            // Top right
            for (var j = 270; j < 360; j += 10)
            {
                vertices[i + 0] = (width - radius) + (MathHelper.XSpeed(j) * radius);
                vertices[i + 1] = radius + (MathHelper.YSpeed(j) * radius);

                i += 2;
            }

            // Right
            vertices[i + 0] = width;
            vertices[i + 1] = radius;

            i += 2;

            // Bottom right
            for (var j = 0; j < 90; j += 10)
            {
                vertices[i + 0] = (width - radius) + (MathHelper.XSpeed(j) * radius);
                vertices[i + 1] = (height - radius) + (MathHelper.YSpeed(j) * radius);

                i += 2;
            }

            // Bottom
            vertices[i + 0] = width - radius;
            vertices[i + 1] = height;

            i += 2;

            // Bottom left
            for (var j = 90; j < 180; j += 10)
            {
                vertices[i + 0] = radius + (MathHelper.XSpeed(j) * radius);
                vertices[i + 1] = (height - radius) + (MathHelper.YSpeed(j) * radius);

                i += 2;
            }

            // Left
            vertices[i + 0] = 0;
            vertices[i + 1] = height - radius;

            Polygon(vertices, x, y, angle, r, g, b, a, fillMode);
        }

        public static void Circle(float x, float y, int radius, int steps, float angle, int r, int g, int b, int a, PolygonFillMode fillMode)
        {
            var vertices = new float[(360 / steps) * 2];
            var i = 0;

            for (var j = 0; j < 360; j += steps)
            {
                vertices[i + 0] = MathHelper.XSpeed(j) * radius;
                vertices[i + 1] = MathHelper.YSpeed(j) * radius;

                i += 2;
            }

            Polygon(vertices, x, y, angle, r, g, b, a, fillMode);
        }
        public static void Circle(float x, float y, int radius, int steps, float angle, Color color, PolygonFillMode fillMode)
        {
            Circle(x, y, radius, steps, angle, color.R, color.G, color.B, color.A, PolygonFillMode.Filled);
        }

        public static void Line(float x1, float y1, float x2, float y2, int r, int g, int b, int a)
        {
            var positionAttribLocation = GL20.glGetAttribLocation(Shaders.Default.Id, "aposition");
            var rotationUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "urotation");
            var scaleUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "uscale");
            var translationUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "utranslation");
            var resolutionUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "uresolution");
            var colorUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "ucolor");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, 16, new float[4] { x1, y1, x2, y2 }, GL15.GL_STATIC_DRAW);

            GL11.glBindTexture(GL11.GL_TEXTURE_2D, 0);
            GL20.glUseProgram(Shaders.Default.Id);

            GL20.glEnableVertexAttribArray(positionAttribLocation);

            GL20.glUniform2f(translationUniformLocation, 0, 0);
            GL20.glUniform2f(rotationUniformLocation, 0, 1);
            GL20.glUniform2f(scaleUniformLocation, 1.0f, 1.0f);
            GL20.glUniform2f(resolutionUniformLocation, gameWindow.Width, gameWindow.Height);
            GL20.glUniform4f(colorUniformLocation, r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);

            GL11.glDrawArrays(GL11.GL_LINES, 0, 8);
        }

        public static void Polygon(float[] vertices, float x, float y, float angle, int r, int g, int b, int a, PolygonFillMode fillMode)
        {
            var cos = System.Math.Cos(MathHelper.ToRadians(angle + 90));
            var sin = System.Math.Sin(MathHelper.ToRadians(angle + 90));

            var positionAttribLocation = GL20.glGetAttribLocation(Shaders.Default.Id, "aposition");
            var rotationUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "urotation");
            var scaleUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "uscale");
            var translationUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "utranslation");
            var resolutionUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "uresolution");
            var colorUniformLocation = GL20.glGetUniformLocation(Shaders.Default.Id, "ucolor");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, vertices.Length * 4, vertices, GL15.GL_STATIC_DRAW);

            GL11.glBindTexture(GL11.GL_TEXTURE_2D, 0);
            GL20.glUseProgram(Shaders.Default.Id);

            GL20.glEnableVertexAttribArray(positionAttribLocation);

            GL20.glUniform2f(translationUniformLocation, x, y);
            GL20.glUniform2f(rotationUniformLocation, (float)cos, (float)sin);
            GL20.glUniform2f(scaleUniformLocation, 1.0f, 1.0f);
            GL20.glUniform2f(resolutionUniformLocation, gameWindow.Width, gameWindow.Height);
            GL20.glUniform4f(colorUniformLocation, (float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f, (float)a / 255.0f);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);

            if (fillMode == PolygonFillMode.Filled)
            {
                GL10.glPolygonMode(GL11.GL_FRONT_AND_BACK, GL11.GL_FILL);
            }

            if (fillMode == PolygonFillMode.Lines)
            {
                GL10.glPolygonMode(GL11.GL_FRONT_AND_BACK, GL11.GL_LINE);
            }

            if (fillMode == PolygonFillMode.Points)
            {
                GL10.glPolygonMode(GL11.GL_FRONT_AND_BACK, GL11.GL_POINT);
            }

            GL11.glDrawArrays(GL11.GL_POLYGON, 0, vertices.Length / 2);
        }

        public static void Image(Image image, float x, float y, float width, float height, float offsetX, float offsetY, float angle, float clipX, float clipY, float clipWidth, float clipHeight, float scaleX, float scaleY)
        {
            var cos = System.Math.Cos(MathHelper.ToRadians(angle + 90));
            var sin = System.Math.Sin(MathHelper.ToRadians(angle + 90));
            var texCoordX = (1f / (float)image.Width) * clipX;
            var texCoordY = (1f / (float)image.Height) * clipY;
            var texCoordWidth = 1f / ((float)(image.Width) / clipWidth);
            var texCoordHeight = 1f / ((float)(image.Height) / clipHeight);
            var vertices = new float[12] { offsetX, offsetY, offsetX + width, offsetY, offsetX, offsetY + height, offsetX + width, offsetY, offsetX, offsetY + height, offsetX + width, offsetY + height };
            var texcoords = new float[12] { texCoordX, texCoordY, texCoordX + texCoordWidth, texCoordY, texCoordX, texCoordY + texCoordHeight, texCoordX + texCoordWidth, texCoordY, texCoordX, texCoordY + texCoordHeight, texCoordX + texCoordWidth, texCoordY + texCoordHeight };

            var positionAttribLocation = GL20.glGetAttribLocation(Shaders.Image.Id, "aposition");
            var texcoordAttribLocation = GL20.glGetAttribLocation(Shaders.Image.Id, "atexcoord");
            var rotationUniformLocation = GL20.glGetUniformLocation(Shaders.Image.Id, "urotation");
            var scaleUniformLocation = GL20.glGetUniformLocation(Shaders.Image.Id, "uscale");
            var translationUniformLocation = GL20.glGetUniformLocation(Shaders.Image.Id, "utranslation");
            var resolutionUniformLocation = GL20.glGetUniformLocation(Shaders.Image.Id, "uresolution");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, vertices.Length * 4, vertices, GL15.GL_STATIC_DRAW);
            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, tcbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, texcoords.Length * 4, texcoords, GL15.GL_STATIC_DRAW);

            GL11.glBindTexture(GL11.GL_TEXTURE_2D, image.Id);
            GL20.glUseProgram(Shaders.Image.Id);

            GL20.glEnableVertexAttribArray(positionAttribLocation);
            GL20.glEnableVertexAttribArray(texcoordAttribLocation);

            GL20.glUniform2f(translationUniformLocation, x, y);
            GL20.glUniform2f(rotationUniformLocation, (float)cos, (float)sin);
            GL20.glUniform2f(scaleUniformLocation, scaleX, scaleY);
            GL20.glUniform2f(resolutionUniformLocation, gameWindow.Width, gameWindow.Height);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, tcbuffer);
            GL20.glVertexAttribPointer(texcoordAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);

            GL10.glPolygonMode(GL11.GL_FRONT_AND_BACK, GL11.GL_FILL);
            GL11.glDrawArrays(GL11.GL_TRIANGLES, 0, vertices.Length / 2);
        }
        public static void Image(Image image, Rectangle bounds, float offsetX, float offsetY, float angle, float clipX, float clipY, float clipWidth, float clipHeight, float scaleX, float scaleY)
        {
            Image(image, bounds.X, bounds.Y, bounds.Width, bounds.Height, offsetX, offsetY, angle, clipX, clipY, clipWidth, clipHeight, scaleX, scaleY);
        }
        public static void Image(Image image, Rectangle bounds, float offsetX, float offsetY, float angle, Rectangle clip, float scaleX, float scaleY)
        {
            Image(image, bounds, offsetX, offsetY, angle, clip.X, clip.Y, clip.Width, clip.Height, scaleX, scaleY);
        }
        public static void Image(Image image, Rectangle bounds, Vector2 offset, float angle, Rectangle clip, Vector2 scale)
        {
            Image(image, bounds, offset.X, offset.Y, angle, clip, scale.X, scale.Y);
        }

        public static void Text(Font font, string text, float x, float y)
        {

        }
    }
}
