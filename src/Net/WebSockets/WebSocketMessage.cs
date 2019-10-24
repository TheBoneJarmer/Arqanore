using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Net.WebSockets
{
    public class WebSocketMessage
    {
        public byte[] Buffer { get; private set; }
        public string Message { get; private set; }
        public WebSocketMessageType Type { get; private set; }

        public WebSocketMessage(byte[] data)
        {
            this.Buffer = data;
        }
        public WebSocketMessage(string message, WebSocketMessageType type)
        {
            this.Message = message;
            this.Type = type;
        }

        public void Decode()
        {
            int byteIndex = 0;

            // Primary byte
            int finished = ByteToBinaryIntArray(Buffer[byteIndex])[0];
            int rsv1 = ByteToBinaryIntArray(Buffer[byteIndex])[1];
            int rsv2 = ByteToBinaryIntArray(Buffer[byteIndex])[2];
            int rsv3 = ByteToBinaryIntArray(Buffer[byteIndex])[3];
            int opcode = BinaryStringToInt(ByteToBinaryString(Buffer[byteIndex]).Substring(4, 4));

            byteIndex += 1;

            // Just a text message
            if (opcode == 1)
            {
                // Masked bool and payload length byte
                int masked = ByteToBinaryIntArray(Buffer[byteIndex])[0];
                int payloadLengthValue = BinaryStringToInt(ByteToBinaryString(Buffer[byteIndex]).Substring(1, 7));
                int payloadLength = 0;

                // Correct the payload length
                if (payloadLengthValue < 125)
                {
                    payloadLength = payloadLengthValue;
                    byteIndex += 1;
                }
                if (payloadLengthValue == 126)
                {
                    string value1 = ByteToBinaryString(Buffer[byteIndex + 1]);
                    string value2 = ByteToBinaryString(Buffer[byteIndex + 2]);

                    payloadLength = BinaryStringToInt(value1 + value2);

                    byteIndex += 3;
                }
                if (payloadLengthValue == 127)
                {
                    string value1 = ByteToBinaryString(Buffer[byteIndex + 1]);
                    string value2 = ByteToBinaryString(Buffer[byteIndex + 2]);
                    string value3 = ByteToBinaryString(Buffer[byteIndex + 3]);
                    string value4 = ByteToBinaryString(Buffer[byteIndex + 4]);

                    payloadLength = BinaryStringToInt(value1 + value2 + value3 + value4);

                    byteIndex += 5;
                }

                // Mask bytes
                if (masked == 1)
                {
                    byte[] maskBytes = new byte[4];
                    maskBytes[0] = Buffer[byteIndex + 0];
                    maskBytes[1] = Buffer[byteIndex + 1];
                    maskBytes[2] = Buffer[byteIndex + 2];
                    maskBytes[3] = Buffer[byteIndex + 3];

                    byteIndex += 4;

                    // Payload bytes
                    byte[] payload = new byte[payloadLength];

                    for (int i=0; i<payloadLength; i++)
                    {
                        byte encoded = Buffer[byteIndex + i];
                        byte decoded = (byte)(encoded ^ maskBytes[i % 4]);

                        payload[i] = decoded;
                    }

                    // Convert the payload
                    Message = Encoding.ASCII.GetString(payload);
                }
                else
                {
                    // Payload bytes
                    byte[] payload = new byte[payloadLength];

                    for (int i=0; i<payloadLength; i++)
                    {
                        payload[i] = Buffer[byteIndex + i];
                    }

                    // Convert the payload
                    Message = Encoding.ASCII.GetString(payload);
                }
            }

            // Closing connection
            if (opcode == 8)
            {
                
            }

            // Set the message type
            Type = (WebSocketMessageType)opcode;
        }
        public void Encode(bool masked = false)
        {
            List<byte> result = new List<byte>();

            // Primary byte
            result.Add((byte)BinaryStringToInt("1000" + IntToBinaryString((int)Type).PadLeft(4, '0')));

            if (!masked)
            {
                // Masked bool and payload length byte
                if (Message.Length < 126)
                {
                    result.Add((byte)BinaryStringToInt("0" + IntToBinaryString(Message.Length).PadLeft(7, '0')));
                }
                if (Message.Length >= 126)
                {
                    result.Add((byte)BinaryStringToInt("0" + IntToBinaryString(126).PadLeft(7, '0')));
                    result.Add((byte)BinaryStringToInt(IntToBinaryString(Message.Length).PadLeft(16, '0').Substring(0, 8)));
                    result.Add((byte)BinaryStringToInt(IntToBinaryString(Message.Length).PadLeft(16, '0').Substring(8, 8)));
                }

                // Payload bytes
                for (int i=0; i<Message.Length; i++)
                {
                    result.Add((byte)Message[i]);
                }
            }
            else
            {
                byte[] maskBytes = new byte[4];
                maskBytes[0] = (byte)new Random().Next(0, 255);
                maskBytes[1] = (byte)new Random().Next(0, 255);
                maskBytes[2] = (byte)new Random().Next(0, 255);
                maskBytes[3] = (byte)new Random().Next(0, 255);

                // Masked bool and payload length byte
                if (Message.Length < 126)
                {
                    result.Add((byte)BinaryStringToInt("1" + IntToBinaryString(Message.Length).PadLeft(7, '0')));
                }
                if (Message.Length >= 126)
                {
                    result.Add((byte)BinaryStringToInt("1" + IntToBinaryString(126).PadLeft(7, '0')));
                    result.Add((byte)BinaryStringToInt(IntToBinaryString(Message.Length).PadLeft(16, '0').Substring(0, 7)));
                    result.Add((byte)BinaryStringToInt(IntToBinaryString(Message.Length).PadLeft(16, '0').Substring(8, 15)));
                }

                // Masked bytes
                result.AddRange(maskBytes);

                // Payload bytes
                for (int i=0; i<Message.Length; i++)
                {
                    byte decoded = (byte)Message[i];
                    byte encoded = (byte)(decoded ^ maskBytes[i % 4]);

                    result.Add(encoded);
                }
            }

            // Convert the result
            Buffer = result.ToArray();
        }

        private string ByteToBinaryString(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }
        private char[] ByteToBinaryCharArray(byte b)
        {
            return ByteToBinaryString(b).ToCharArray();
        }
        private int[] ByteToBinaryIntArray(byte b)
        {
            char[] binaryCharArray = ByteToBinaryCharArray(b);
            int[] output = new int[8];

            for (int i=0; i<8; i++)
            {
                output[i] = int.Parse(binaryCharArray[i].ToString());
            }

            return output;
        }

        private string IntToBinaryString(int i)
        {
            return Convert.ToString(i, 2);
        }

        private int BinaryStringToInt(string s)
        {
            int output = 0;
            int x = 1;
            
            for(int i=s.Length - 1; i>-1; i--)
            {
                int value = int.Parse(s[i].ToString());

                if (value == 1)
                {
                    output += x;
                }

                x *= 2;
            }

            return output;
        }
    }
}