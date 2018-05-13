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
    class LogModel : IModel, INotifyPropertyChanged
    {
        public LogModel()
        {

            TcpChannel.Instance.DataRecieved += GetData;

            this.list = new ObservableCollection<TitleAndContent>();

            TitleAndContent test = new TitleAndContent("info", "ok");
            List<TitleAndContent> listt = new List<TitleAndContent>();
            listt.Add(test);


            TACHolder tac = new TACHolder(MessageTypeEnum.SEND_HISTORY, listt);

            string output = JsonConvert.SerializeObject(tac);
            GUIDistributionParser p = new GUIDistributionParser(output, null, this);
            p.passToModel();
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
            GUIDistributionParser p = new GUIDistributionParser(data.Message, null, this);
            p.passToModel();
            
        }
    }
}
