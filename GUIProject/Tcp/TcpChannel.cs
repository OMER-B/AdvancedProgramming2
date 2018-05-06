using CommunicationTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tools;


namespace GUIProject.Tcp
{
    class TcpChannel
    {
        private TcpClient client;
        private IPEndPoint ep;
        private bool connected;
        public event EventHandler<ClientMessageArgs> DataRecieved;
        private BinaryWriter writer;
        private BinaryReader reader;

        // TODO: the log and settings should get the data recieved

        public TcpChannel()
        {
            // TODO recieve the connection data in constructor
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            connected = false;
        }

        public bool SendMessage(string message)
        {
            if (!connected)
            {
                bool result = Connect();                
                if (!result)
                {
                    return false;
                }
            }

            writer.Write(message);
            return true;
        }

        public void ListenToServer()
        {
            while (connected)
            {
                string message = reader.ReadString();
                string[] args = message.Split();
                    //DataRecieved.Invoke(this, new ClientMessageArgs()
            }
        }

        public bool Connect()
        {
            try
            {
                client.Connect(ep);
                Console.WriteLine("You are connected");
                connected = true;
                writer = new BinaryWriter(client.GetStream());
                reader = new BinaryReader(client.GetStream());
                Task t = new Task(() => ListenToServer());
                t.Start();
                return true;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            client.Close();
            connected = false;
        }

        ~TcpChannel()
        {
            Disconnect();
        }
    }
}
