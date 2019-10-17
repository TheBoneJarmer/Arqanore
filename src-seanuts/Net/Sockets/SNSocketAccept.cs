using System;
using System.Net.Sockets;
using System.Threading;

namespace Seanuts.Net.Sockets
{
    public class SNSocketAccept
    {
        public Socket Socket { get; set; }
        public ManualResetEvent ResetEvent { get; set; }
    }
}