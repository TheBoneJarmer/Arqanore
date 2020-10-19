using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Graphics
{
    public static class Shaders
    {
        private static Shader shape;
        private static Shader texture;
        private static Shader glyph;

        public static Shader Default
        {
            get
            {
                if (shape == null)
                {
                    var vSource = new List<string>();
                    vSource.Add("attribute vec2 aposition;\n");
                    vSource.Add("uniform vec2 uresolution;\n");
                    vSource.Add("uniform vec2 urotation;\n");
                    vSource.Add("uniform vec2 utranslation;\n");
                    vSource.Add("uniform vec2 uscale;\n");
                    vSource.Add("\n");
                    vSource.Add("void main() {\n");
                    vSource.Add("vec2 rotatedPosition = vec2(aposition.x * urotation.y + aposition.y * urotation.x, aposition.y * urotation.y - aposition.x * urotation.x);\n");
                    vSource.Add("vec2 position = (rotatedPosition + utranslation) * uscale;\n");
                    vSource.Add("vec2 zeroToOne = position / uresolution;\n");
                    vSource.Add("vec2 zeroToTwo = zeroToOne * 2.0;\n");
                    vSource.Add("vec2 clipSpace = zeroToTwo - 1.0;\n");
                    vSource.Add("gl_Position = vec4(clipSpace.x, -clipSpace.y, 0, 1);\n");
                    vSource.Add("}\n");

                    var fSource = new List<string>();
                    fSource.Add("#version 130\n");
                    fSource.Add("\n");
                    fSource.Add("precision mediump float;\n");
                    fSource.Add("uniform vec4 ucolor;\n");
                    fSource.Add("\n");
                    fSource.Add("void main() {\n");
                    fSource.Add("gl_FragColor = ucolor;\n");
                    fSource.Add("}\n");

                    shape = new Shader(vSource.ToArray(), fSource.ToArray());
                }

                return shape;
            }
        }

        public static Shader Image
        {
            get
            {
                if (texture == null)
                {
                    var vSource = new List<string>();
                    vSource.Add("attribute vec2 aposition;\n");
                    vSource.Add("attribute vec2 atexcoord;\n");
                    vSource.Add("uniform vec2 uresolution;\n");
                    vSource.Add("uniform vec2 urotation;\n");
                    vSource.Add("uniform vec2 utranslation;\n");
                    vSource.Add("uniform vec2 uscale;\n");
                    vSource.Add("varying vec2 vtexcoord;\n");
                    vSource.Add("\n");
                    vSource.Add("void main() {\n");
                    vSource.Add("vec2 rotatedPosition = vec2(aposition.x * urotation.y + aposition.y * urotation.x, aposition.y * urotation.y - aposition.x * urotation.x);\n");
                    vSource.Add("vec2 position = (rotatedPosition + utranslation) * uscale;\n");
                    vSource.Add("vec2 zeroToOne = position / uresolution;\n");
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
                    fSource.Add("gl_FragColor = texture2D(uimage, vtexcoord) * color;\n");
                    fSource.Add("}");

                    texture = new Shader(vSource.ToArray(), fSource.ToArray());
                }

                return texture;
            }
        }

        public static Shader Glyph
        {
            get
            {
                if (glyph == null)
                {
                    var vSource = new List<string>();
                    vSource.Add("attribute vec2 aposition;\n");
                    vSource.Add("attribute vec2 atexcoord;\n");
                    vSource.Add("uniform vec2 uresolution;\n");
                    vSource.Add("uniform vec2 urotation;\n");
                    vSource.Add("uniform vec2 utranslation;\n");
                    vSource.Add("uniform vec2 uscale;\n");
                    vSource.Add("varying vec2 vtexcoord;\n");
                    vSource.Add("\n");
                    vSource.Add("void main() {\n");
                    vSource.Add("vec2 rotatedPosition = vec2(aposition.x * urotation.y + aposition.y * urotation.x, aposition.y * urotation.y - aposition.x * urotation.x);\n");
                    vSource.Add("vec2 position = (rotatedPosition + utranslation) * uscale;\n");
                    vSource.Add("vec2 zeroToOne = position / uresolution;\n");
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

                    glyph = new Shader(vSource.ToArray(), fSource.ToArray());
                }

                return glyph;
            }
        }
    }
}
