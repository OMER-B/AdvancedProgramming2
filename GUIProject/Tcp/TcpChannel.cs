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

        public bool Connected { get { return this.connected; } }

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
            try
            {
                if (!connected)
                {
                    Connect();
                }
                System.Windows.MessageBox.Show("Sending message to server");
                writer.Write(message);
            }
            catch { }
        }

        public void ListenToServer()
        {
            System.Windows.MessageBox.Show("Listening to server");
            while (connected)
            {
                string message = reader.ReadString();
                DataRecieved.Invoke(this, new ClientMessage(message));
            }
        }

        public bool Connect()
        {
            try
            {
                client.Connect(ep);
                netStream = client.GetStream();
                reader = new BinaryReader(netStream);
                writer = new BinaryWriter(netStream);
                System.Windows.MessageBox.Show("Connected to server");
                connected = true;
                Task t = new Task(() => ListenToServer());
                t.Start();
                return true;
            }
            catch { return false; }
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
                System.Windows.MessageBox.Show("Disconnected");
            }

            client.Close();
        }

        ~TcpChannel()
        {
            Disconnect();
        }
    }
}
