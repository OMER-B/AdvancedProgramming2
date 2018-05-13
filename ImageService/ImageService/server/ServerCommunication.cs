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
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            foreach (TcpClient client in clients)
            {
                NetworkStream nwStream = client.GetStream();
                nwStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void ListenToClient(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            while (client.Connected)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                if(bytesRead > 0)
                {
                    //---convert the data received into a string---
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    // TODO activate event in server
                    WriteToLog("Got message: " + message + " num of bytes: " + bytesRead.ToString());
                }
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
                        WriteToLog("connection with: " + client.ToString());
                        clients.Add(client);
                        Task t = Task.Factory.StartNew(() => ListenToClient(client));
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
            });
            task.Start();
        }

        public void Disconnect()
        {
            if(clients.Count != 0)
            {
                foreach (TcpClient client in clients)
                {
                    client.Close();
                }
            }
            listener.Stop();
            connected = false;
        }

        private void WriteToLog(string toWrite)
        {
            logger.Log(this, new MessageRecievedEventArgs(LogMessageTypeEnum.INFO, toWrite));

        }

    }
}
