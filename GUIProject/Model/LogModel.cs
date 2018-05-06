using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Model
{
    class LogModel : IModel<Log>, INotifyPropertyChanged
    {
        public LogModel()
        {
            this.list = new ObservableCollection<Log>();
            this.list.Add(new Log("INFO", "no problem"));
            this.list.Add(new Log("WARNING", "problem"));
            this.list.Add(new Log("FAIL", "ok"));
            this.list.Add(new Log("FAIL", "not good"));
            this.list.Add(new Log("INFO", "very bad"));
            this.list.Add(new Log("FAIL", "good"));
            this.list.Add(new Log("INFO", "stop"));
            this.list.Add(new Log("FAIL", "good"));
            this.list.Add(new Log("INFO", "oll korrect"));
        }

        private ObservableCollection<Log> list;
        public ObservableCollection<Log> List { get { return this.list; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
