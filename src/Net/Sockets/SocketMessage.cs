using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Arqanore.Net.Sockets
{
    public class SocketMessage
    {
        public Socket Socket { get; set; }
        public byte[] Buffer { get; set; }
        public byte[] Data { get; set; }

        public SocketMessage()
        {
            // Create a buffer of 4 MB
            this.Buffer = new byte[4194304];
            this.Data = new byte[0];
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(Data);
        }
    }
}