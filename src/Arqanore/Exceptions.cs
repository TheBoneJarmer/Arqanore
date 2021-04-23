using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Arqanore
{
    public class ArqanoreException : Exception
    {
        public ArqanoreException(string message) : base(message)
        {
            
        }
        
        public ArqanoreException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }

    public class ArqanoreShaderException : ArqanoreException
    {
        public ShaderType ShaderType { get; private set; }
        
        public ArqanoreShaderException(ShaderType shaderType, string message) : base(message)
        {
            ShaderType = shaderType;
        }

        public ArqanoreShaderException(ShaderType shaderType, string message, Exception inner) : base(message, inner)
        {
            ShaderType = shaderType;
        }
    }

    public class ArqanoreGlfwException : ArqanoreException
    {
        public int ErrorCode { get; private set; }
        public string ErrorDescription { get; private set; }
        
        public ArqanoreGlfwException(int errorCode, string errorDescription) : base("A GLFW error occurred.")
        {
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }

        public ArqanoreGlfwException(int errorCode, string errorDescription, Exception inner) : base("A GLFW error occurred", inner)
        {
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }
    }
}
