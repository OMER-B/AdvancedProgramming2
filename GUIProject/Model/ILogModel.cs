﻿using CommunicationTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Model
{
    interface ILogModel
    {
        #region Notify Changed
        event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name);
        #endregion
        ObservableCollection<TitleAndContent> List { get; set; }
        void GetData(object sender, ClientMessage data);
    }
}