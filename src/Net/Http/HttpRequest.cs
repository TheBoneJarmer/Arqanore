using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;

namespace Arqanore.Net.Http
{
    public class HttpRequest
    {
        public HttpMethod Method { get; set; }
        public HttpVersion Version { get; set; }
        public HttpHeaders Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public Dictionary<string, string> Query { get; set; }
        public List<HttpRequestFile> Files { get; set; }
        public HttpRequestBody Body { get; set; }
        public string Path { get; set; }
        public string Raw { get; set; }
        
        public HttpRequest()
        {
            this.Headers = new HttpHeaders();
            this.Cookies = new Dictionary<string, string>();
            this.Files = new List<HttpRequestFile>();
            this.Query = new Dictionary<string, string>();
        }

        public HttpRequest(byte[] data) : this()
        {
            Raw = Encoding.ASCII.GetString(data);
            this.Parse(data, 2 * 1024 * 1024);
        }
        public HttpRequest(byte[] data, int maxContentLength) : this()
        {
            Raw = Encoding.ASCII.GetString(data);
            this.Parse(data, maxContentLength);
        }

        private void Parse(byte[] data, int maxContentLength)
        {
            var bodyStart = false;
            var bodyData = new List<byte>();
            var line = "";
            var carriageReturn = false;
            var newLine = false;

            foreach (var b in data)
            {
                char c = (char)b;

                // Add the character to the line
                line += c;

                // Check for line endings
                if (c == '\r') carriageReturn = true;
                if (c == '\n') newLine = true;

                // Check if the line indicates the start of the body
                if (line.Length == 2 && carriageReturn && newLine)
                {
                    bodyStart = true;
                    continue;
                }

                // Parse the line if the line is complete
                if (carriageReturn && newLine && !bodyStart)
                {
                    ParseLine(line);

                    line = "";
                    carriageReturn = false;
                    newLine = false;
                }

                // If the headers are done, start with the body
                if (bodyStart)
                {
                    bodyData.Add(b);
                }
            }

            ValidateMethod();
            ValidateContentType();
            ValidateContentLength(maxContentLength);

            // Parse the body
            ParseBody(bodyData);
        }

        private void ParseLine(string line)
        {
            // Convert the HttpMethod enum to an array of strings
            var httpMethods = new string[5] { "GET", "POST", "PUT", "DELETE", "OPTIONS" };

            // Check if the line is the method line
            if (httpMethods.Any(x => line.StartsWith(x.ToUpper())))
            {
                ParseFirst(line);
            }
            else
            {
                ParseHeader(line);
            }
        }

        private void ParseFirst(string line)
        {
            string method = line.Split(' ')[0];
            string path = line.Split(' ')[1];
            string version = line.Split(' ')[2];

            // Set the method
            if (method == "GET") this.Method = HttpMethod.Get;
            if (method == "POST") this.Method = HttpMethod.Post;
            if (method == "PUT") this.Method = HttpMethod.Put;
            if (method == "DELETE") this.Method = HttpMethod.Delete;
            if (method == "OPTIONS") this.Method = HttpMethod.Options;

            // Set the path
            this.Path = HttpHelper.ConvertHexCodes(path);

            // Convert part of the path to query if the method is GET
            if (this.Method == HttpMethod.Get && path.Contains("?"))
            {
                string actualPath = path.Split('?')[0];
                string queryString = path.Split('?')[1];

                // Reset the path
                this.Path = actualPath;

                // Fill up the query dictionary
                this.Query = new Dictionary<string, string>();

                foreach (string keyvalue in queryString.Split('&'))
                {
                    string key = keyvalue.Split('=')[0];
                    string value = keyvalue.Split('=')[1];

                    Query.Add(key, HttpHelper.ConvertHexCodes(value));
                }
            }

            // Set the version
            if (version == "HTTP/1.1") this.Version = HttpVersion.Http11;
        }

        private void ParseHeader(string line)
        {
            string headerName = line.Split(':')[0];
            string headerValue = line.Split(':')[1].Substring(1);

            // The cookie header's values are key value pairs as well, so therefore the seperate dictionary
            if (headerName == "cookie")
            {
                string[] cookies = headerValue.Split(';');

                foreach (var cookie in cookies)
                {
                    string cookieName = cookie.Replace(" ", "").Split('=')[0];
                    string cookieValue = cookie.Replace(" ", "").Split('=')[1];

                    Cookies.Add(cookieName, cookieValue);
                }
            }

            this.Headers.Add(headerName, headerValue);
        }

        private void ParseBody(List<byte> data)
        {
            var contentType = new string[0];

            // Parse the body only if the method needs it
            if (Method == HttpMethod.Get || Method == HttpMethod.Options || Method == HttpMethod.Delete)
            {
                return;
            }

            if (Headers.Contains("content-type"))
            {
                contentType = Headers["content-type"].Replace(" ", "").Replace("\r\n", "").Split(';');
            }

            if (contentType.Contains("multipart/form-data"))
            {
                ParseBodyMultipartFormData(data, contentType[1].Replace("boundary=", "").Replace("\r", "").Replace("\n", ""));
            }
            else
            {
                Body = new HttpRequestBody(Encoding.ASCII.GetString(data.ToArray()), contentType[0]);
            }
        }

