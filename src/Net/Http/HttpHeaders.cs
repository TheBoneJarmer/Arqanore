using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Net.Http
{
    public class HttpHeaders
    {
        private Dictionary<string, string> Headers { get; set; }

        // Pre-defined
        public string Accept
        {
            get { return this["Accept"]; }
            set { this["Accept"] = value; }
        }
        public string AcceptEncoding
        {
            get { return this["Accept-Encoding"]; }
            set { this["Accept-Encoding"] = value; }
        }
        public string Authorization
        {
            get { return this["Authorization"]; }
            set { this["Authorization"] = value; }
        }
        public string CacheControl
        {
            get { return this["Cache-Control"]; }
            set { this["Cache-Control"] = value; }
        }
        public string Connection
        {
            get { return this["Connection"]; }
            set { this["Connection"] = value; }
        }
        public int ContentLength
        {
            get
            {
                var value = this["Content-Length"];

                if (value == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(value);
                }
            }
            set
            {
                this["Content-Length"] = value.ToString();
            }
        }
        public string ContentType
        {
            get { return this["Content-Type"]; }
            set { this["Content-Type"] = value; }
        }
        public string Host
        {
            get { return this["Host"]; }
            set { this["Host"] = value; }
        }
        public string UserAgent
        {
            get { return this["User-Agent"]; }
            set { this["User-Agent"] = value; }
        }

        public string this[string key]
        {
            get
            {
                if (Headers.ContainsKey(key.ToLower()))
                {
                    return Headers[key.ToLower()];
                }

                return null;
            }
            set
            {
                if (this[key] == null)
                {
                    Headers.Add(key.ToLower(), value);
                }
                else
                {
                    Headers[key.ToLower()] = value;
                }
            }
        }

        public HttpHeaders()
        {
            Headers = new Dictionary<string, string>();
        }

        public void Add(string name, string value)
        {
            if (this[name] != null)
            {
                throw new InvalidOperationException("Header already exists");
            }

            Headers.Add(name.ToLower(), value.Replace("\r\n", ""));
        }
        public bool Contains(string name)
        {
            return this[name] != null;
        }

        public override string ToString()
        {
            var result = "";

            foreach (var header in Headers)
            {
                result += header.Key + ": " + header.Value + "\r\n";
            }

            return result;
        }
        public List<string> ToList()
        {
            var result = new List<string>();

            foreach (var header in Headers)
            {
                result.Add(header.Key + ": " + header.Value);
            }

            return result;
        }
    }
}
