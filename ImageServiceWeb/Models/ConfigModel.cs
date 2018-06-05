using CommunicationTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        private string outputDir;
        private bool recievedConfig = false;
        private bool recievedRemoved = false;
        public string OutputDir { get { return outputDir; } set => outputDir = value; }

        private List<TitleAndContent> handlersList;
        public List<TitleAndContent> HandlersList { get { return this.handlersList; } }
        private List<TitleAndContent> list;
        public List<TitleAndContent> List { get { return this.list; } set { this.list = value; } }

        private string selectedHandler;
        public string SelectedHandler { get { return selectedHandler; } set { selectedHandler = value; } }

        /// <summary>
        /// Constructor for SettingsModel.
        /// </summary>
        public ConfigModel()
        {
            TcpClientChannel.Instance.DataRecieved += GetData;
            this.list = new List<TitleAndContent>();
            this.handlersList = new List<TitleAndContent>();
            recievedConfig = false;
        }

        public void Initialize()
        {
            if (recievedConfig) return;
            if (!TcpClientChannel.Instance.Connected)
            {
                TcpClientChannel.Instance.Connect();
            }
            TcpClientChannel.Instance.SendMessage(new TACHolder(MessageTypeEnum.APP_CONFIG, null).ToJson());
        }


        /// <summary>
        /// Remove a path handler from the handlers list.
        /// </summary>
        /// <param name="selectedHandler">What to remove from list.</param>
        public void Remove(string name)
        {
            recievedRemoved = false;
            TitleAndContent selectedHandler = new TitleAndContent("Path", name);
            TACHolder tac = new TACHolder(MessageTypeEnum.CLOSE_HANDLER, new List<TitleAndContent> { selectedHandler });
            string json = tac.ToJson();

            try
            {
                TcpClientChannel.Instance.SendMessage(json);
                while (!recievedRemoved) { System.Threading.Thread.Sleep(500); }

            }
            catch { }
        }




        /// <summary>
        /// The function activated by the recieve data of the tcp-channel.
        /// determine if the type of the message is relevant (app config or remove handler)
        /// and acitivate on property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public void GetData(object sender, ClientMessage data)
        {
            TACHolder tac = CommunicationTools.MessageParser.ParseJsonToTAC(data.Message);
            switch (tac.CommandID)
            {
                case MessageTypeEnum.APP_CONFIG:
                    recievedConfig = true;
                    foreach (TitleAndContent t in tac.List)
                    {
                        if (t.Title.ToLower() != "path")
                        {
                            if (t.Title.ToLower().Equals("OutputDir".ToLower()))
                            {
                                this.outputDir = t.Content;
                            }
                                this.list.Add(t);
                        }
                        else
                        {
                            this.handlersList.Add(t);
                        }
                    }
                    break;
                case MessageTypeEnum.CLOSE_HANDLER:
                    recievedRemoved = true;
                    TitleAndContent input = tac.List[0];

                    for (int i = 0; i < this.handlersList.Count; i++)
                    {
                        if (input.Content.Equals(handlersList[i].Content))
                        {
                            this.handlersList.Remove(handlersList[i]);
                            break;
                        }
                    }

                    break;
                default: break;
            }
        }

    }

}