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
            this.list = new ObservableCollection<TitleAndContent>();
            Object locker = new Object();
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(list, locker);
            TcpChannel.Instance.DataRecieved += GetData;
            System.Threading.Thread.Sleep(500);
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
                case MessageTypeEnum.SEND_LOG:
                    MessageBox.Show("Recieved log");
                    foreach (TitleAndContent t in tac.List)
                    {
                        this.list.Add(t);
                    }
                    break;
                case MessageTypeEnum.LOG_HISTORY:
                    foreach (TitleAndContent t in tac.List)
                    {
                        this.list.Add(t);
                    }
                    break;
                default: break;
            }
            OnPropertyChanged("list");
        }
    }
}
