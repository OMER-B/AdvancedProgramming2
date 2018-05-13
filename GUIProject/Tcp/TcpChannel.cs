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
        }

        public void SendMessage(string message)
        {
            if (!connected)
            {
                Connect();
            }
            writer.Write(message);
        }

        public void ListenToServer()
        {
            while (connected)
            {
                string message = reader.ReadString();
                DataRecieved.Invoke(this, new ClientMessage(message));
            }
        }

        public void Connect()
        {
            client.Connect(ep);
            netStream = client.GetStream();
            reader = new BinaryReader(netStream);
            writer = new BinaryWriter(netStream);
            connected = true;
            Task t = new Task(() => ListenToServer());
            t.Start();
        }

        public void Disconnect()
        {
            if (connected)
            {
            TACHolder holder = new TACHolder(MessageTypeEnum.DISCONNECT, null);
            writer.Write(holder.ToJson());
            writer.Close();
            reader.Close();
            netStream.Close();
            connected = false;
            }
            client.Close();
        }

        ~TcpChannel()
        {
            Disconnect();
        }
    }
}
