using CommunicationTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace GUIProject.Tcp
{
    class TcpChannel
    {
        private TcpClient client;
        private IPEndPoint ep;
        private bool connected;
        public event EventHandler<ClientMessage> DataRecieved;
        NetworkStream netStream;
        BinaryReader reader;
        BinaryWriter writer;

        public bool Connected{ get { return this.connected; } }

        // The tcp channel is a singleton
        private static TcpChannel instance;
        public static TcpChannel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TcpChannel();
                }
                return instance;
            }
        }

        private TcpChannel()
        {
            // TODO recieve the connection data in constructor
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            connected = false;
            Connect();
            SendMessage("test");
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
                DataRecieved.Invoke(this, new ClientMessage(message));
            }
        }

        public bool Connect()
        {
            client.Connect(ep);
            netStream = client.GetStream();
            reader = new BinaryReader(netStream);
            writer = new BinaryWriter(netStream);
            Console.WriteLine("You are connected");
            connected = true;
            netStream = client.GetStream();
            try
            {
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
            netStream.Close();
            connected = false;
        }

        ~TcpChannel()
        {
            Disconnect();
        }
    }
}
