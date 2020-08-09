using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Arqanore
{
    public class WebSocketException : Exception
    {
        public WebSocketException(string message) : base(message)
        {

        }
    }
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ResponseText { get; private set; }

        public HttpException(HttpStatusCode status, string responseText) : base(status.ToString() + ": " + responseText)
        {
            this.StatusCode = status;
            this.ResponseText = responseText;
        }
    }
    public class GLFWException : Exception
    {
        public int Code { get; private set; }
        public string Description { get; private set; }

        public GLFWException(int code, string description) : base(code + ":" + description)
        {
            this.Code = code;
            this.Description = description;
        }
    }
    public class ArqanoreException : Exception
    {
        public ArqanoreException(string message) : base(message)
        {
            
        }
    }
}
