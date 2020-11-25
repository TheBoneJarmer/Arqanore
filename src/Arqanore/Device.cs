using System;
using Arqan;

namespace Arqanore
{
    public unsafe static class Device
    {
        public static string GLVersion
        {
            get { return new string(GL.glGetString(GL.GL_VERSION)); }
        }
        public static string GLSLVersion
        {
            get { return new string(GL.glGetString(GL.GL_SHADING_LANGUAGE_VERSION)); }
        }
    }
}