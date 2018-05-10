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
        private StreamWriter writer;
        private StreamReader reader;

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
                // message is TacHolder in json
                string message = reader.ReadLine();
                DataRecieved.Invoke(this, new ClientMessage(message));
            }
        }

        public bool Connect()
        {
            try
            {
                client.Connect(ep);
                Console.WriteLine("You are connected");
                connected = true;

                writer = new StreamWriter(client.GetStream());
                reader = new StreamReader(client.GetStream());
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
            writer.Close();
            reader.Close();
            connected = false;
        }

        ~TcpChannel()
        {
            Disconnect();
        }
    }
}
