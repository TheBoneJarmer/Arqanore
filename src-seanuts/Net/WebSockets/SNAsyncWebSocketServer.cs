using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Seanuts.Net.Http;
using Seanuts.Net.Sockets;

namespace Seanuts.Net.WebSockets
{
    public class SNAsyncWebSocketServer
    {
        public List<SNClient> Clients { get; private set; }

        private Thread UpdateThread { get; set; }
        private Thread ListenThread1 { get; set; }
        private Thread ListenThread2 { get; set; }
        public bool Running { get; private set; }

        private ManualResetEvent resetEvent1;
        private ManualResetEvent resetEvent2;
        private int port;
        private int clientCount;

        public SNAsyncWebSocketServer() : this(5000)
        {

        }
        public SNAsyncWebSocketServer(int port)
        {
            this.port = port;
            this.resetEvent1 = new ManualResetEvent(false);
            this.resetEvent2 = new ManualResetEvent(false);
            this.Clients = new List<SNClient>();
        }

        public void Start()
        {
            this.Running = true;

            // Start the update thread
            UpdateThread = new Thread(UpdateThreadCallback);
            UpdateThread.Start();

            // Start the listen threads
            ListenThread1 = new Thread(ListenThread1Callback);
            ListenThread1.Start();

            ListenThread2 = new Thread(ListenThread2Callback);
            ListenThread2.Start();
        }

        public void Stop()
        {
            this.Running = false;

            foreach (var client in Clients)
            {
                client.Kick();
            }
        }

        public void Broadcast(string data)
        {
            foreach (var client in Clients)
            {
                client.Send(data);
            }
        }
        public void Broadcast(List<SNClient> clients, string data)
        {
            foreach (var client in clients)
            {
                client.Send(data);
            }
        }

        private void AcceptCallback1(IAsyncResult result)
        {
            try
            {
                // Continue the main thread
                resetEvent1.Set();

                // Get the socket handler
                Socket listener = (Socket)result.AsyncState;
                Socket handler = listener.EndAccept(result);

                // Create the package object
                SNSocketMessage socketMessage = new SNSocketMessage();
                socketMessage.Socket = handler;

                // Read the package
                handler.BeginReceive(socketMessage.Buffer, 0, socketMessage.Buffer.Length, SocketFlags.None, ReceiveCallback, socketMessage);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }
        private void AcceptCallback2(IAsyncResult result)
        {
            try
            {
                // Continue the main thread
                resetEvent2.Set();

                // Get the socket handler
                Socket listener = (Socket)result.AsyncState;
                Socket handler = listener.EndAccept(result);

                // Create the package object
                SNSocketMessage socketMessage = new SNSocketMessage();
                socketMessage.Socket = handler;

                // Read the package
                handler.BeginReceive(socketMessage.Buffer, 0, socketMessage.Buffer.Length, SocketFlags.None, ReceiveCallback, socketMessage);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
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
                            // Create the client
                            SNClient client = new SNClient(clientCount);

                            // Add all events
                            client.OnConnect += OnConnect;
                            client.OnMessage += OnMessage;
                            client.OnError += OnError;
                            client.OnDisconnect += OnDisconnect;
                            client.OnFatal += OnFatal;

                            // Connect the client
                            client.Connect(socketMessage.Socket, socketMessage.ToString());

                            // Add the client to our list of clients
                            Clients.Add(client);

                            // Increase the client count variable for future id generation
                            clientCount += 1;
                        }
                        catch (SNWebSocketException ex)
                        {
                            SNHttpServerResponse response = new SNHttpServerResponse(handler);
                            response.BadRequest(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            SNHttpServerResponse response = new SNHttpServerResponse(handler);
                            response.InternalServerError(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        private bool CheckLocalResolving()
        {
            var ipHostEntry1 = Dns.GetHostEntry(Environment.MachineName);
            var ipHostEntry2 = Dns.GetHostEntry("localhost");
            var ipAddress1 = ipHostEntry1.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            var ipAddress2 = ipHostEntry2.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);

            // If the machine's IP is a APIPA address it can't make a connection to the internet and is considered local too
            if (ipAddress1.ToString().StartsWith("169.254"))
            {
                return true;
            }

            return ipAddress1.ToString().Equals(ipAddress2.ToString());
        }

        /* THREAD CALLBACKS */
        private void ListenThread1Callback()
        {
            // Setup global endpoint
            var ipHostEntry = Dns.GetHostEntry(Environment.MachineName);
            var ipAddress = ipHostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            var ipEndpoint = new IPEndPoint(ipAddress, port);

            Console.WriteLine("Listening on " + Environment.MachineName + ":" + port);
            Console.WriteLine("Listening on " + ipAddress + ":" + port);

            // Create a TCP/IP socket
            Socket listener = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Start listening to incoming requests
            listener.Bind(ipEndpoint);
            listener.Listen(1000000);

            while (Running)
            {
                resetEvent1.Reset();
                listener.BeginAccept(AcceptCallback1, listener);
                resetEvent1.WaitOne();
            }
        }
        private void ListenThread2Callback()
        {
            // Setup local endpoint
            var ipHostEntry = Dns.GetHostEntry("localhost");
            var ipAddress = ipHostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            var ipEndpoint = new IPEndPoint(ipAddress, port);

            // If the machine is not connected to the internet, both the machine name and localhost will resolve to 127.0.0.1
            // This is going to cause an error because you cannot have multiple sockets listen to the same ip address
            if (CheckLocalResolving())
            {
                return;
            }

            Console.WriteLine("Listening on localhost:" + port);
            Console.WriteLine("Listening on " + ipAddress + ":" + port);

            // Create a TCP/IP socket
            Socket listener = new Socket(ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Start listening to incoming requests
            listener.Bind(ipEndpoint);
            listener.Listen(1000000);

            while (Running)
            {
                resetEvent2.Reset();
                listener.BeginAccept(AcceptCallback2, listener);
                resetEvent2.WaitOne();
            }
        }

        private void UpdateThreadCallback()
        {
            // Update each 5 seconds
            while (Running)
            {
                try
                {
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        var client = Clients[i];

                        // If the client is not connected anymore, remove it from the list
                        if (client.Status == SNWebSocketStatus.Closed)
                        {
                            Clients.Remove(client);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                }

                Thread.Sleep(10 * 1000);
            }
        }

        /* EVENTS */
        public delegate void OnConnectDelegate(SNClient client);
        public event OnConnectDelegate OnConnect;
        public delegate void OnMessageDelegate(SNClient client, string message);
        public event OnMessageDelegate OnMessage;
        public delegate void OnErrorDelegate(SNClient client, Exception exception);
        public event OnErrorDelegate OnError;
        public delegate void OnFatalDelegate(Exception exception);
        public event OnFatalDelegate OnFatal;
        public delegate void OnDisconnectDelegate(SNClient client);
        public event OnDisconnectDelegate OnDisconnect;
    }
}