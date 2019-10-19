using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seanuts.Graphics;
using Seanuts.Math;
using TilarGL;

namespace Seanuts
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
            GL20.glUniform4f(colorUniformLocation, r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);

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

        public static void Text(Font font, string text, float x, float y)
        {
            var xx = x;
            var yy = y;
            var width = font.Size * 2;
            var height = font.Size * 2;

            for (var i = 0; i < text.Length; i++)
            {
                var chr = (char)text[i];
                var code = (byte)chr;
                var bounds = font.Bounds[(int)code];

                Draw.Image(font.Image, xx, yy - bounds.Height, bounds.Width, bounds.Height, 0, 0, 0, bounds.X, bounds.Y, bounds.Width, bounds.Height, 1, 1);

                xx += bounds.Width;
            }
        }
    }
}
