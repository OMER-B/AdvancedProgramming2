using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CommunicationTools
{
    public class ConnectionDetails
    {
        private static IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        public static IPEndPoint EndPoint { get => endPoint; }
    }
}
