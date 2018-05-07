using CommunicationTools;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUIProject.ViewModel
{
    class LogVM : IViewModel
    {
        private Model.IModel model;
        public ObservableCollection<TitleAndContent> LogList { get { return this.model.List; } }
        public ICommand RemoveCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public LogVM(Model.IModel model)
        {
            this.model = model;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };

        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
