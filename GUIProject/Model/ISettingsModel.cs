using CommunicationTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Model
{
    interface ISettingsModel
    {
        ObservableCollection<TitleAndContent> HandlersList { get; }
        ObservableCollection<TitleAndContent> List { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        TitleAndContent SelectedHandler { get; set; }
        void Remove(TitleAndContent selectedHandler);
        void OnPropertyChanged(string name);
        void GetData(object sender, ClientMessage data);
    }
}
