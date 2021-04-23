using System;
using System.Collections.Generic;
using System.Text;
using Arqan;

namespace Arqanore.Graphics
{
    public class Shader
    {
        public uint Id { get; set; }

        public Shader(List<string> vertexSource, List<string> fragmentSource) : this(vertexSource.ToArray(), fragmentSource.ToArray())
        {

        }
        public Shader(string[] vertexSource, string[] fragmentSource)
        {
            var program = GL.glCreateProgram();
            var vshader = CompileShader(vertexSource, ShaderType.Vertex);
            var fshader = CompileShader(fragmentSource, ShaderType.Fragment);
            
            GL.glAttachShader(program, vshader);
            GL.glAttachShader(program, fshader);
            GL.glLinkProgram(program);
            GL.glDeleteShader(vshader);
            GL.glDeleteShader(fshader);

            this.Id = program;
        }

        private uint CompileShader(string[] shaderSource, ShaderType shaderType)
        {
            var shader = GL.glCreateShader((uint)shaderType);
            var compileStatus = 0;
            var shaderSourceLength = new int[shaderSource.Length];

            // Calculate lengths
            for (var i=0; i<shaderSource.Length; i++)
            {
                shaderSourceLength[i] = shaderSource[i].Length;
            }

            GL.glShaderSource(shader, shaderSource.Length, shaderSource, shaderSourceLength);
            GL.glCompileShader(shader);
            GL.glGetShaderiv(shader, GL.GL_COMPILE_STATUS, ref compileStatus);

            if (compileStatus == 0)
            {
                var buffer = new byte[2048];
                var bufferSize = 0;

                GL.glGetShaderInfoLog(shader, 2048, ref bufferSize, buffer);
                
                throw new ArqanoreShaderException(shaderType, Encoding.ASCII.GetString(buffer));
            }

            return shader;
        }
    }
}
