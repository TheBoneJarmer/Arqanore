using System;
using Arqan;

namespace Arqanore
{
    public unsafe static class Device
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