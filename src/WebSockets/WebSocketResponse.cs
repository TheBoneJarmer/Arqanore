using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using Seanuts.Sockets;

namespace Seanuts.WebSockets
{
    public class WebSocketResponse
    {
        public string Key { get; set; }

        public WebSocketResponse()
        {

        }

        public void Parse(string data)
        {
            // For validation
            string connectionHeader = "";
            string upgradeHeader = "";

            // Convert all carriage returns and newlines to just newlines and split the string on the newlines
            string[] lines = data.Replace("\r\n", "\n").Split('\n');

            // Iterate over all lines and parse them
            foreach (string line in lines)
            {
                if (line.StartsWith("Connection"))
                {
                    connectionHeader = line.Replace("Connection: ", "");
                }
                if (line.StartsWith("Upgrade"))
                {
                    upgradeHeader = line.Replace("Upgrade: ", "");
                }

                if (line.StartsWith("Sec-WebSocket-Accept"))
                {
                    Key = line.Replace("Sec-WebSocket-Accept: ", "");
                }
            }

            // Validate the input
            if (connectionHeader == "")
            {
                throw new WebSocketException("Header 'Connection' is missing or empty");
            }
            if (upgradeHeader == "")
            {
                throw new WebSocketException("Header 'Upgrade' is missing or empty");
            }
            if (connectionHeader != "Upgrade")
            {
                throw new WebSocketException("Invalid connection header. Expected: Upgrade");
            }
            if (upgradeHeader != "websocket")
            {
                throw new WebSocketException("Invalid upgrade header. Expected: websocket");
            }

            if (Key == "")
            {
                throw new WebSocketException("Header 'Sec-WebSocket-Accept' is missing or empty");
            }
        }

        public string GenerateKey(string webSocketRequestKey)
        {
            SHA1 sha1 = SHA1.Create();

            // First, concatenate the input with a guid specified by RFC 6455
            string concatenation = webSocketRequestKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

            // Second, convert the result of the concatenation to bytes
            byte[] concatenationBytes = Encoding.ASCII.GetBytes(concatenation);

            // Third, hash those bytes using the SHA1 algorithm
            byte[] concatenationBytesHashed = sha1.ComputeHash(concatenationBytes);

            // And finally, convert the result to a base64 string
            string output = Convert.ToBase64String(concatenationBytesHashed);

            return output;
        }

        public byte[] ToHttpResponse()
        {
            string data = "";

            data += "HTTP/1.1 101 Switching Protocols\r\n";
            data += "Connection: Upgrade\r\n";
            data += "Upgrade: websocket\r\n";
            data += "Sec-WebSocket-Accept: " + Key + "\r\n";
            data += "\r\n";

            return Encoding.ASCII.GetBytes(data);
        }
    }
}