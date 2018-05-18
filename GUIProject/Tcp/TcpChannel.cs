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
        private static System.Threading.Mutex mutex = new System.Threading.Mutex();
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
            Connect();
        }

        public void SendMessage(string message)
        {
            try
            {
                if (!connected)
                {
                    Connect();
                }
                mutex.WaitOne();
                writer.Write(message);
                mutex.ReleaseMutex();
            }
            catch { }
        }

        public void ListenToServer()
        {
            string message;
            try
            {
                while (client.Connected)
                {
                    if ((message = reader.ReadString()) != null)
                    {
                        DataRecieved.Invoke(this, new ClientMessage(message));
                    }
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Connection disabled");
            }
        }

        public void Connect()
        {
            try
            {
                client.Connect(ep);
                netStream = client.GetStream();
                reader = new BinaryReader(netStream);
                writer = new BinaryWriter(netStream);
                connected = true;
                Task t = new Task(() => ListenToServer());
                t.Start();
            }
            catch { connected = false; }
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