        private void ParseBodyMultipartFormData(List<byte> data, string boundary)
        {
            var current = new HttpRequestFile();
            var newLine = false;
            var carriageReturn = false;
            var originalLine = "";
            var line = "";
            var lineBytes = new List<byte>();
            var keyValueData = "";

            // Parse the data
            foreach (var b in data)
            {
                char c = (char)b;

                // Add the char to the line
                line += c;

                // Add the byte to the list of bytes
                lineBytes.Add(b);

                // Check for newline and carriage return
                if (c == '\r') carriageReturn = true;
                if (c == '\n') newLine = true;

                // If the line ends, parse it
                if (!newLine || !carriageReturn)
                {
                    continue;
                }

                // Set the original line
                originalLine = line;

                // Remove the line endings
                line = line.Replace("\r", "");
                line = line.Replace("\n", "");

                if (line.Contains(boundary))
                {
                    if (current.Data.Count == 0)
                    {
                        line = "";
                        lineBytes.Clear();
                        carriageReturn = false;
                        newLine = false;

                        continue;
                    }
                    else
                    {
                        // Remove the last two bytes from the data because those are not part of the original file's content
                        current.Data.RemoveAt(current.Data.Count - 1);
                        current.Data.RemoveAt(current.Data.Count - 1);

                        if (current.ContentType != null)
                        {
                            this.Files.Add(current);
                        }
                        else
                        {
                            if (keyValueData.Length == 0)
                            {
                                keyValueData = current.Name + "=" + Encoding.ASCII.GetString(current.Data.ToArray());
                            }
                            else
                            {
                                keyValueData += "&" + current.Name + "=" + Encoding.ASCII.GetString(current.Data.ToArray());
                            }
                        }

                        current = new HttpRequestFile();
                        line = "";
                        lineBytes.Clear();
                        carriageReturn = false;
                        newLine = false;

                        continue;
                    }
                }

                // Parse the disposition and content type for the file
                if (line.StartsWith("Content-Disposition"))
                {
                    var values = line.Replace(" ", "").Replace("Content-Disposition:", "").Split(';');

                    if (values[0] == "form-data")
                    {
                        if (values.Length > 1)
                        {
                            current.Name = values[1].Split('=')[1].Replace("\"", "");
                        }
                        if (values.Length > 2)
                        {
                            current.FileName = values[2].Split('=')[1].Replace("\"", "");
                        }
                    }

                    line = "";
                    lineBytes.Clear();
                    carriageReturn = false;
                    newLine = false;

                    continue;
                }

                // Parse the content type
                if (line.StartsWith("Content-Type"))
                {
                    current.ContentType = line.Split(':')[1].Trim();
                    line = "";
                    lineBytes.Clear();
                    carriageReturn = false;
                    newLine = false;

                    continue;
                }

                // Parse the file body
                if (line.Length > 0)
                {
                    current.Data.AddRange(lineBytes);
                }

                line = "";
                lineBytes.Clear();
                carriageReturn = false;
                newLine = false;
            }

            // Set the body
            this.Body = new HttpRequestBody(keyValueData, "multipart/form-data");
        }

        private void ValidateContentType()
        {
            var contentType = new string[0];

            // Only validate in case of a post or put request
            if (Method == HttpMethod.Get || Method == HttpMethod.Options || Method == HttpMethod.Delete)
            {
                return;
            }

            // Search for the content type header
            if (Headers.Contains("content-type"))
            {
                contentType = Headers["content-type"].Trim().Split(';');
            }

            // Only continue if the header was found
            if (contentType.Length > 0)
            {
                var valid = false;

                // Check each content type and determine if it is allowed or not
                if (contentType[0] == "application/x-www-form-urlencoded")
                {
                    valid = true;
                }
                if (contentType[0] == "application/json")
                {
                    valid = true;
                }
                if (contentType[0] == "multipart/form-data")
                {
                    valid = true;
                }

                // If the header is still not valid, send a 415 Unsupported media type
                if (!valid)
                {
                    throw new HttpException(HttpStatusCode.UnsupportedMediaType, $"Content type \"{contentType[0]}\" not supported");
                }
            }
            else
            {
                throw new HttpException(HttpStatusCode.UnsupportedMediaType, $"Header \"Content-Type\" missing");
            }
        }
        private void ValidateContentLength(int maxLength)
        {
            // Only validate in case of a post or put request
            if (Method == HttpMethod.Get || Method == HttpMethod.Options || Method == HttpMethod.Delete)
            {
                return;
            }

            if (!Headers.Contains("content-length"))
            {
                throw new HttpException(HttpStatusCode.LengthRequired, $"Header 'content-length' missing");
            }

            // Check the content length
            if (Headers.ContentLength > maxLength)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Maximum content length exceeded");
            }
        }
        private void ValidateMethod()
        {
            if (Method == null)
            {
                throw new HttpException(HttpStatusCode.MethodNotAllowed, "The request method is not allowed");
            }
        }
    }
}