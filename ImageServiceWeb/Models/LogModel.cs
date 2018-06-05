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
        static bool called = false;
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
            if (called == false)
            {

                if (!TcpClientChannel.Instance.Connected)
                {
                    TcpClientChannel.Instance.Connect();
                }
                this.list.Clear();
                TcpClientChannel.Instance.SendMessage(new TACHolder(MessageTypeEnum.LOG_HISTORY, null).ToJson());
            }
            called = true;
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
                case MessageTypeEnum.LOG_HISTORY:
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