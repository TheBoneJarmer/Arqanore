using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Net.Http
{
    public class HttpRequestFile
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public List<byte> Data { get; set; }

        public HttpRequestFile()
        {
            Data = new List<byte>();
        }
    }
}
