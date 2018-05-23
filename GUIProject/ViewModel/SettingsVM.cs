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
    class SettingsVM
    {
        private Model.ISettingsModel model;
        public ObservableCollection<TitleAndContent> ConfigList { get { return this.model.List; } }
        public ObservableCollection<TitleAndContent> HandlersList { get { return this.model.HandlersList; } }
        public ICommand RemoveCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor for SettingsVM.
        /// </summary>
        /// <param name="model">Model that implements the ISettingsModel interface to set as the model.</param>
        public SettingsVM(Model.ISettingsModel model)
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

        /// <summary>
        /// Remove handler from list of handlers.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
        private void RemoveHandler(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command?.RaiseCanExecuteChanged();

        }

        /// <summary>
        /// Property to set the currently selected handler from the view.
        /// </summary>
        public TitleAndContent SelectedHandler
        {
            get { return this.model.SelectedHandler; }
            set
            {
                this.model.SelectedHandler = value;
                NotifyPropertyChanged("SelectedHandler");
            }
        }

        /// <summary>
        /// What to do when remove button is clicked.
        /// </summary>
        /// <param name="obj">object</param>
        public void OnRemove(object obj)
        {
            this.model.Remove(this.SelectedHandler);
            this.model.SelectedHandler = null;
        }

        /// <summary>
        /// Checks if handler can be removed.
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>boolean true if yes, false otherwise.</returns>
        private bool CanRemove(object obj)
        {
            return this.model.HandlersList.Contains(SelectedHandler);
        }

        /// <summary>
        /// Notifies when property has changed for the view.
        /// </summary>
        /// <param name="name">string changed.</param>
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
