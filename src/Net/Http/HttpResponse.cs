using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Arqanore.Net.Http
{
    public class HttpResponse
    {
        public Dictionary<string, string> Cookies { get; set; }
        public HttpHeaders Headers { get; set; }
        public HttpVersion Version { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Socket Socket { get; private set; }

        public HttpResponse(Socket socket)
        {
            this.Version = HttpVersion.Http11;
            this.StatusCode = HttpStatusCode.OK;
            this.Socket = socket;
            this.Headers = new HttpHeaders();
            this.Cookies = new Dictionary<string, string>();

            // Add default headers
            this.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public void Send(string body = "")
        {
            // Add required headers
            if (!Headers.Contains("Content-Type")) Headers.Add("Content-Type", "text");
            if (!Headers.Contains("Content-Length")) Headers.Add("Content-Length", body.Length.ToString());

            // Then, generate the response
            byte[] data = Encoding.ASCII.GetBytes(GenerateRaw(body));

            // Send the data
            Socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, Socket);
        }

        public void SendFile(string path)
        {
            if (!File.Exists(path))
            {
                StatusCode = HttpStatusCode.NotFound;
                Send();

                return;
            }

            // Send the file
            if (!Headers.Contains("Content-Type")) Headers.Add("Content-Type", HttpHelper.GetContentType(path));
            if (!Headers.Contains("Content-Length")) Headers.Add("Content-Length", new FileInfo(path).Length.ToString());

            Socket.BeginSendFile(path, Encoding.ASCII.GetBytes(GenerateRawHeaders()), null, 0, SendFileCallback, Socket);
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;

            handler.EndSend(result);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        private void SendFileCallback(IAsyncResult result)
        {
            Socket handler = (Socket)result.AsyncState;

            handler.EndSendFile(result);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        public string GenerateRawHeaders()
        {
            string data = "";

            // Start the first line with the http version
            if (Version == HttpVersion.Http11) data += "HTTP/1.1 ";

            // Continue with the status
            data += (int)StatusCode + " ";

            // And finish with the status text and a carriage return and line feed
            data += HttpHelper.GetStatusText(StatusCode) + "\r\n";

            // Write down all the headers
            data += Headers.ToString();

            if (Cookies.Count > 0)
            {
                data += "Set-Cookie: ";

                for (int i = 0; i < Cookies.Count; i++)
                {
                    var key = Cookies.Keys.ElementAt(i);
                    var value = Cookies.Values.ElementAt(i);

                    data += key + "=" + value;

                    if (i < Cookies.Count - 1)
                    {
                        data += ";";
                    }
                }

                data += "\r\n";
            }

            // Write down a new line
            data += "\r\n";

            return data;
        }

        public string GenerateRaw(string body)
        {
            return GenerateRawHeaders() + body;
        }
    }
}