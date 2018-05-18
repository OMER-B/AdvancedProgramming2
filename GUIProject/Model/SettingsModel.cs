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

namespace GUIProject.Model
{
    class SettingsModel : IModel, INotifyPropertyChanged
    {
        public SettingsModel()
        {
            TcpChannel.Instance.DataRecieved += GetData;
            this.list = new ObservableCollection<TitleAndContent>();
            this.handlersList = new ObservableCollection<TitleAndContent>();
            Object locker = new Object();
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(handlersList, locker);
            System.Threading.Thread.Sleep(500);
            TcpChannel.Instance.SendMessage(new TACHolder(MessageTypeEnum.APP_CONFIG, null).ToJson());
        }

        private ObservableCollection<TitleAndContent> list;
        public ObservableCollection<TitleAndContent> List { get { return this.list; } set { this.list = value; } }

        internal void Remove(TitleAndContent selectedHandler)
        {
            TACHolder tac = new TACHolder(MessageTypeEnum.CLOSE_HANDLER, new List<TitleAndContent> { selectedHandler });
            string json = tac.ToJson();

            try
            {
                TcpChannel.Instance.SendMessage(json);
                this.handlersList.Remove(selectedHandler);
            }
            catch { }
        }

        private ObservableCollection<TitleAndContent> handlersList;
        public ObservableCollection<TitleAndContent> HandlersList { get { return this.handlersList; } }

        private TitleAndContent selectedHandler;
        public TitleAndContent SelectedHandler
        {
            get { return selectedHandler; }
            set
            {
                selectedHandler = value;
                OnPropertyChanged("SelectedHandler");
            }
        }

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
                case MessageTypeEnum.APP_CONFIG:
                    foreach (TitleAndContent t in tac.List)
                    {
                        if (t.Title.ToLower() != "path")
                        {
                            this.list.Add(t);
                        }
                        else
                        {
                            this.handlersList.Add(t);
                        }

                    }
                    break;
                case MessageTypeEnum.CLOSE_HANDLER:
                    foreach (TitleAndContent t in tac.List)
                    {
                        if (this.handlersList.Contains(t))
                        {
                            this.handlersList.Remove(t);
                        }
                    }
                    OnPropertyChanged("handlersList");
                    break;
                default: break;
            }
        }
    }
}
