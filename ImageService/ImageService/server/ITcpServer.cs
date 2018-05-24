using CommunicationTools;
using System;
using System.Net.Sockets;
using Tools;

namespace ImageService
{
    interface ITcpServer
    {
        event EventHandler<ClientMessage> MessageFromClient;
        void MessageClients(object sender, CommunicationTools.ClientMessage message);
        void OnClientRemoveHandler(object sender, DirectoryCloseEventArgs args);

        void OnClientsLog(object sender, LogMessageArgs args);

        void ListenToClient(TcpClient client);

        void Connect();

        void DisconnectClient(TcpClient client);

        void DisconnectAll();
    }
}
