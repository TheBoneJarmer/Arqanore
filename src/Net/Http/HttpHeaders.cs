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
        }
        public string AcceptEncoding
        {
            get { return this["Accept-Encoding"]; }
        }
        public string Authorization
        {
            get { return this["Authorization"]; }
        }
        public string CacheControl
        {
            get { return this["Cache-Control"]; }
        }
        public string Connection
        {
            get { return this["Connection"]; }
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
        }
        public string ContentType
        {
            get { return this["Content-Type"]; }
        }
        public string Host
        {
            get { return this["Host"]; }
        }
        public string UserAgent
        {
            get { return this["User-Agent"]; }
        }


        // Indexer
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
