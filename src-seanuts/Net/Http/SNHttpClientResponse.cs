using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Seanuts.Net.Http
{
    public class SNHttpClientResponse
    {
        public SNHttpHeaders Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public SNHttpVersion Version { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public byte[] Body { get; set; }

        public SNHttpClientResponse()
        {
            Headers = new SNHttpHeaders();
            Cookies = new Dictionary<string, string>();
        }
        public SNHttpClientResponse(byte[] data) : this()
        {
            Parse(data);
        }

        private void Parse(byte[] data)
        {
            var line = "";
            var lineBytes = new List<byte>();
            var bodyBytes = new List<byte>();
            var carriageReturn = false;
            var newLine = false;
            var bodyStart = false;

            foreach (var b in data)
            {
                // Add the byte to the list of bytes
                lineBytes.Add(b);

                if (!bodyStart)
                {
                    // Check for newline and carriage return
                    if ((char)b == '\r') carriageReturn = true;
                    if ((char)b == '\n') newLine = true;

                    // If the line ends, parse it
                    if (!newLine || !carriageReturn)
                    {
                        continue;
                    }

                    // Remove the line endings
                    lineBytes.RemoveRange(lineBytes.Count - 2, 2);

                    // Convert the line to text
                    line = Encoding.ASCII.GetString(lineBytes.ToArray());
                }
                else
                {
                    bodyBytes.Add(b);
                    continue;
                }

                // Parse the first line
                if (line.StartsWith("HTTP"))
                {
                    var httpVersion = line.Split(' ')[0];
                    var statusCode = line.Split(' ')[1];
                    var statusText = line.Split(' ')[2];

                    // Set the right http version
                    if (httpVersion == "HTTP/1.1") Version = SNHttpVersion.Http11;

                    // Set the status code
                    StatusCode = (HttpStatusCode)(int.Parse(statusCode));

                    lineBytes.Clear();
                    carriageReturn = false;
                    newLine = false;

                    continue;
                }

                // Check for the body to start
                if (line == "" && !bodyStart)
                {
                    bodyStart = true;
                    lineBytes.Clear();
                    carriageReturn = false;
                    newLine = false;
                    continue;
                }

                // Parse all headers and cookies
                if (!bodyStart)
                {
                    var key = line.Split(':')[0];
                    var value = line.Split(':')[1].Substring(1);

                    if (key == "Set-Cookie")
                    {
                        var cookies = value.Split(';');

                        foreach (var cookie in cookies)
                        {
                            var cookieName = cookie.Split('=')[0];
                            var cookieValue = cookie.Split('=')[1];

                            Cookies.Add(cookieName, cookieValue);
                        }
                    }
                    else
                    {
                        Headers.Add(key, value);
                    }

                    lineBytes.Clear();
                    carriageReturn = false;
                    newLine = false;
                }
            }

            // Set the body
            Body = bodyBytes.ToArray();
        }
    }
}
