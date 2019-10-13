using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seanuts.Framework.Graphics;
using Seanuts.Framework.Math;
using TilarGL;

namespace Seanuts.Framework
{
    public static class SNDraw
    {
        private static uint vbuffer;
        private static uint tcbuffer;
        private static SNWindow gameWindow;

        internal static void Init(SNWindow gameWindow)
        {
            uint[] buffers = new uint[2];
            GL15.glGenBuffers(2, buffers);

            SNDraw.vbuffer = buffers[0];
            SNDraw.tcbuffer = buffers[1];
            SNDraw.gameWindow = gameWindow;
        }

        public static void Box(float x, float y, float width, float height, float angle, float offsetX, float offsetY, int r, int g, int b, int a, SNPolygonFillMode fillMode)
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
            var positionAttribLocation = GL20.glGetAttribLocation(SNShaders.Default.Id, "aposition");
            var rotationUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "urotation");
            var scaleUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "uscale");
            var translationUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "utranslation");
            var resolutionUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "uresolution");
            var colorUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "ucolor");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, 16, new float[4] { x1, y1, x2, y2 }, GL15.GL_STATIC_DRAW);

            GL11.glBindTexture(GL11.GL_TEXTURE_2D, 0);
            GL20.glUseProgram(SNShaders.Default.Id);

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

        public static void Polygon(float[] vertices, float x, float y, float angle, int r, int g, int b, int a, SNPolygonFillMode fillMode)
        {
            var cos = System.Math.Cos(SNMath.ToRadians(angle + 90));
            var sin = System.Math.Sin(SNMath.ToRadians(angle + 90));

            var positionAttribLocation = GL20.glGetAttribLocation(SNShaders.Default.Id, "aposition");
            var rotationUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "urotation");
            var scaleUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "uscale");
            var translationUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "utranslation");
            var resolutionUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "uresolution");
            var colorUniformLocation = GL20.glGetUniformLocation(SNShaders.Default.Id, "ucolor");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, vertices.Length * 4, vertices, GL15.GL_STATIC_DRAW);

            GL11.glBindTexture(GL11.GL_TEXTURE_2D, 0);
            GL20.glUseProgram(SNShaders.Default.Id);

            GL20.glEnableVertexAttribArray(positionAttribLocation);

            GL20.glUniform2f(translationUniformLocation, x, y);
            GL20.glUniform2f(rotationUniformLocation, (float)cos, (float)sin);
            GL20.glUniform2f(scaleUniformLocation, 1.0f, 1.0f);
            GL20.glUniform2f(resolutionUniformLocation, gameWindow.Width, gameWindow.Height);
            GL20.glUniform4f(colorUniformLocation, r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);

            if (fillMode == SNPolygonFillMode.Filled)
            {
                GL10.glPolygonMode(GL11.GL_FRONT_AND_BACK, GL11.GL_FILL);
            }

            if (fillMode == SNPolygonFillMode.Lines)
            {
                GL10.glPolygonMode(GL11.GL_FRONT_AND_BACK, GL11.GL_LINE);
            }

            if (fillMode == SNPolygonFillMode.Points)
            {
                GL10.glPolygonMode(GL11.GL_FRONT_AND_BACK, GL11.GL_POINT);
            }

            GL11.glDrawArrays(GL11.GL_POLYGON, 0, vertices.Length / 2);
        }

        public static void Background(SNBackground background, float x, float y, float width, float height, float scaleX, float scaleY, float angle)
        {
            var cos = System.Math.Cos(SNMath.ToRadians(angle + 90));
            var sin = System.Math.Sin(SNMath.ToRadians(angle + 90));
            var vertices = new float[12] { 0, 0, width, 0, 0, height, width, 0, 0, height, width, height };
            var texcoords = new float[12] { 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1 };

            var positionAttribLocation = GL20.glGetAttribLocation(SNShaders.Background.Id, "aposition");
            var texcoordAttribLocation = GL20.glGetAttribLocation(SNShaders.Background.Id, "atexcoord");
            var rotationUniformLocation = GL20.glGetUniformLocation(SNShaders.Background.Id, "urotation");
            var scaleUniformLocation = GL20.glGetUniformLocation(SNShaders.Background.Id, "uscale");
            var translationUniformLocation = GL20.glGetUniformLocation(SNShaders.Background.Id, "utranslation");
            var resolutionUniformLocation = GL20.glGetUniformLocation(SNShaders.Background.Id, "uresolution");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, vertices.Length * 4, vertices, GL15.GL_STATIC_DRAW);
            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, tcbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, texcoords.Length * 4, texcoords, GL15.GL_STATIC_DRAW);

            GL11.glBindTexture(GL11.GL_TEXTURE_2D, background.Id);
            GL20.glUseProgram(SNShaders.Background.Id);

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

        public static void Tileset(SNTileset tileset, float x, float y, float width, float height, int tileX, int tileY, int tileWidth, int tileHeight, float scaleX, float scaleY)
        {
            var tcx = (1 / (tileset.Width / tileWidth)) * tileX;
            var tcy = (1 / (tileset.Height / tileHeight)) * tileY;
            var difx = 1 / (tileset.Width / tileWidth);
            var dify = 1 / (tileset.Height / tileHeight);
            var vertices = new float[12] { 0, 0, width, 0, 0, height, width, 0, 0, height, width, height };
            var texcoords = new float[12] { tcx, tcy, tcx + difx, tcy, tcx, tcy + dify, tcx + difx, tcy, tcx, tcy + dify, tcx + difx, tcy + dify };

            var positionAttribLocation = GL20.glGetAttribLocation(SNShaders.Sprite.Id, "aposition");
            var texcoordAttribLocation = GL20.glGetAttribLocation(SNShaders.Sprite.Id, "atexcoord");
            var rotationUniformLocation = GL20.glGetUniformLocation(SNShaders.Sprite.Id, "urotation");
            var scaleUniformLocation = GL20.glGetUniformLocation(SNShaders.Sprite.Id, "uscale");
            var translationUniformLocation = GL20.glGetUniformLocation(SNShaders.Sprite.Id, "utranslation");
            var resolutionUniformLocation = GL20.glGetUniformLocation(SNShaders.Sprite.Id, "uresolution");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, vertices.Length * 4, vertices, GL15.GL_STATIC_DRAW);
            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, tcbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, texcoords.Length * 4, texcoords, GL15.GL_STATIC_DRAW);

            GL11.glBindTexture(GL11.GL_TEXTURE_2D, tileset.Id);
            GL20.glUseProgram(SNShaders.Sprite.Id);

            GL20.glEnableVertexAttribArray(positionAttribLocation);
            GL20.glEnableVertexAttribArray(texcoordAttribLocation);

            GL20.glUniform2f(translationUniformLocation, x, y);
            GL20.glUniform2f(rotationUniformLocation, 0, 1);
            GL20.glUniform2f(scaleUniformLocation, scaleX, scaleY);
            GL20.glUniform2f(resolutionUniformLocation, gameWindow.Width, gameWindow.Height);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL20.glVertexAttribPointer(positionAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, tcbuffer);
            GL20.glVertexAttribPointer(texcoordAttribLocation, 2, GL11.GL_FLOAT, false, 0, IntPtr.Zero);

            GL10.glPolygonMode(GL11.GL_FRONT_AND_BACK, GL11.GL_FILL);
            GL11.glDrawArrays(GL11.GL_TRIANGLES, 0, vertices.Length / 2);
        }

        public static void Sprite(SNSprite sprite, float x, float y, float width, float height, float offsetX, float offsetY, float angle, int framehor, int framevert, int framewidth, int frameheight, float scaleX, float scaleY)
        {
            var cos = System.Math.Cos(SNMath.ToRadians(angle + 90));
            var sin = System.Math.Sin(SNMath.ToRadians(angle + 90));
            var tcx = (1.0f / ((float)sprite.Width / (float)framewidth)) * (float)framehor;
            var tcy = (1.0f / ((float)sprite.Height / (float)frameheight)) * (float)framevert;
            var difx = 1.0f / (float)(sprite.Width / (float)framewidth);
            var dify = 1.0f / ((float)sprite.Height / (float)frameheight);
            var vertices = new float[12] { offsetX, offsetY, offsetX + width, offsetY, offsetX, offsetY + height, offsetX + width, offsetY, offsetX, offsetY + height, offsetX + width, offsetY + height };
            var texcoords = new float[12] { tcx, tcy, tcx + difx, tcy, tcx, tcy + dify, tcx + difx, tcy, tcx, tcy + dify, tcx + difx, tcy + dify };

            var positionAttribLocation = GL20.glGetAttribLocation(SNShaders.Sprite.Id, "aposition");
            var texcoordAttribLocation = GL20.glGetAttribLocation(SNShaders.Sprite.Id, "atexcoord");
            var rotationUniformLocation = GL20.glGetUniformLocation(SNShaders.Sprite.Id, "urotation");
            var scaleUniformLocation = GL20.glGetUniformLocation(SNShaders.Sprite.Id, "uscale");
            var translationUniformLocation = GL20.glGetUniformLocation(SNShaders.Sprite.Id, "utranslation");
            var resolutionUniformLocation = GL20.glGetUniformLocation(SNShaders.Sprite.Id, "uresolution");

            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, vbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, vertices.Length * 4, vertices, GL15.GL_STATIC_DRAW);
            GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, tcbuffer);
            GL15.glBufferData(GL15.GL_ARRAY_BUFFER, texcoords.Length * 4, texcoords, GL15.GL_STATIC_DRAW);

            GL11.glBindTexture(GL11.GL_TEXTURE_2D, sprite.Id);
            GL20.glUseProgram(SNShaders.Sprite.Id);

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
    }
}
