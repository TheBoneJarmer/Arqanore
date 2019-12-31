using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Arqanore.Net.Http;
using Arqanore.Net.Sockets;

namespace Arqanore.Net.Http
{
    public class HttpServer
    {
        private Thread thread1;
        private Thread thread2;

        private int port;
        private int maxConnections;
        private int maxUploadSize;

        public int MaxUploadSize
        {
            get { return maxUploadSize; }
            set
            {
                maxUploadSize = value;

                if (maxUploadSize <= 0)
                {
                    throw new InvalidOperationException("Maximum Upload Size cannot be smaller than 1");
                }
            }
        }

        public HttpServer() : this(80)
        {

        }
        public HttpServer(int port)
        {
            this.port = port;
            this.maxConnections = 1000000;
        }

        public void Start()
        {
            thread1 = new Thread(Thread1_Callback);
            thread1.Start();

            thread2 = new Thread(Thread2_Callback);
            thread2.Start();
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
            SocketAccept accept = new SocketAccept();
            accept.Socket = listener;
            accept.ResetEvent = new ManualResetEvent(false);

            while (true)
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
                SocketAccept accept = (SocketAccept)result.AsyncState;

                // Continue the main thread
                accept.ResetEvent.Set();

                // Get the socket handler
                Socket listener = accept.Socket;
                Socket handler = listener.EndAccept(result);

                // Create the package object
                SocketMessage socketMessage = new SocketMessage();
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
                        HttpRequest request = null;
                        HttpResponse response = null;
                        HttpConnectionInfo info = null;

                        try
                        {
                            info = new HttpConnectionInfo(socketMessage.Socket);
                            response = new HttpResponse(socketMessage.Socket);

                            if (!CheckFilter(info.IPAddress))
                            {
                                Console.WriteLine("Blocked connection attempt for " + info.IPAddress);

                                response.StatusCode = HttpStatusCode.Forbidden;
                                response.Send();

                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex.Message);
                            Console.Error.WriteLine(ex.StackTrace);
                        }

                        try
                        {
                            // Create the request object
                            if (MaxUploadSize == 0)
                            {
                                request = new HttpRequest(socketMessage.Data);
                            }
                            else
                            {
                                request = new HttpRequest(socketMessage.Data, MaxUploadSize);
                            }

                            // Handle OPTIONS request
                            if (request.Method == HttpMethod.Options)
                            {
                                response.Headers.Add("Access-Control-Allow-Method", "GET, POST, OPTIONS, PUT, DELETE");
                                response.Headers.Add("Access-Control-Allow-Headers", "*");
                                response.Send();

                                return;
                            }

                            if (this.OnRequest != null)
                            {
                                this.OnRequest(request, response, info);
                            }
                            else
                            {
                                response.Send();
                            }
                        }
                        catch (HttpException ex)
                        {
                            response.StatusCode = ex.StatusCode;
                            response.Send(ex.ResponseText);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex.Message);
                            Console.Error.WriteLine(ex.StackTrace);

                            response.StatusCode = HttpStatusCode.InternalServerError;
                            response.Send();
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

        private bool CheckFilter(IPAddress iPAddress)
        {
            // Only allow access from ip addresses listed in this file
            if (File.Exists("whitelist.dat"))
            {
                string[] ipaddresses = File.ReadAllLines("whitelist.dat");

                foreach (var address in ipaddresses)
                {
                    if (address == iPAddress.ToString())
                    {
                        return true;
                    }
                }

                return false;
            }

            // Do not allow access to ip addresses listed in this file
            if (File.Exists("blacklist.dat"))
            {
                string[] ipaddresses = File.ReadAllLines("whitelist.dat");

                foreach (var address in ipaddresses)
                {
                    if (address == iPAddress.ToString())
                    {
                        return false;
                    }
                }

                return true;
            }

            return true;
        }

        /* EVENTS */
        public delegate void OnRequestHandler(HttpRequest request, HttpResponse response, HttpConnectionInfo info);
        public event OnRequestHandler OnRequest;
    }
}