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
            this.list = new ObservableCollection<TitleAndContent>();
            TitleAndContent test = new TitleAndContent("info", "ok");
            TitleAndContent test2 = new TitleAndContent("warning", "bad");
            List<TitleAndContent> listt = new List<TitleAndContent>();
            listt.Add(test);
            listt.Add(test2);


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
    }
}
