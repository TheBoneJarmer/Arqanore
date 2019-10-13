using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Seanuts
{
    public class SNWebSocketException : Exception
    {
        public SNWebSocketException(string message) : base(message)
        {

        }
    }
    public class SNHttpException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ResponseText { get; private set; }

        public SNHttpException(HttpStatusCode status, string responseText) : base(status.ToString() + ": " + responseText)
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
    public class SeanutsException : Exception
    {
        public SeanutsException(string message) : base(message)
        {
            
        }
    }
}
