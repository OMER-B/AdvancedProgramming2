using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace ImageService
{
    class ServerCommunication
    {
        // TODO make a list of clients
        private List<TcpClient> clients;
        private ILogger logger;

        public ServerCommunication(ILogger logger)
        {
            this.logger = logger;
            this.clients = new List<TcpClient>();
        }

        public bool Connect()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                TcpListener listener = new TcpListener(ep);
                listener.Start();
                Console.WriteLine("Waiting for client connections...");
                TcpClient client = listener.AcceptTcpClient();
                logger.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, "Connection established with: " + client.ToString()));
                clients.Add(client);

                using (NetworkStream stream = client.GetStream())
                //using (BinaryReader reader = new BinaryReader(stream))
                //using (BinaryWriter writer = new BinaryWriter(stream))

                client.Close();
                listener.Stop();
                return true;
            }catch(Exception e)
            {
                //TODO write to log
                Console.WriteLine(e.Message);
                return false;
            }
            

        }

    }
}
