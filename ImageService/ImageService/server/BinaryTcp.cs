using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationTools;
using Logic;
using Tools;

namespace ImageService
{
    class BinaryTcp : ITCP
    {
        private List<TcpClient> clients;
        private IPEndPoint ep;
        private TcpListener listener;
        private bool connected;
        private ILogger logger;
        private static Mutex mutex = new Mutex();
        private object locker = new object();

        public event EventHandler<ClientMessage> MessageFromClient;

        public BinaryTcp(ILogger logger)
        {
            this.logger = logger;
            this.clients = new List<TcpClient>();
            ep = ConnectionDetails.EndPoint;
            listener = new TcpListener(ep);
            connected = false;
        }

        public void Connect()
        {
            listener.Start();
            connected = true;
            Task task = new Task(() =>
            {
                try
                {
                    while (connected)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        clients.Add(client);
                        Task t = new Task(() => ListenToClient(client));
                        t.Start();
                    }
                }
                catch (Exception e)
                {
                    logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.FAIL, e.Message));
                }
            });
            task.Start();
        }

        public void DisconnectClient(TcpClient client)
        {
            try
            {
                client.Close();
                if (clients.Contains(client))
                {
                    clients.Remove(client);
                }
            }
            catch (Exception e)
            {
                logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.FAIL, e.Message));
            }
        }

        public void DisconnectAll()
        {
            if (clients.Count != 0)
            {
                foreach (TcpClient client in clients)
                {
                    client.Close();
                }
            }
            clients.Clear();
            listener.Stop();
            connected = false;
        }

        public void ListenToClient(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            try
            {
                while (client.Connected && nwStream.CanRead)
                {
                    byte[] bytes = new byte[client.ReceiveBufferSize];
                    if (nwStream.CanRead)
                    {
                        nwStream.Read(bytes, 0, bytes.Length);
                        MessageFromClient.Invoke(this, new ClientMessage(bytes));
                    }
                }
            }
            catch (Exception e)
            {
                logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.FAIL, e.Message));
            }
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
                {
                    DisconnectClient(client);
                }
                else
                {
                    try
                    {
                        NetworkStream nwStream = client.GetStream();
                        try
                        {
                            mutex.WaitOne();
                            nwStream.Write(message.Message, 0, message.Message.Length);
                            mutex.ReleaseMutex();
                        }
                        catch (Exception io)
                        {
                            mutex.ReleaseMutex();
                            throw io;
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.FAIL, e.Message));
                        if (!client.Connected)
                        {
                            DisconnectClient(client);
                        }
                    }
                }
            }

        }
    }
}