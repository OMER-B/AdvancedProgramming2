using CommunicationTools;
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

        public void SendClientsLog(object sender, MessageRecievedEventArgs args)
        {
            List<TitleAndContent> tacList = new List<TitleAndContent>
            {
                new TitleAndContent(args.Status.ToString(), args.Message)
            };
            TACHolder tac = new TACHolder(MessageTypeEnum.SEND_LOG, tacList);

            string message = tac.ToJson();
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
                while (connected)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        logger.Log(this, new MessageRecievedEventArgs(LogMessageTypeEnum.INFO, "Connection established with: " + client.ToString()));
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
