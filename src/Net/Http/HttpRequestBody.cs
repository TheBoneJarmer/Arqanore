using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Arqanore.Net.Http
{
    public class HttpRequestBody
    {
        private string data;

        internal HttpRequestBody(string data, string contentType)
        {
            if (data.Length > 0)
            {
                if (contentType == "application/x-www-form-urlencoded" || contentType == "multipart/form-data")
                {
                    this.data = JsonConvert.SerializeObject(data.Split('&').ToDictionary(v => v.Split('=')[0], v => v.Split('=')[1]));
                }
                else
                {
                    this.data = data;
                }
            }
        }

        public override string ToString()
        {
            return data;
        }
    }
}
