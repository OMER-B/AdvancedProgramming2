using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Communication
{
    class ConnectionClient
    {
        private IPEndPoint ep;

        public ConnectionClient()
        {
            this.ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        }

        public string ConnectToServer(string message)
        {
            string result = "";
            TcpClient client = new TcpClient();
            client.Connect(ep);
            Console.WriteLine("You are connected");
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(message);
                // Get result from server
                result = reader.ReadString();
            }
            client.Close();
            return result;
        }
    }
}
