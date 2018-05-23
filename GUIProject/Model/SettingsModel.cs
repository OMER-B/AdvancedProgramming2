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

        private ObservableCollection<TitleAndContent> handlersList;
        public ObservableCollection<TitleAndContent> HandlersList { get { return this.handlersList; } }

        private TitleAndContent selectedHandler;
        private bool recievedData = false;
        public SettingsModel()
        {
            TcpChannel.Instance.DataRecieved += GetData;
            this.list = new ObservableCollection<TitleAndContent>();
            this.handlersList = new ObservableCollection<TitleAndContent>();
            Object locker = new Object();
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(handlersList, locker);
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
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    this.handlersList.Remove(selectedHandler);
                });
            }
            catch { }
        }

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
                    if (recievedData)
                    {
                        break;
                    }
                    recievedData = true;
                    foreach (TitleAndContent t in tac.List)
                    {
                        if (t.Title.ToLower() != "path")
                        {
                            App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                this.list.Add(t);
                            });
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                this.handlersList.Add(t);
                            });
                        }

                    }
                    break;
                case MessageTypeEnum.CLOSE_HANDLER:
                    TitleAndContent input = tac.List[0];

                    for (int i = 0; i < this.handlersList.Count; i++)
                    {
                        if (input.Content.Equals(handlersList[i].Content))
                        {
                            App.Current.Dispatcher.Invoke((System.Action)delegate
                            {
                                this.handlersList.Remove(handlersList[i]);
                            });
                            break;
                        }
                    }

                    OnPropertyChanged("handlersList");
                    break;
                default: break;
            }
        }
    }
}
