using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Seanuts.Sockets;
using Seanuts.WebSockets;

namespace Seanuts.WebSockets
{
    public class SNAsyncWebSocketClient
    {
        private Thread Thread { get; set; }
        private Socket Socket { get; set; }
        private SNWebSocketRequest Request { get; set; }
        private SNWebSocketResponse Response { get; set; }

        public SNWebSocketStatus Status { get; private set; }

        public SNAsyncWebSocketClient()
        {
            Status = SNWebSocketStatus.None;
            Request = new SNWebSocketRequest();
            Response = new SNWebSocketResponse();
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

        public void Send(string data)
        {
            SNWebSocketMessage message = new SNWebSocketMessage(data, SNWebSocketMessageType.Text);
            message.Encode(true);

            Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
        }

        public void Disconnect()
        {
            SNWebSocketMessage message = new SNWebSocketMessage("", SNWebSocketMessageType.CloseConnection);
            message.Encode();

            Status = SNWebSocketStatus.Closing;
            Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
        }

        private void SendHandshake()
        {
            // Generate the request's key
            Request.Host = "localhost";
            Request.Origin = "localhost";
            Request.Key = Request.GenerateKey();

            // Move the status forward to opening
            Status = SNWebSocketStatus.Opening;
            Socket.BeginSend(Request.ToHttpRequest(), 0, Request.ToHttpRequest().Length, SocketFlags.None, SendCallback, Socket);
        }
        private void ReceiveHandshake()
        {
            SNSocketMessage socketMessage = new SNSocketMessage();
            socketMessage.Socket = Socket;

            Socket.BeginReceive(socketMessage.Buffer, 0, socketMessage.Buffer.Length, SocketFlags.None, ReceiveCallback, socketMessage);
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;
            handler.EndSend(result);

            // Final step, closing the connection
            if (Status == SNWebSocketStatus.Closing)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Status = SNWebSocketStatus.Closed;
            }
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
                    // Parse the response
                    Response.Parse(socketMessage.ToString());

                    // If all is well, set the status to open
                    Status = SNWebSocketStatus.Open;

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
                byte[] buffer = new byte[1024];

                if (this.OnConnect != null)
                {
                    this.OnConnect();
                }

                // Go in a loop to wait for incoming messages
                while (Status == SNWebSocketStatus.Open)
                {
                    // Wait for data
                    int bytesReceived = Socket.Receive(buffer);

                    if (bytesReceived > 0)
                    {
                        try
                        {
                            SNWebSocketMessage message = new SNWebSocketMessage(buffer);
                            message.Decode();

                            if (message.Type == SNWebSocketMessageType.Text && this.OnMessage != null)
                            {
                                this.OnMessage(message.Message);
                            }
                            if (message.Type == SNWebSocketMessageType.CloseConnection)
                            {
                                if (Status == SNWebSocketStatus.Open)
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
                Status = SNWebSocketStatus.Closed;
            }

            if (this.OnDisconnect != null)
            {
                this.OnDisconnect();
            }
        }

        public delegate void OnConnectDelegate();
        public event OnConnectDelegate OnConnect;
        public delegate void OnMessageDelegate(string message);
        public event OnMessageDelegate OnMessage;
        public delegate void OnErrorDelegate(Exception exception);
        public event OnErrorDelegate OnError;
        public delegate void OnDisconnectDelegate();
        public event OnDisconnectDelegate OnDisconnect;
    }
}