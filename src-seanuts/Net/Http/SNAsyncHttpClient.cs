using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Seanuts.Net.Sockets;

namespace Seanuts.Net.Http
{
    public class SNAsyncHttpClient
    {
        private string Host { get; set; }
        private int Port { get; set; }

        public SNAsyncHttpClient()
        {

        }

        public void Connect(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }

        public void Send(SNHttpClientRequest request)
        {
            Send(request.ToString());
        }

        private void Send(string data)
        {
            Send(Encoding.ASCII.GetBytes(data));
        }
        private void Send(byte[] data)
        {
            // Setup local endpoint
            var ipHostEntry = Dns.GetHostEntry(Host);
            var ipAddress = ipHostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            var ipEndpoint = new IPEndPoint(ipAddress, Port);

            // Create the client socket
            var socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect the client socket
            socket.Connect(ipEndpoint);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, socket);
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;
            handler.EndSend(result);

            SNSocketMessage message = new SNSocketMessage();
            message.Socket = handler;

            handler.BeginReceive(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, ReceiveCallback, message);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            // Retrieve the package
            SNSocketMessage socketMessage = (SNSocketMessage)result.AsyncState;
            Socket handler = socketMessage.Socket;

            // Read it
            int bytesRead = handler.EndReceive(result);

            if (bytesRead > 0)
            {
                // Complete the package's data
                socketMessage.Data = socketMessage.Data.Push(socketMessage.Buffer.Slice(0, bytesRead));

                // Continue until all data is received
                if (bytesRead == socketMessage.Buffer.Length)
                {
                    handler.BeginReceive(socketMessage.Buffer, 0, socketMessage.Buffer.Length, SocketFlags.None, ReceiveCallback, socketMessage);
                }
                else
                {
                    try
                    {
                        var response = new SNHttpClientResponse(socketMessage.Data);

                        if (OnResponse != null)
                        {
                            OnResponse(response);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (OnError != null)
                        {
                            OnError(ex);
                        }
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
        }

        /* EVENTS */
        public delegate void OnResponseHandler(SNHttpClientResponse response);
        public delegate void OnErrorHandler(Exception ex);
        public event OnResponseHandler OnResponse;
        public event OnErrorHandler OnError;
    }
}
