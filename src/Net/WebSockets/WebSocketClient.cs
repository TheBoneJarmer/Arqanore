using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Arqanore.Net.Sockets;
using Arqanore.Net.WebSockets;

namespace Arqanore.Net.WebSockets
{
    public class WebSocketClient
    {
        private Thread Thread { get; set; }
        private Socket Socket { get; set; }
        private WebSocketRequest Request { get; set; }
        private WebSocketResponse Response { get; set; }

        public WebSocketStatus Status { get; private set; }

        public WebSocketClient()
        {
            Status = WebSocketStatus.Closed;
            Request = new WebSocketRequest();
            Response = new WebSocketResponse();
        }

        public void Connect(string host, int port)
        {
            try
            {
                // Setup local endpoint
                var ipHostEntry = Dns.GetHostEntry(host);
                var ipAddress = ipHostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
                var ipEndpoint = new IPEndPoint(ipAddress, port);

                // Create the client socket
                Socket = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the client socket
                Socket.Connect(ipEndpoint);

                // Handshake with server
                SendHandshake();
                ReceiveHandshake();
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(ex);
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                var message = new WebSocketMessage(data, WebSocketMessageType.Text);
                message.Encode(true);

                Socket.Send(message.Buffer);
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(ex);
            }
        }

        public void Disconnect()
        {
            try
            {
                var message = new WebSocketMessage("", WebSocketMessageType.CloseConnection);
                message.Encode();

                Socket.Send(message.Buffer);
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Status = WebSocketStatus.Closed;
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(ex);
            }
        }

        private void SendHandshake()
        {
            try
            {
                // Generate the request's key
                Request.Host = "localhost";
                Request.Origin = "localhost";
                Request.Key = Request.GenerateKey();

                // Move the status forward to opening
                Socket.Send(Request.ToHttpRequest());
                Status = WebSocketStatus.Open;
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(ex);
            }
        }
        private void ReceiveHandshake()
        {
            try
            {
                var buffer = new byte[1024];
                var bytesReceived = Socket.Receive(buffer);

                // Again we are going to assume the client received data
                // Not sure what I will do if that is not the case
                if (bytesReceived > 0)
                {
                    // Set the data variable assuming all data was sent in 1 go
                    var data = buffer.Slice(0, bytesReceived);

                    // Parse the response
                    Response.Parse(data);

                    // If all is well, set the status to open
                    Status = WebSocketStatus.Open;

                    // Start the client's thread
                    Thread = new Thread(ThreadCallback);
                    Thread.Start();
                }
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(ex);
            }
        }

        private void ThreadCallback()
        {
            try
            {
                var buffer = new byte[1024];
                var data = new List<byte>();

                // Execute the onconnect event
                OnConnect?.Invoke();

                // Go in a loop to wait for incoming messages
                while (Status == WebSocketStatus.Open)
                {
                    // Wait for data
                    int bytesReceived = Socket.Receive(buffer);

                    while (true)
                    {
                        data.AddRange(buffer.Slice(0, bytesReceived));

                        if (bytesReceived < buffer.Length)
                        {
                            break;
                        }
                    }

                    if (bytesReceived > 0)
                    {
                        var bytes = data.ToArray();

                        while (bytes != null)
                        {
                            WebSocketMessage message = new WebSocketMessage(bytes);
                            bytes = message.Decode();

                            if (message.Type == WebSocketMessageType.Text && OnMessage != null)
                            {
                                OnMessage(message.Message);
                            }
                            if (message.Type == WebSocketMessageType.CloseConnection)
                            {
                                Disconnect();
                            }
                        }
                    }

                    data.Clear();
                }
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Lost;
                OnSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(ex);
            }

            OnDisconnect?.Invoke();
        }

        public delegate void OnConnectDelegate();
        public event OnConnectDelegate OnConnect;
        public delegate void OnMessageDelegate(byte[] message);
        public event OnMessageDelegate OnMessage;
        public delegate void OnErrorDelegate(Exception exception);
        public event OnErrorDelegate OnError;
        public delegate void OnSocketErrorDelegate(SocketException exception);
        public event OnSocketErrorDelegate OnSocketError;
        public delegate void OnDisconnectDelegate();
        public event OnDisconnectDelegate OnDisconnect;
    }
}