using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arqanore.Net.Http
{
    public class HttpRequestBody
    {
        private Dictionary<string, string> keyValuePairs;
        private string json;

        public string this[string key]
        {
            get
            {
                if (keyValuePairs != null && keyValuePairs.ContainsKey(key))
                {
                    return keyValuePairs[key];
                }

                return null;
            }
        }

        internal HttpRequestBody(string data, string contentType)
        {
            if (contentType == "application/json")
            {
                json = data;
                return;
            }
            if (contentType == "application/x-www-form-urlencoded")
            {
                if (data.Length > 0)
                {
                    keyValuePairs = data.Split('&').ToDictionary(v => v.Split('=')[0], v => v.Split('=')[1]);
                }
                else
                {
                    keyValuePairs = new Dictionary<string, string>();
                }

                return;
            }
            if (contentType == "multipart/form-data")
            {
                if (data.Length > 0)
                {
                    keyValuePairs = data.Split('&').ToDictionary(v => v.Split('=')[0], v => v.Split('=')[1]);
                }
                else
                {
                    keyValuePairs = new Dictionary<string, string>();
                }

                return;
            }
        }

        public override string ToString()
        {
            if (keyValuePairs != null)
            {
                var data = "";

                foreach (var entry in keyValuePairs)
                {
                    if (data == "")
                    {
                        data = entry.Key + "=" + entry.Value;
                    }
                    else
                    {
                        data += "&" + entry.Key + "=" + entry.Value;
                    }
                }

                return data;
            }

            if (json != null)
            {
                return json;
            }

            return "";
        }
        public Dictionary<string, string> ToDictionary()
        {
            return keyValuePairs;
        }
    }
}
