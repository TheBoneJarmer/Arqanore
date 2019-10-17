using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Seanuts.Net.WebSockets;
using Seanuts.Net.Http;
using Seanuts.Net.Sockets;
using Newtonsoft.Json;

namespace Seanuts.Net.WebSockets
{
    public class SNClient
    {
        public int Id { get; private set; }

        public Thread Thread { get; set; }
        public Socket Socket { get; set; }
        public SNWebSocketStatus Status { get; set; }

        public IPAddress IPAddress
        {
            get { return ((IPEndPoint)(Socket.RemoteEndPoint)).Address; }
        }

        public SNClient(int id)
        {
            Id = id;
            Status = SNWebSocketStatus.Opening;
        }

        public void Connect(Socket socket, string webSocketRequestData)
        {
            Socket = socket;
            Status = SNWebSocketStatus.Open;

            // Parse the request
            var request = new SNWebSocketRequest();
            request.Parse(webSocketRequestData);

            // Generate the response
            var response = new SNWebSocketResponse();
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
                SNWebSocketMessage message = new SNWebSocketMessage(data, SNWebSocketMessageType.Text);
                message.Encode();

                Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                Status = SNWebSocketStatus.Closed;
            }
        }

        public void Disconnect()
        {
            try
            {
                Status = SNWebSocketStatus.Closing;

                SNWebSocketMessage message = new SNWebSocketMessage("", SNWebSocketMessageType.CloseConnection);
                message.Encode();

                Socket.BeginSend(message.Buffer, 0, message.Buffer.Length, SocketFlags.None, SendCallback, Socket);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                Status = SNWebSocketStatus.Closed;
            }
        }

        public void Kick()
        {
            try
            {
                Status = SNWebSocketStatus.Closed;

                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                Status = SNWebSocketStatus.Closed;
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

        private void SendHandshake(SNWebSocketResponse response)
        {
            byte[] httpResponse = response.ToHttpResponse();
            Socket.BeginSend(httpResponse, 0, httpResponse.Length, SocketFlags.None, SendCallback, Socket);
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;
            handler.EndSend(result);

            if (Status == SNWebSocketStatus.Closing)
            {
                // Close the socket connection
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Status = SNWebSocketStatus.Closed;
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
                                this.OnMessage(this, message.Message);
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

                Status = SNWebSocketStatus.Closed;
            }

            if (this.OnDisconnect != null)
            {
                this.OnDisconnect(this);
            }
        }

        /* EVENTS */
        public event SNAsyncWebSocketServer.OnConnectDelegate OnConnect;
        public event SNAsyncWebSocketServer.OnMessageDelegate OnMessage;
        public event SNAsyncWebSocketServer.OnErrorDelegate OnError;
        public event SNAsyncWebSocketServer.OnFatalDelegate OnFatal;
        public event SNAsyncWebSocketServer.OnDisconnectDelegate OnDisconnect;
    }
}