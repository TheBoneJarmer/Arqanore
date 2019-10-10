using System;
using System.Collections.Generic;
using System.Text;
using TilarGL;

namespace Seanuts.Framework.Graphics
{
    public class Shader
    {
        public uint Id { get; set; }

        public Shader(string[] vertexSource, string[] fragmentSource)
        {
            var program = GL20.glCreateProgram();
            var vshader = Compile(vertexSource, GL20.GL_VERTEX_SHADER);
            var fshader = Compile(fragmentSource, GL20.GL_FRAGMENT_SHADER);

            GL20.glAttachShader(program, vshader);
            GL20.glAttachShader(program, fshader);
            GL20.glLinkProgram(program);
            GL20.glDeleteShader(vshader);
            GL20.glDeleteShader(fshader);

            this.Id = program;
        }

        private uint Compile(string[] shaderSource, uint shaderType)
        {
            var shader = GL20.glCreateShader(shaderType);
            var compiled = 0;
            var shaderSourceLength = new int[shaderSource.Length];

            // Calculate lengths
            for (var i=0; i<shaderSource.Length; i++)
            {
                shaderSourceLength[i] = shaderSource[i].Length;
            }

            GL20.glShaderSource(shader, shaderSource.Length, shaderSource, shaderSourceLength);
            GL20.glCompileShader(shader);
            GL20.glGetShaderiv(shader, GL20.GL_COMPILE_STATUS, ref compiled);

            if (compiled == 0)
            {
                var buffer = new byte[2048];
                var bufferSize = 0;

                GL20.glGetShaderInfoLog(shader, 2048, ref bufferSize, buffer);

                throw new Exception(Encoding.ASCII.GetString(buffer));
            }

            return shader;
        }
    }
}
