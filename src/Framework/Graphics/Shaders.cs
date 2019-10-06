using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Framework.Graphics
{
    public static class Shaders
    {
        private static Shader @default;
        private static Shader sprite;
        private static Shader background;

        public static Shader Default
        {
            get
            {
                if (@default == null)
                {
                    var vSource = new List<string>();
                    vSource.Add("attribute vec2 aposition;");
                    vSource.Add("uniform vec2 uresolution;");
                    vSource.Add("uniform vec2 urotation;");
                    vSource.Add("uniform vec2 utranslation;");
                    vSource.Add("uniform vec2 uscale;");
                    vSource.Add("");
                    vSource.Add("void main() {");
                    vSource.Add("vec2 rotatedPosition = vec2(aposition.x * urotation.y + aposition.y * urotation.x, aposition.y * urotation.y - aposition.x * urotation.x);");
                    vSource.Add("vec2 position = (rotatedPosition + utranslation) * uscale;");
                    vSource.Add("vec2 zeroToOne = position / uresolution;");
                    vSource.Add("vec2 zeroToTwo = zeroToOne * 2.0;");
                    vSource.Add("vec2 clipSpace = zeroToTwo - 1.0;");
                    vSource.Add("gl_Position = vec4(clipSpace.x, -clipSpace.y, 0, 1);");
                    vSource.Add("}");

                    var fSource = new List<string>();
                    fSource.Add("precision mediump float;");
                    fSource.Add("uniform vec4 ucolor;");
                    fSource.Add("void main() {");
                    fSource.Add("vec4 finalcolor = ucolor;");
                    fSource.Add("gl_FragColor = finalcolor;");
                    fSource.Add("}");

                    @default = new Shader(vSource.ToArray(), fSource.ToArray());
                }

                return @default;
            }
        }

        public static Shader Sprite
        {
            get
            {
                if (sprite == null)
                {
                    var vSource = new List<string>();
                    vSource.Add("attribute vec2 aposition;");
                    vSource.Add("attribute vec2 atexcoord;");
                    vSource.Add("uniform vec2 uresolution;");
                    vSource.Add("uniform vec2 urotation;");
                    vSource.Add("uniform vec2 utranslation;");
                    vSource.Add("uniform vec2 uscale;");
                    vSource.Add("varying vec2 vtexcoord;");
                    vSource.Add("");
                    vSource.Add("void main() {");
                    vSource.Add("vec2 rotatedPosition = vec2(aposition.x * urotation.y + aposition.y * urotation.x, aposition.y * urotation.y - aposition.x * urotation.x);");
                    vSource.Add("vec2 position = (rotatedPosition + utranslation) * uscale;");
                    vSource.Add("vec2 zeroToOne = position / uresolution;");
                    vSource.Add("vec2 zeroToTwo = zeroToOne * 2.0;");
                    vSource.Add("vec2 clipSpace = zeroToTwo - 1.0;");
                    vSource.Add("");
                    vSource.Add("vtexcoord = atexcoord;");
                    vSource.Add("");
                    vSource.Add("gl_Position = vec4(clipSpace.x, -clipSpace.y, 0, 1);");
                    vSource.Add("}");

                    var fSource = new List<string>();
                    fSource.Add("precision mediump float;");
                    fSource.Add("uniform sampler2D uimage;");
                    fSource.Add("varying vec2 vtexcoord;");
                    fSource.Add("void main() {");
                    fSource.Add("gl_FragColor = texture2D(uimage, vtexcoord);");
                    fSource.Add("}");

                    sprite = new Shader(vSource.ToArray(), fSource.ToArray());
                }

                return sprite;
            }
        }

        public static Shader Background
        {
            get
            {
                if (background == null)
                {
                    var vSource = new List<string>();
                    vSource.Add("attribute vec2 aposition;");
                    vSource.Add("attribute vec2 atexcoord;");
                    vSource.Add("uniform vec2 uresolution;");
                    vSource.Add("uniform vec2 urotation;");
                    vSource.Add("uniform vec2 utranslation;");
                    vSource.Add("uniform vec2 uscale;");
                    vSource.Add("varying vec2 vtexcoord;");
                    vSource.Add("");
                    vSource.Add("void main() {");
                    vSource.Add("vec2 rotatedPosition = vec2(aposition.x * urotation.y + aposition.y * urotation.x, aposition.y * urotation.y - aposition.x * urotation.x);");
                    vSource.Add("vec2 position = (rotatedPosition + utranslation) * uscale;");
                    vSource.Add("vec2 zeroToOne = position / uresolution;");
                    vSource.Add("vec2 zeroToTwo = zeroToOne * 2.0;");
                    vSource.Add("vec2 clipSpace = zeroToTwo - 1.0;");
                    vSource.Add("");
                    vSource.Add("vtexcoord = atexcoord;");
                    vSource.Add("");
                    vSource.Add("gl_Position = vec4(clipSpace.x, -clipSpace.y, 0, 1);");
                    vSource.Add("}");

                    var fSource = new List<string>();
                    fSource.Add("precision mediump float;");
                    fSource.Add("uniform sampler2D uimage;");
                    fSource.Add("varying vec2 vtexcoord;");
                    fSource.Add("void main() {");
                    fSource.Add("gl_FragColor = texture2D(uimage, vtexcoord);");
                    fSource.Add("}");

                    background = new Shader(vSource.ToArray(), fSource.ToArray());
                }

                return background;
            }
        }
    }
}
