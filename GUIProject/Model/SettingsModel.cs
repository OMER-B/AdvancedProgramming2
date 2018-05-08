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

            this.list = new ObservableCollection<TitleAndContent>();
            List<TitleAndContent> listt = new List<TitleAndContent>();
            listt.Add(new TitleAndContent("Output Directory", "C:\\Users\\Omer\\Desktop\\test"));
            listt.Add(new TitleAndContent("Source Name", "ImageServiceSource"));
            listt.Add(new TitleAndContent("Log Name", "ImageServiceLog"));
            listt.Add(new TitleAndContent("Thumbnail Size", "120"));

            TACHolder tac = new TACHolder(CommunicationTools.MessageTypeEnum.SEND_CONFIG, listt);
            string output = JsonConvert.SerializeObject(tac);
            GUIDistributionParser p = new GUIDistributionParser(output, this, null);
            p.passToModel();

            this.handlersList = new ObservableCollection<TitleAndContent>();
            List<TitleAndContent> listt2 = new List<TitleAndContent>();

            listt2.Add(new TitleAndContent("Path", "C:"));
            listt2.Add(new TitleAndContent("Path", "C:\\Users"));
            listt2.Add(new TitleAndContent("Path", "C:\\Users\\Omer"));
            listt2.Add(new TitleAndContent("Path", "C:\\Users\\Omer\\Desktop\\test"));
            listt2.Add(new TitleAndContent("Path", "D:"));
            listt2.Add(new TitleAndContent("Path", "D:\\Downloads"));
            listt2.Add(new TitleAndContent("Path", "D:\\Files"));

            TACHolder tac2 = new TACHolder(CommunicationTools.MessageTypeEnum.SEND_CONFIG, listt2);
            string output2 = JsonConvert.SerializeObject(tac2);
            GUIDistributionParser p2 = new GUIDistributionParser(output2, this, null);
            p2.passToModel();

        }

        private ObservableCollection<TitleAndContent> list;
        public ObservableCollection<TitleAndContent> List { get { return this.list; } set { this.list = value; } }

        internal void Remove(TitleAndContent selectedHandler)
        {
            // TODO: Here need to call the client to remove from the server! If successful then do
            try
            {
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
    }
}
