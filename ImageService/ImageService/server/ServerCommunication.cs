using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace ImageService
{
    class ServerCommunication
    {
        // TODO make a list of clients
        private List<TcpClient> clients;
        private ILogger logger;
        private IPEndPoint ep;
        private TcpListener listener;
        private bool connected;

        public ServerCommunication(ILogger logger)
        {
            this.logger = logger;
            this.clients = new List<TcpClient>();
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            listener = new TcpListener(ep);
            connected = false;
        }

        public void MessageClients(string message)
        {
            // TODO be invoked by the logger
            foreach(TcpClient client in clients)
            {
                using (StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII))
                {
                    writer.Write(message);
                }              
            }
        }

        public void ListenToClient(TcpClient client)
        {
            using (StreamReader reader = new StreamReader(client.GetStream()))
            {
                string message = reader.ReadLine();
            }
        }


        public void Connect()
        {
            listener.Start();
            connected = true;
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        logger.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, "Connection established with: " + client.ToString()));
                        clients.Add(client);
                        Task t = Task.Factory.StartNew(() => ListenToClient(client));
                        Thread.Sleep(1000);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

        public void Disconnect()
        {
            foreach (TcpClient client in clients)
            {
                client.Close();
            }
            listener.Stop();
            connected = false;
        }

    }
}
