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
using System.Collections.Generic;

namespace Arqanore.Net.WebSockets
{
    public class Client
    {
        private Thread thread;
        private Socket socket;

        public string Id { get; private set; }
        public WebSocketStatus Status { get; private set; }
        public IPAddress IPAddress { get; private set; }
        public Dictionary<string, string> Properties { get; private set; }

        public Client()
        {
            Id = Guid.NewGuid().ToString();
            Status = WebSocketStatus.Closed;
            Properties = new Dictionary<string, string>();
        }

        public void Connect(Socket socket, byte[] data)
        {
            Connect(socket, Encoding.ASCII.GetString(data));
        }
        public void Connect(Socket socket, string webSocketRequestData)
        {
            try
            {
                this.socket = socket;
                this.Status = WebSocketStatus.Open;
                this.IPAddress = ((IPEndPoint)(socket.RemoteEndPoint)).Address;

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
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(this, ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(this, ex);
            }
        }

        public void Send(string data)
        {
            if (Status != WebSocketStatus.Open)
            {
                return;
            }

            try
            {
                WebSocketMessage message = new WebSocketMessage(data, WebSocketMessageType.Text);
                message.Encode();

                socket.Send(message.Buffer);
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(this, ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(this, ex);
            }
        }
        public void Send(byte[] data)
        {
            if (Status != WebSocketStatus.Open)
            {
                return;
            }

            try
            {
                WebSocketMessage message = new WebSocketMessage(data, WebSocketMessageType.Text);
                message.Encode();

                socket.Send(message.Buffer);
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(this, ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(this, ex);
            }
        }

        public void Disconnect()
        {
            try
            {
                WebSocketMessage message = new WebSocketMessage("", WebSocketMessageType.CloseConnection);
                message.Encode();

                socket.Send(message.Buffer);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                Status = WebSocketStatus.Closed;
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(this, ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(this, ex);
            }
        }

        public void Kick()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                Status = WebSocketStatus.Closed;
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(this, ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(this, ex);
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
                socket.Send(response.ToHttpResponse());
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(this, ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(this, ex);
            }
        }

        private void ThreadCallback()
        {
            try
            {
                var buffer = new byte[1024 * 1024 * 4];

                OnConnect?.Invoke(this);

                // Go in a loop to wait for incoming messages
                while (Status == WebSocketStatus.Open)
                {
                    var bytesReceived = socket.Receive(buffer);

                    if (bytesReceived > 0)
                    {
                        var data = buffer.Slice(0, bytesReceived);

                        while (data != null)
                        {
                            var message = new WebSocketMessage(data);
                            data = message.Decode();

                            if (message.Type == WebSocketMessageType.Text && OnMessage != null)
                            {
                                OnMessage(this, message.Message);
                            }
                            if (message.Type == WebSocketMessageType.CloseConnection)
                            {
                                Disconnect();
                            }
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                Status = WebSocketStatus.Closed;
                OnSocketError?.Invoke(this, ex);
            }
            catch (Exception ex)
            {
                Status = WebSocketStatus.Closed;
                OnError?.Invoke(this, ex);
            }

            OnDisconnect?.Invoke(this);
        }

        /* EVENTS */
        public event WebSocketServer.OnConnectDelegate OnConnect;
        public event WebSocketServer.OnMessageDelegate OnMessage;
        public event WebSocketServer.OnClientErrorDelegate OnError;
        public event WebSocketServer.OnClientSocketErrorDelegate OnSocketError;
        public event WebSocketServer.OnDisconnectDelegate OnDisconnect;
    }
}