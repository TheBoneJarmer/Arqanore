using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Seanuts.Sockets
{
    public class SNSocketMessage
    {
        public Socket Socket { get; set; }
        public byte[] Buffer { get; set; }
        public byte[] Data { get; set; }

        public SNSocketMessage()
        {
            this.Buffer = new byte[4096];
            this.Data = new byte[0];
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(Data);
        }
    }
}