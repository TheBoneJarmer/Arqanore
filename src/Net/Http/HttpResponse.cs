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
        public HttpHeaders Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public HttpVersion Version { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Body { get; set; }
        public Socket Socket { get; private set; }

        public HttpResponse(Socket socket)
        {
            this.Version = HttpVersion.Http11;
            this.StatusCode = HttpStatusCode.OK;
            this.Body = "";
            this.Socket = socket;
            this.Headers = new HttpHeaders();
            this.Cookies = new Dictionary<string, string>();

            // Add default headers
            this.Headers.Add("Server", "Ecredia");
            this.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public void Send()
        {
            // Add required headers
            if (!Headers.Contains("Content-Type")) Headers.Add("Content-Type", "text");
            if (!Headers.Contains("Content-Length"))Headers.Add("Content-Length", Body.Length.ToString());

            // Then, generate the response
            byte[] data = Encoding.ASCII.GetBytes(GenerateRaw());

            // Send the data
            Socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, Socket);
        }

        public void SendFile(string path)
        {
            if (!File.Exists(path))
            {
                NotFound();
                return;
            }

            // Send the file
            if (!Headers.Contains("Content-Type")) Headers.Add("Content-Type", HttpHelper.GetContentType(path));
            if (!Headers.Contains("Content-Length")) Headers.Add("Content-Length", new FileInfo(path).Length.ToString());
            
            Socket.BeginSendFile(path, Encoding.ASCII.GetBytes(GenerateRawHeaders()), null, 0, SendFileCallback, Socket);
        }
        public void SendJson(object obj)
        {
            Body = JsonConvert.SerializeObject(obj);
            Headers.Add("Content-Type", "application/json");
            Headers.Add("Content-Length", Body.Length.ToString());
            Send();
        }

        /* STATUS CODE SEND METHODS */
        public void Ok(string data = "")
        {
            StatusCode = HttpStatusCode.OK;
            Body = data;
            Send();
        }
        public void Ok(object obj)
        {
            StatusCode = HttpStatusCode.OK;
            SendJson(obj);
        }
        public void NotFound(string data = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Body = data;
            Send();
        }
        public void NotFound(object obj)
        {
            StatusCode = HttpStatusCode.NotFound;
            SendJson(obj);
        }
        public void Redirect(string url)
        {
            StatusCode = HttpStatusCode.Redirect;
            Headers.Add("Location", url);
            Send();
        }
        public void InternalServerError(string data = "")
        {
            StatusCode = HttpStatusCode.InternalServerError;
            Body = data;
            Send();
        }
        public void InternalServerError(object obj)
        {
            StatusCode = HttpStatusCode.InternalServerError;
            SendJson(obj);
        }
        public void BadRequest(string data = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Body = data;
            Send();
        }
        public void BadRequest(object obj)
        {
            StatusCode = HttpStatusCode.BadRequest;
            SendJson(obj);
        }
        public void Forbidden(string data = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Body = data;
            Send();
        }
        public void Forbidden(object obj)
        {
            StatusCode = HttpStatusCode.Forbidden;
            SendJson(obj);
        }
        public void Unauthorized(string data = "")
        {
            StatusCode = HttpStatusCode.Unauthorized;
            Body = data;
            Send();
        }
        public void Unauthorized(object obj)
        {
            StatusCode = HttpStatusCode.Unauthorized;
            SendJson(obj);
        }
        public void MethodNotAllowed(string data = "")
        {
            StatusCode = HttpStatusCode.MethodNotAllowed;
            Body = data;
            Send();
        }
        public void MethodNotAllowed(object obj)
        {
            StatusCode = HttpStatusCode.MethodNotAllowed;
            SendJson(obj);
        }
        public void UnsupportedMediaType(string data = "")
        {
            StatusCode = HttpStatusCode.UnsupportedMediaType;
            Body = data;
            Send();
        }
        public void UnsupportedMediaType(object obj)
        {
            StatusCode = HttpStatusCode.UnsupportedMediaType;
            SendJson(obj);
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

                for (int i=0; i<Cookies.Count; i++)
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

        public string GenerateRaw()
        {
            string data = GenerateRawHeaders();

            data += Body;

            return data;
        }
    }
}