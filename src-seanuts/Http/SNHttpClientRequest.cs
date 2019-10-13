using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Seanuts.Http
{
    public class SNHttpClientRequest
    {
        public HttpMethod Method { get; set; }
        public SNHttpHeaders Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public string Body { get; set; }
        public string Path { get; set; }
        public List<SNHttpRequestFile> Files { get; set; }

        public SNHttpClientRequest()
        {
            Method = HttpMethod.Get;
            Headers = new SNHttpHeaders();
            Cookies = new Dictionary<string, string>();
            Files = new List<SNHttpRequestFile>();
            Body = "";
            Path = "/";
        }

        public override string ToString()
        {
            var data = "";

            // Generate body and headers for files
            if (Files.Count > 0)
            {
                var boundary = "----------------------------" + Guid.NewGuid().ToString().Replace("-", "");

                // Generate upload data
                foreach (var file in Files)
                {
                    Body += boundary + "\r\n";
                    Body += $"Content-Disposition: form-data; name={file.Name}";

                    if (file.FileName != null)
                    {
                        Body += $"; filename=\"{file.FileName}\"\r\n";
                    }
                    else
                    {
                        Body += "\r\n";
                    }

                    if (file.ContentType != null)
                    {
                        Body += $"Content-Type: {file.ContentType}\r\n";
                    }

                    Body += "\r\n";
                    Body += Encoding.ASCII.GetString(file.Data.ToArray());
                    Body += "\r\n";
                }

                // Final boundary
                Body += boundary + "--\r\n";

                // Add or set the required headers
                if (!Headers.Contains("Content-Type"))
                {
                    Headers.Add("Content-Type", $"multipart/form-data; boundary={boundary}");
                }
                else
                {
                    Headers["Content-Type"] = $"multipart/form-data; boundary={boundary}";
                }
            }

            // Add missing headers
            if (!Headers.Contains("Content-Length") && Method != HttpMethod.Get)
            {
                Headers.Add("Content-Length", Body.Length.ToString());
            }
            if (!Headers.Contains("Content-Type") && Method != HttpMethod.Get)
            {
                Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            }

            // Start the first line with the http method
            data += Method.ToString().ToUpper() + " ";

            // Continue with the path
            data += Path;

            // And finish with the http version
            data += " HTTP/1.1\r\n";

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

            // Write down the body
            data += Body;

            return data;
        }
    }
}
