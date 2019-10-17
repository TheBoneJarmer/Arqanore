using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Net.Http
{
    public class SNHttpRequestFile
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public List<byte> Data { get; set; }

        public SNHttpRequestFile()
        {
            Data = new List<byte>();
        }
    }
}
