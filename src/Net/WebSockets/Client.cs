using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Arqanore.Net.WebSockets;
using Arqanore.Net.Http;
using Arqanore.Net.Sockets;
using Newtonsoft.Json;

namespace Arqanore.Net.WebSockets
{
    public class Client
    {
        public int Id { get; private set; }

        public Thread Thread { get; set; }
        public Socket Socket { get; set; }
        public WebSocketStatus Status { get; set; }

        public IPAddress IPAddress
        {
            get { return ((IPEndPoint)(Socket.RemoteEndPoint)).Address; }
        }

        public Client(int id)
        {
            Id = id;
            Status = WebSocketStatus.Opening;
        }

        public void Connect(Socket socket, string webSocketRequestData)
        {
            Socket = socket;
            Status = WebSocketStatus.Open;

            // Parse the request
            var request = new WebSocketRequest();
            request.Parse(webSocketRequestData);

            // Generate the response
            var response = new WebSocketResponse();
            response.Key = response.GenerateKey(request.Key);

            // Send the response
            SendHandshake(response);

            // Create the thread and start it
            Thread = new Thread(ThreadCallback);
            Thread.Start();
        }

        public void Send(string data)
        {
            try
            {
                WebSocketMessage message = new WebSocketMessage(data, WebSocketMessageType.Text);
                message.Encode();

                Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                Status = WebSocketStatus.Closed;
            }
        }

        public void Disconnect()
        {
            try
            {
                Status = WebSocketStatus.Closing;

                WebSocketMessage message = new WebSocketMessage("", WebSocketMessageType.CloseConnection);
                message.Encode();

                Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                Status = WebSocketStatus.Closed;
            }
        }

        public void Kick()
        {
            try
            {
                Status = WebSocketStatus.Closed;

                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                Status = WebSocketStatus.Closed;
            }
        }

        public long Ping()
        {
            var ping = new Ping();
            var reply = ping.Send(IPAddress);

            if (reply.Status == IPStatus.Success)
            {
                return reply.RoundtripTime;
            }

            return -1;
        }

        private void SendHandshake(WebSocketResponse response)
        {
            byte[] httpResponse = response.ToHttpResponse();
            Socket.BeginSend(httpResponse, 0, httpResponse.Length, SocketFlags.None, SendCallback, Socket);
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;
            handler.EndSend(result);

            if (Status == WebSocketStatus.Closing)
            {
                // Close the socket connection
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Status = WebSocketStatus.Closed;
            }
        }

        private void ThreadCallback()
        {
            try
            {
                byte[] buffer = new byte[1024];

                if (this.OnConnect != null)
                {
                    this.OnConnect(this);
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
                                this.OnMessage(this, message.Message);
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
                                this.OnError(this, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.OnFatal != null)
                {
                    this.OnFatal(ex);
                }

                Status = WebSocketStatus.Closed;
            }

            if (this.OnDisconnect != null)
            {
                this.OnDisconnect(this);
            }
        }

        /* EVENTS */
        public event WebSocketServer.OnConnectDelegate OnConnect;
        public event WebSocketServer.OnMessageDelegate OnMessage;
        public event WebSocketServer.OnErrorDelegate OnError;
        public event WebSocketServer.OnFatalDelegate OnFatal;
        public event WebSocketServer.OnDisconnectDelegate OnDisconnect;
    }
}