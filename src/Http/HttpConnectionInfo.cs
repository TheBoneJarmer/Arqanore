using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Seanuts.Http
{
    public class HttpConnectionInfo
    {
        private Socket Socket { get; set; }

        public IPAddress IPAddress
        {
            get { return ((IPEndPoint)(Socket.RemoteEndPoint)).Address; }
        }
        public int Port
        {
            get { return ((IPEndPoint)(Socket.RemoteEndPoint)).Port; }
        }

        public HttpConnectionInfo(Socket socket)
        {
            this.Socket = socket;
        }
    }
}