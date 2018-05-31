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
    class LogModel : ILogModel, INotifyPropertyChanged
    {
        private ObservableCollection<TitleAndContent> list;
        public ObservableCollection<TitleAndContent> List { get { return this.list; } set { this.list = value; } }
        public event PropertyChangedEventHandler PropertyChanged;
        private bool recievedHistory = false;

        /// <summary>
        /// Constructor for LogModel.
        /// </summary>
        public LogModel()
        {
            this.list = new ObservableCollection<TitleAndContent>();
            Object locker = new Object();
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(list, locker);
            TcpChannel.Instance.DataRecieved += GetData;
            System.Threading.Thread.Sleep(500);
            TcpChannel.Instance.SendMessage(new TACHolder(MessageTypeEnum.LOG_HISTORY, null).ToJson());
        }
        
        /// <summary>
        /// OnPropertyChanged method to update when selection is not available.
        /// </summary>
        /// <param name="name">String changed.</param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            this.list.Add(t);
                        });
                    }
                    break;
                case MessageTypeEnum.LOG_HISTORY:
                    if (recievedHistory)
                    {
                        break;
                    }
                    recievedHistory = true;
                    foreach (TitleAndContent t in tac.List)
                    {
                        App.Current.Dispatcher.Invoke((System.Action)delegate
                        {
                            this.list.Add(t);
                        });

                    }
                    break;
                default: break;
            }
            OnPropertyChanged("list");
        }
    }
}
