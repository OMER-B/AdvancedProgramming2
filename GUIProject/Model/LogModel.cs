using CommunicationTools;
using GUIProject.Tcp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUIProject.Model
{
    class LogModel : IModel, INotifyPropertyChanged
    {
        public LogModel()
        {
            TcpChannel.Instance.DataRecieved += GetData;
            this.list = new ObservableCollection<TitleAndContent>();
            TcpChannel.Instance.SendMessage(new TACHolder(MessageTypeEnum.LOG_HISTORY, null).ToJson());
        }

        private ObservableCollection<TitleAndContent> list;
        public ObservableCollection<TitleAndContent> List { get { return this.list; } set { this.list = value; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void GetData(object sender, ClientMessage data)
        {
            TACHolder tac = CommunicationTools.MessageParser.ParseJsonToTAC(data.Message);
            switch (tac.CommandID)
            {

                case MessageTypeEnum.LOG_HISTORY:
                case MessageTypeEnum.SEND_LOG:
                    foreach (TitleAndContent t in tac.List)
                    {
                        this.list.Add(t);
                    }
                    break;

                default: break;
            }
            throw new NotImplementedException();

        }
    }
}
