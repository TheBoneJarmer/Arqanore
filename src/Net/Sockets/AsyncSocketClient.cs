using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Arqanore.Net.Sockets
{
    public class AsyncSocketClient
    {
        private string Host { get; set; }
        private int Port { get; set; }

        public AsyncSocketClient()
        {
            
        }

        public void Connect(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }

        public void Send(string data)
        {
            Send(Encoding.ASCII.GetBytes(data));
        }
        public void Send(byte[] data)
        {
            try
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
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;
            handler.EndSend(result);

            SocketMessage message = new SocketMessage();
            message.Socket = handler;

            handler.BeginReceive(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, ReceiveCallback, message);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                // Retrieve the package
                SocketMessage socketMessage = (SocketMessage)result.AsyncState;
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
                        if (OnMessage != null)
                        {
                            OnMessage(socketMessage.Data);
                        }

                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        /* EVENTS */
        public delegate void OnMessageHandler(byte[] message);
        public event OnMessageHandler OnMessage;
    }
}
