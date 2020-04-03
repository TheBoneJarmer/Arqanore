using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Arqanore.Net.Http;
using Arqanore.Net.Sockets;

namespace Arqanore.Net.WebSockets
{
    public class WebSocketServer
    {
        public List<Client> Clients { get; private set; }
        public bool Running { get; private set; }

        private Thread threadUpdate;
        private Thread threadListen1;
        private Thread threadListen2;
        private int port;

        public WebSocketServer(int port)
        {
            this.port = port;
            this.Clients = new List<Client>();
        }

        public void Start()
        {
            Running = true;

            // Start the update thread
            threadUpdate = new Thread(UpdateThreadCallback);
            threadUpdate.Start();

            // Start the listen threads
            threadListen1 = new Thread(ListenThread1Callback);
            threadListen1.Start();

            threadListen2 = new Thread(ListenThread2Callback);
            threadListen2.Start();
        }

        public void Stop()
        {
            Running = false;

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

        private bool CheckLocalResolving()
        {
            try
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
            catch (SocketException ex)
            {
                OnServerSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                OnServerError?.Invoke(ex);
            }

            return false;
        }

        private void Listen(IPEndPoint ipEndPoint)
        {
            var buffer = new byte[4194304];
            var listener = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(ipEndPoint);
            listener.Listen(1000000);

            while (Running)
            {
                var handler = listener.Accept();
                var bytesReceived = handler.Receive(buffer);

                if (bytesReceived > 0)
                {
                    var data = buffer.Slice(0, bytesReceived);

                    // Create the client
                    var client = new Client();
                    client.OnConnect += OnConnect;
                    client.OnMessage += OnMessage;
                    client.OnError += OnClientError;
                    client.OnDisconnect += OnDisconnect;
                    client.Connect(handler, data);

                    // Add the client to our list of clients
                    Clients.Add(client);
                }
            }
        }

        /* THREAD CALLBACKS */
        private void ListenThread1Callback()
        {
            try
            {
                // Setup global endpoint
                var ipHostEntry = Dns.GetHostEntry(Environment.MachineName);
                var ipAddress = ipHostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
                var ipEndpoint = new IPEndPoint(ipAddress, port);

                Console.WriteLine("Listening on " + Environment.MachineName + ":" + port);
                Console.WriteLine("Listening on " + ipAddress + ":" + port);

                Listen(ipEndpoint);
            }
            catch (SocketException ex)
            {
                OnServerSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                OnServerError?.Invoke(ex);
            }
        }
        private void ListenThread2Callback()
        {
            try
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

                Listen(ipEndpoint);
            }
            catch (SocketException ex)
            {
                OnServerSocketError?.Invoke(ex);
            }
            catch (Exception ex)
            {
                OnServerError?.Invoke(ex);
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
                        if (client.Status == WebSocketStatus.Closed)
                        {
                            Clients.Remove(client);
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnServerError?.Invoke(ex);
                }

                Thread.Sleep(10 * 1000);
            }
        }

        /* EVENTS */
        public delegate void OnConnectDelegate(Client client);
        public event OnConnectDelegate OnConnect;
        public delegate void OnMessageDelegate(Client client, byte[] message);
        public event OnMessageDelegate OnMessage;
        public delegate void OnClientErrorDelegate(Client client, Exception exception);
        public event OnClientErrorDelegate OnClientError;
        public delegate void OnClientSocketErrorDelegate(Client client, SocketException exception);
        public event OnClientSocketErrorDelegate OnClientSocketError;
        public delegate void OnServerSocketErrorDelegate(SocketException exception);
        public event OnServerSocketErrorDelegate OnServerSocketError;
        public delegate void OnServerErrorDelegate(Exception exception);
        public event OnServerErrorDelegate OnServerError;
        public delegate void OnDisconnectDelegate(Client client);
        public event OnDisconnectDelegate OnDisconnect;
    }
}