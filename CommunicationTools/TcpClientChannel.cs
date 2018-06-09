using CommunicationTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace CommunicationTools
{
    class TcpClientChannel
    {
        private TcpClient client;
        private IPEndPoint endPoint;
        private NetworkStream netStream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private static System.Threading.Mutex mutex = new System.Threading.Mutex();
        private bool connected;
        public bool Connected { get { return this.connected; } }

        public event EventHandler<ClientMessage> DataRecieved;

        private static TcpClientChannel instance;

        /// <summary>
        /// TCP Channel is a singleton.
        /// </summary>
        public static TcpClientChannel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TcpClientChannel();
                }
                return instance;
            }
        }

        /// <summary>
        /// Constructor, initialize a connection with the server.
        /// </summary>
        private TcpClientChannel()
        {
            this.endPoint = ConnectionDetails.EndPoint;
            client = new TcpClient();
            connected = false;
            Connect();
        }

        /// <summary>
        /// Send a message from the model to the server.
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            try
            {
                if (!connected)
                {
                    Connect();
                }
                // use a mutex when sending a message.
                mutex.WaitOne();
                writer.Write(message);
                mutex.ReleaseMutex();
            }
            catch
            {
                // Here debug
            }
        }

        /// <summary>
        /// A task to listen to the server in a different thread
        /// and invoke the event of data-recieved.
        /// </summary>
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
                // Here debug
            }
        }

        /// <summary>
        /// Create a connection with the server
        /// if successfull, create a task to listen to updates from the server.
        /// </summary>
        public void Connect()
        {
            try
            {
                client.Connect(endPoint);
                netStream = client.GetStream();
                reader = new BinaryReader(netStream);
                writer = new BinaryWriter(netStream);
                connected = true;
                Task t = new Task(() => ListenToServer());
                t.Start();
            }
            catch
            {
                connected = false;
            }
        }

        /// <summary>
        /// Disconnet: close the socket, update the server,
        /// close the networkstream, reader and writer.
        /// </summary>
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

        /// <summary>
        /// Destructor: call disconnect.
        /// </summary>
        ~TcpClientChannel()
        {
            Disconnect();
        }
    }
}
