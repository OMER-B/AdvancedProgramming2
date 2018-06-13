using CommunicationTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    interface ITCP
    {
        event EventHandler<ClientMessage> MessageFromClient;

        void MessageClients(object sender, ClientMessage message);

        void ListenToClient(TcpClient client);

        void Connect();

        void DisconnectClient(TcpClient client);

        void DisconnectAll();

    }
}
