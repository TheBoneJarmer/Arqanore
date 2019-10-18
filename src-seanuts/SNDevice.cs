using System;
using TilarGL;

namespace Seanuts
{
    public unsafe static class SNDevice
    {
        public static string GLVersion
        {
            get { return new string(GL10.glGetString(GL11.GL_VERSION)); }
        }
        public static string GLSLVersion
        {
            get { return new string(GL10.glGetString(GL20.GL_SHADING_LANGUAGE_VERSION)); }
        }
    }
}