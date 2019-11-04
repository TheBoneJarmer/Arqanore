using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Arqanore.Net.Sockets;

namespace Arqanore.Net.WebSockets
{
    public class WebSocketRequest
    {
        public string Key { get; set; }
        public string Path { get; set; }
        public string Host { get; set; }
        public string Origin { get; set; }
        public string Protocol { get; set; }

        public WebSocketRequest()
        {
            Path = "/";
            Protocol = "chat, superchat";
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

                if (line.StartsWith("Sec-WebSocket-Key"))
                {
                    Key = line.Replace("Sec-WebSocket-Key: ", "");
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
            if (!connectionHeader.Split(',').Any(x => x.Trim() == "Upgrade"))
            {
                throw new WebSocketException("Invalid connection header. Expected: Upgrade");
            }
            if (upgradeHeader != "websocket")
            {
                throw new WebSocketException("Invalid upgrade header. Expected: websocket");
            }

            if (Key == "")
            {
                throw new WebSocketException("Header 'Sec-WebSocket-Key' is missing or empty");
            }
        }

        public string GenerateKey(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)new Random().Next(0, 255);
            }

            return Convert.ToBase64String(bytes);
        }
        public string GenerateKey()
        {
            return GenerateKey(new byte[16]);
        }

        public byte[] ToHttpRequest()
        {
            string data = "";

            data += "GET " + Path + "HTTP/1.1\r\n";
            data += "Host: " + Host + "\r\n";
            data += "Upgrade: websocket\r\n";
            data += "Connection: Upgrade\r\n";
            data += "Sec-WebSocket-Key: " + Key + "\r\n";
            data += "Origin: " + Origin + "\r\n";
            data += "Sec-WebSocket-Protocol: " + Protocol + "\r\n";
            data += "Sec-WebSocket-Version: 13\r\n";
            data += "\r\n";

            return Encoding.ASCII.GetBytes(data);
        }
    }
}