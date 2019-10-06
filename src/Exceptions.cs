using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Seanuts
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
        public GLFWException(string message) : base(message)
        {

        }
    }
}
