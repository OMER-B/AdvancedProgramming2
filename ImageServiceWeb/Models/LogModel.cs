using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using CommunicationTools;

namespace ImageServiceWeb.Models
{
    public class LogModel
    {
        private List<TitleAndContent> list;
        public List<TitleAndContent> List { get { return this.list; } set { this.list = value; } }
        private bool recieved;

        /// <summary>
        /// Constructor for LogModel.
        /// </summary>
        public LogModel()
        {
            this.list = new List<TitleAndContent>();
            TcpClientChannel.Instance.DataRecieved += GetData;
        }

        public void Initialize()
        {

            if (!TcpClientChannel.Instance.Connected)
            {
                TcpClientChannel.Instance.Connect();
            }
            recieved = false;
            TcpClientChannel.Instance.SendMessage(new TACHolder(MessageTypeEnum.LOG_HISTORY, null).ToJson());
            int i = 0;
            while (!recieved)
            {
                System.Threading.Thread.Sleep(500);
                if (i++ == 4)
                    break;
            }
        }

        /// <summary>
        /// The function called by the event of the tcp-channel.
        /// when a message is recieved check if it is relevant to
        /// the log model (send log or log history)
        /// activate the on property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public void GetData(object sender, ClientMessage data)
        {
            TACHolder tac = MessageParser.ParseJsonToTAC(data.Message);
            switch (tac.CommandID)
            {
                case MessageTypeEnum.SEND_LOG:
                    foreach (TitleAndContent t in tac.List)
                    {
                        this.list.Add(t);
                    }
                    break;
                case MessageTypeEnum.LOG_HISTORY:
                    list.Clear();
                    recieved = true;
                    foreach (TitleAndContent t in tac.List)
                    {
                        this.list.Add(t);
                    }
                    break;
                default: break;
            }
        }
    }
}