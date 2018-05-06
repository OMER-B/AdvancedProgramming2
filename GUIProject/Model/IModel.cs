using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Model
{
    interface IModel<T>
    {
        #region Notify Changed
        event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name);
        #endregion
        ObservableCollection<T> List { get; }
    }
}
