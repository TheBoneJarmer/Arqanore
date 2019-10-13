using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Seanuts.Sockets
{
    public class SNAsyncSocketServer
    {
        private Thread thread1;
        private Thread thread2;

        private int port;
        private int maxConnections;
        private bool listening;

        public SNAsyncSocketServer(int port)
        {
            this.port = port;
            this.maxConnections = 1000000;
        }

        public void Start()
        {
            listening = true;

            thread1 = new Thread(Thread1_Callback);
            thread1.Start();

            thread2 = new Thread(Thread2_Callback);
            thread2.Start();
        }
        public void Stop()
        {
            listening = false;
        }

        private void Thread1_Callback()
        {
            var ipHostEntry = Dns.GetHostEntry(Environment.MachineName);
            var ipAddress = ipHostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            var ipEndpoint = new IPEndPoint(ipAddress, port);

            Console.WriteLine("Listening on " + ipHostEntry.HostName + ":" + port);
            Console.WriteLine("Listening on " + ipAddress + ":" + port);

            Listen(ipEndpoint);
        }
        private void Thread2_Callback()
        {
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

        private bool CheckLocalResolving()
        {
            var ipHostEntry1 = Dns.GetHostEntry(Environment.MachineName);
            var ipHostEntry2 = Dns.GetHostEntry("localhost");
            var ipAddress1 = ipHostEntry1.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            var ipAddress2 = ipHostEntry2.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);

            // If the machine's IP is a APIPA address it can't make connection and is considered local too
            if (ipAddress1.ToString().StartsWith("169.254"))
            {
                return true;
            }

            return ipAddress1.ToString().Equals(ipAddress2.ToString());
        }

        private void Listen(IPEndPoint endPoint)
        {
            // Create a TCP/IP socket
            Socket listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Start listening to incoming requests
            listener.Bind(endPoint);
            listener.Listen(maxConnections);

            // Create the accept object
            SNSocketAccept accept = new SNSocketAccept();
            accept.Socket = listener;
            accept.ResetEvent = new ManualResetEvent(false);

            while (listening)
            {
                try
                {
                    accept.ResetEvent.Reset();
                    accept.Socket.BeginAccept(AcceptCallback, accept);
                    accept.ResetEvent.WaitOne();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                }
            }
        }

        private void AcceptCallback(IAsyncResult result)
        {
            try
            {
                SNSocketAccept accept = (SNSocketAccept)result.AsyncState;

                // Continue the main thread
                accept.ResetEvent.Set();

                // Get the socket handler
                Socket listener = accept.Socket;
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
                    if (bytesRead == 1024)
                    {
                        handler.BeginReceive(socketMessage.Buffer, 0, 1024, SocketFlags.None, ReceiveCallback, socketMessage);
                    }
                    else
                    {
                        if (OnMessage != null)
                        {
                            OnMessage(handler, socketMessage.Data);
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

        /* EVENTS */
        public delegate void OnMessageHandler(Socket client, byte[] message);
        public event OnMessageHandler OnMessage;
    }
}
