using System;

namespace Seanuts.WebSockets
{
    public enum WebSocketStatus
    {
        None,
        Opening,
        Open,
        Closing,
        Closed
    }

    public enum WebSocketMessageType
    {
        Text = 1,
        CloseConnection = 8
    }
}