using CommunicationTools;
using GUIProject.Model;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUIProject.ViewModel
{
    class SettingsVM : IViewModel
    {
        private Model.SettingsModel model;
        public ObservableCollection<TitleAndContent> ConfigList { get { return this.model.List; } }
        public ObservableCollection<TitleAndContent> HandlersList { get { return this.model.HandlersList; } }
        public ICommand RemoveCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsVM(Model.SettingsModel model)
        {
            this.model = model;
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            PropertyChanged += this.RemoveHandler;
            this.model.PropertyChanged +=
       delegate (Object sender, PropertyChangedEventArgs e)
       {
           NotifyPropertyChanged(e.PropertyName);
       };

        }
        private void RemoveHandler(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command?.RaiseCanExecuteChanged();

        }
        public TitleAndContent SelectedHandler
        {
            get { return this.model.SelectedHandler; }
            set
            {
                this.model.SelectedHandler = value;
                NotifyPropertyChanged("SelectedHandler");
            }
        }

        public void OnRemove(object obj)
        {
            this.model.Remove(this.SelectedHandler);
            this.model.SelectedHandler = null;
        }

        private bool CanRemove(object obj)
        {
            return this.model.HandlersList.Contains(SelectedHandler);
        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
