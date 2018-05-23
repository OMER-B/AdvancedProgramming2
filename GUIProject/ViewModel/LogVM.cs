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
    class LogVM
    {
        private Model.ILogModel model;
        public ObservableCollection<TitleAndContent> LogList { get { return this.model.List; } }
        public ICommand RemoveCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor for LogModel.
        /// </summary>
        /// <param name="model">Model that implements that ILogModel interface.</param>
        public LogVM(Model.ILogModel model)
        {
            this.model = model;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };

        }

        /// <summary>
        /// Notify when property has changed, to invoke update.
        /// </summary>
        /// <param name="name">string that has changed.</param>
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
