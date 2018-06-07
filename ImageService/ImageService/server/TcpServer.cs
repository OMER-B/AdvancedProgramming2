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
    class TcpServer : ITcpServer
    {
        private List<TcpClient> clients;
        private ILogger logger;
        private IPEndPoint ep;
        private TcpListener listener;
        private bool connected;
        private static Mutex mutex = new Mutex();
        private object locker = new object();

        public event EventHandler<ClientMessage> MessageFromClient;

        /// <summary>
        /// Constructor for ServerCommuncation.
        /// </summary>
        /// <param name="logger">Logger to write to.</param>
        public TcpServer(ILogger logger)
        {
            this.logger = logger;
            this.clients = new List<TcpClient>();
            ep = ConnectionDetails.EndPoint;
            listener = new TcpListener(ep);
            connected = false;

        }

        /// <summary>
        /// Sending a message to all of the clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
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
                } else
                {
                    try
                    {
                        NetworkStream nwStream = client.GetStream();
                        BinaryWriter writer = new BinaryWriter(nwStream);
                        try
                        {
                            mutex.WaitOne();
                            writer.Write(message.Message);
                            mutex.ReleaseMutex();
                        } catch (Exception io)
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

        /// <summary>
        /// Get notified by the remove handler command that a handler was removed
        /// then need too send the message to all of the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnClientRemoveHandler(object sender, DirectoryCloseEventArgs args)
        {
            string path = args.DirectoryPath;
            TACHolder holder = new TACHolder(MessageTypeEnum.CLOSE_HANDLER, new List<TitleAndContent> { new TitleAndContent("Path", path) });
            MessageClients(sender, new ClientMessage(holder.ToJson()));
        }

        /// <summary>
        /// Get notified when the logger gets a new messgae
        /// then send the message to the clients.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnClientsLog(object sender, LogMessageArgs args)
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

        /// <summary>
        /// A task happening in a different thread.
        /// This task is listening to *each* client and notifies the server for each new message.
        /// </summary>
        /// <param name="client"></param>
        public void ListenToClient(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            BinaryReader reader = new BinaryReader(nwStream);
            string line;
            try
            {
                while (client.Connected && reader.BaseStream.CanRead)
                {
                    if ((line = reader.ReadString()) != null)
                    {
                        MessageFromClient.Invoke(this, new ClientMessage(line));
                    }
                }
            }
            catch (Exception e)
            {
                logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.FAIL, e.Message));
            }
        }

        /// <summary>
        /// activate the listener, and accept client in a new task.
        /// then, for each new connected client create a new task to handle it
        /// and continue listening to new clients.
        /// </summary>
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

        /// <summary>
        /// Close a client socket and remove it from client list.
        /// </summary>
        /// <param name="client"></param>
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

        /// <summary>
        /// Close all the client socket and clear the client list.
        /// </summary>
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

        /// <summary>
        /// For debugging purposes:
        /// Send to a debug log error and inormation messages.
        /// </summary>
        /// <param name="toWrite"></param>
        private void WriteToLog(string toWrite)
        {
            DebugLogger.Instance.Log(this, new LogMessageArgs(LogMessageTypeEnum.INFO, toWrite));
        }

    }
}
