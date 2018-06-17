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
            byte[] approve = { 1 };
            NetworkStream stream = client.GetStream();
            stream.Flush();
            try
            {
                while (client.Connected && stream.CanRead)
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    int numBytesReadForNameSize = stream.Read(buffer, 0, client.ReceiveBufferSize);
                    int size;
                    if (numBytesReadForNameSize == 1)
                    {
                        size = Convert.ToInt32(buffer[0]);
                    }
                    else
                    {
                        byte[] realSize = new byte[numBytesReadForNameSize];
                        Array.Copy(buffer, 0, realSize, 0, numBytesReadForNameSize);
                        size = BitConverter.ToInt32(realSize, 0);
                    }

                    stream.Write(approve, 0, 1);

                    byte[] imageName = new byte[size];
                    int numBytesReadForName = stream.Read(imageName, 0, size);

                    string name = System.Text.Encoding.Default.GetString(imageName);

                    stream.Write(approve, 0, 1);

                    int numBytesReadForImageSize = stream.Read(buffer, 0, client.ReceiveBufferSize);
                    if (numBytesReadForImageSize == 1)
                    {
                        size = Convert.ToInt32(buffer[0]);
                    }
                    else
                    {
                        byte[] realSize = new byte[numBytesReadForImageSize];
                        Array.Copy(buffer, 0, realSize, 0, numBytesReadForImageSize);
                        size = BitConverter.ToInt32(realSize, 0);
                    }

                    stream.Write(approve, 0, 1);

                    byte[] image = new byte[size];
                    int numBytesReadForImage = stream.Read(image, 0, size);

                    MessageFromClient.Invoke(this, new ClientMessage(image));

                    stream.Write(approve, 0, 1);
                }
            }
            catch (Exception e)
            {
                logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.FAIL, e.Message));
                DisconnectClient(client);
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