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
        private Thread thread;
        private ManualResetEvent reset;

        public int Id { get; private set; }
        public Socket Socket { get; set; }
        public WebSocketStatus Status { get; set; }
        public IPAddress IPAddress { get; set; }

        public Client(int id)
        {
            Id = id;
            Status = WebSocketStatus.Opening;
        }

        public void Connect(Socket socket, string webSocketRequestData)
        {
            Socket = socket;
            Status = WebSocketStatus.Open;
            IPAddress = ((IPEndPoint)(socket.RemoteEndPoint)).Address;

            reset = new ManualResetEvent(false);

            // Parse the request
            var request = new WebSocketRequest();
            request.Parse(webSocketRequestData);

            // Generate the response
            var response = new WebSocketResponse();
            response.Key = response.GenerateKey(request.Key);

            // Send the response
            SendHandshake(response);

            // Create the thread and start it
            thread = new Thread(ThreadCallback);
            thread.Start();
        }

        public void Send(string data)
        {
            try
            {
                WebSocketMessage message = new WebSocketMessage(data, WebSocketMessageType.Text);
                message.Encode();

                Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
            }
            catch (Exception)
            {
                Status = WebSocketStatus.Closed;
                throw;
            }
        }
        public void Send(byte[] data)
        {
            try
            {
                WebSocketMessage message = new WebSocketMessage(data, WebSocketMessageType.Text);
                message.Encode();

                Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
            }
            catch (Exception)
            {
                Status = WebSocketStatus.Closed;
                throw;
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
            catch (Exception)
            {
                Status = WebSocketStatus.Closed;
                throw;
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
            catch (Exception)
            {
                Status = WebSocketStatus.Closed;
                throw;
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
            try
            {
                byte[] httpResponse = response.ToHttpResponse();
                Socket.BeginSend(httpResponse, 0, httpResponse.Length, SocketFlags.None, SendCallback, Socket);
            }
            catch (Exception)
            {
                Status = WebSocketStatus.Closed;
                throw;
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            try
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
            catch (Exception ex)
            {
                if (this.OnError != null)
                {
                    this.OnError(this, ex);
                }

                Status = WebSocketStatus.Closed;
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                var socketMessage = (SocketMessage)result.AsyncState;
                var handler = socketMessage.Socket;

                // Continue with the thread
                reset.Set();

                // Read the amount of received bytes
                var bytesReceived = handler.EndReceive(result);

                // Handle them
                if (bytesReceived == 0)
                {
                    return;
                }

                // Complete the package's data
                socketMessage.Data = socketMessage.Data.Push(socketMessage.Buffer.Slice(0, bytesReceived));

                if (bytesReceived == socketMessage.Buffer.Length)
                {
                    handler.BeginReceive(socketMessage.Buffer, 0, socketMessage.Buffer.Length, SocketFlags.None, ReceiveCallback, socketMessage);
                }
                else
                {
                    WebSocketMessage message = new WebSocketMessage(socketMessage.Buffer);
                    message.Decode();

                    if (message.Type == WebSocketMessageType.Text && this.OnMessage != null)
                    {
                        OnMessage(this, message.Message);
                    }
                    if (message.Type == WebSocketMessageType.CloseConnection && Status == WebSocketStatus.Open)
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

                Status = WebSocketStatus.Closed;
            }
        }

        private void ThreadCallback()
        {
            try
            {
                if (this.OnConnect != null)
                {
                    this.OnConnect(this);
                }

                // Go in a loop to wait for incoming messages
                while (Status == WebSocketStatus.Open)
                {
                    reset.Reset();

                    // Create the package object
                    SocketMessage socketMessage = new SocketMessage();
                    socketMessage.Socket = Socket;

                    // Read the package
                    Socket.BeginReceive(socketMessage.Buffer, 0, socketMessage.Buffer.Length, SocketFlags.None, ReceiveCallback, socketMessage);

                    reset.WaitOne();
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