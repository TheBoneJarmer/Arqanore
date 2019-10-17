using Seanuts.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Seanuts.Net.Http
{
    public class SNHttpClient
    {
        private string Host { get; set; }
        private int Port { get; set; }

        public SNHttpClient()
        {

        }

        public void Connect(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }

        public SNHttpClientResponse Send(SNHttpClientRequest request)
        {
            var bytes = Send(request.ToString());
            var response = new SNHttpClientResponse(bytes);

            return response;
        }

        private byte[] Send(string data)
        {
            return Send(Encoding.ASCII.GetBytes(data));
        }
        private byte[] Send(byte[] data)
        {
            var buffer = new byte[4096];
            var response = new List<byte>();
            var bytesSent = 0;
            var bytesReceived = 0;

            // Setup local endpoint
            var ipHostEntry = Dns.GetHostEntry(Host);
            var ipAddress = ipHostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            var ipEndpoint = new IPEndPoint(ipAddress, Port);

            // Create the client socket
            var socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect the client socket
            socket.Connect(ipEndpoint);

            // Send the data and receive the response
            bytesSent = socket.Send(data);
            bytesReceived = socket.Receive(buffer);

            response.AddRange(buffer.ToList().GetRange(0, bytesReceived));

            // Wait for a response
            while (bytesReceived == buffer.Length)
            {
                bytesReceived = socket.Receive(buffer);
                response.AddRange(buffer.ToList().GetRange(0, bytesReceived));
            }

            // Return the response
            return response.ToArray();
        }
    }
}
