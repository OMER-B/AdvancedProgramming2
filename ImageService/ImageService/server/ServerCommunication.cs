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
        private static Mutex mutex = new Mutex();

        public event EventHandler<ClientMessage> MessageFromClient;

        public ServerCommunication(ILogger logger)
        {
            this.logger = logger;
            this.clients = new List<TcpClient>();
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            listener = new TcpListener(ep);
            connected = false;

        }

        public void MessageClients(object sender, ClientMessage message)
        {
            if (!connected)
            {
                Connect();
            }
            if (clients.Count == 0)
            {
                return;
            }
            foreach (TcpClient client in clients)
            {
                if (!client.Connected)
                    continue;
                try
                {
                    NetworkStream nwStream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(nwStream);
                    mutex.WaitOne();
                    writer.Write(message.Message);
                    mutex.ReleaseMutex();
                }
                catch (Exception e)
                {
                    logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.FAIL, e.Message));
                }
            }
        }

        public void SendClientsLog(object sender, LogMessageArgs args)
        {
            if (clients.Count == 0)
            {
                return;
            }
            List<TitleAndContent> tacList = new List<TitleAndContent>
            {
                new TitleAndContent(args.Status.ToString(), args.Message)
            };
            TACHolder tac = new TACHolder(MessageTypeEnum.SEND_LOG, tacList);

            string message = tac.ToJson();

            MessageClients(this, new ClientMessage(message));
        }

        public void ListenToClient(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            BinaryReader reader = new BinaryReader(nwStream);
            string line;
            try
            {
                while (client.Connected)
                {
                    if ((line = reader.ReadString()) != null)
                    {
                        MessageFromClient.Invoke(this, new ClientMessage(line));
                    }
                }
            }
            catch { DisconnectClient(client); }
        }

        public void Connect()
        {
            listener.Start();
            connected = true;
            Task task = new Task(() =>
            {
                while (connected)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    clients.Add(client);
                    Task t = Task.Factory.StartNew(() => ListenToClient(client));
                }
            });
            task.Start();
        }

        public void DisconnectClient(TcpClient client)
        {

            client.Close();
            if (clients.Contains(client))
            {
                clients.Remove(client);
            }
        }

        public void DisconnectAll()
        {
            if (clients.Count != 0)
            {
                foreach (TcpClient client in clients)
                {
                    client.Close();
                    WriteToLog("Disconnected from: " + client.ToString());
                }
            }
            listener.Stop();
            connected = false;
        }

        private void WriteToLog(string toWrite)
        {
            logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.INFO, toWrite));

        }

    }
}
