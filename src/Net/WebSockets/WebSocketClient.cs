using System;
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

        public void Send(byte[] data)
        {
            var message = new WebSocketMessage(data, WebSocketMessageType.Text);
            message.Encode(true);

            Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
        }
        public void Send(string data)
        {
            var message = new WebSocketMessage(data, WebSocketMessageType.Text);
            message.Encode(true);

            Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
        }

        public void Disconnect()
        {
            var message = new WebSocketMessage("", WebSocketMessageType.CloseConnection);
            message.Encode();

            Status = WebSocketStatus.Closing;
            Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
        }

        private void SendHandshake()
        {
            // Generate the request's key
            Request.Host = "localhost";
            Request.Origin = "localhost";
            Request.Key = Request.GenerateKey();

            // Move the status forward to opening
            Status = WebSocketStatus.Opening;
            Socket.BeginSend(Request.ToHttpRequest(), 0, Request.ToHttpRequest().Length, SocketFlags.None, SendCallback, Socket);
        }
        private void ReceiveHandshake()
        {
            SocketMessage socketMessage = new SocketMessage();
            socketMessage.Socket = Socket;

            Socket.BeginReceive(socketMessage.Buffer, 0, socketMessage.Buffer.Length, SocketFlags.None, ReceiveCallback, socketMessage);
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;
            handler.EndSend(result);

            // Final step, closing the connection
            if (Status == WebSocketStatus.Closing)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Status = WebSocketStatus.Closed;
            }
        }

        private void ReceiveCallback(IAsyncResult result)
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
                    // Parse the response
                    Response.Parse(socketMessage.ToString());

                    // If all is well, set the status to open
                    Status = WebSocketStatus.Open;

                    // Start the client's thread
                    Thread = new Thread(ThreadCallback);
                    Thread.Start();
                }
            }
        }

        private void ThreadCallback()
        {
            try
            {
                // Create a buffer of 1 MB
                byte[] buffer = new byte[1048576];

                if (this.OnConnect != null)
                {
                    this.OnConnect();
                }

                // Go in a loop to wait for incoming messages
                while (Status == WebSocketStatus.Open)
                {
                    // Wait for data
                    int bytesReceived = Socket.Receive(buffer);

                    if (bytesReceived > 0)
                    {
                        try
                        {
                            WebSocketMessage message = new WebSocketMessage(buffer);
                            message.Decode();

                            if (message.Type == WebSocketMessageType.Text && this.OnMessage != null)
                            {
                                this.OnMessage(message.Message);
                            }
                            if (message.Type == WebSocketMessageType.CloseConnection)
                            {
                                if (Status == WebSocketStatus.Open)
                                {
                                    Disconnect();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (this.OnError != null)
                            {
                                this.OnError(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Status = WebSocketStatus.Closed;
            }

            if (this.OnDisconnect != null)
            {
                this.OnDisconnect();
            }
        }

        public delegate void OnConnectDelegate();
        public event OnConnectDelegate OnConnect;
        public delegate void OnMessageDelegate(byte[] message);
        public event OnMessageDelegate OnMessage;
        public delegate void OnErrorDelegate(Exception exception);
        public event OnErrorDelegate OnError;
        public delegate void OnDisconnectDelegate();
        public event OnDisconnectDelegate OnDisconnect;
    }
}