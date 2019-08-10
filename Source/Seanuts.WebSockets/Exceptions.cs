using System;
using System.Net;

namespace Seanuts.WebSockets
{
    public class WebSocketException : Exception
    {
        public WebSocketException(string message) : base(message)
        {

        }
    }
}