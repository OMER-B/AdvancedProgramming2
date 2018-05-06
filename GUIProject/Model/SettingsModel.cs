using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Model
{
    class SettingsModel : IModel<ConfigHolder>, INotifyPropertyChanged
    {
        public SettingsModel()
        {
            string[] a = { "Output Directory", "C:\\Users\\Omer\\Desktop\\test" };
            string[] b = { "Source Name", "ImageServiceSource" };
            string[] c = { "Log Name", "ImageServiceLog" };
            string[] d = { "Thumbnail Size", "120" };
            this.list = new ObservableCollection<ConfigHolder>();
            this.list.Add(new ConfigHolder(a[0], a[1]));
            this.list.Add(new ConfigHolder(b[0], b[1]));
            this.list.Add(new ConfigHolder(c[0], c[1]));
            this.list.Add(new ConfigHolder(d[0], d[1]));


            this.handlersList = new ObservableCollection<ConfigHolder>();
            this.handlersList.Add(new ConfigHolder("Path", "C:\\Users\\Omer\\Desktopaeyi7"));
            this.handlersList.Add(new ConfigHolder("Path", "C:\\Users\\Omer\\absd"));
            this.handlersList.Add(new ConfigHolder("Path", "C:\\Users\\Omer\\D345y"));
            this.handlersList.Add(new ConfigHolder("Path", "C:\\Users\\Omer\\hsdfg3"));
            this.handlersList.Add(new ConfigHolder("Path", "C:\\Users\\Omer\\g55y"));
            this.handlersList.Add(new ConfigHolder("Path", "C:\\Users\\Omer\\546h"));
            this.handlersList.Add(new ConfigHolder("Path", "C:\\Users\\Omer\\bdfh"));

        }

        private ObservableCollection<ConfigHolder> list;
        public ObservableCollection<ConfigHolder> List { get { return this.list; } }

        internal void Remove(ConfigHolder selectedHandler)
        {
            // TODO: Here need to call the client to remove from the server! If successful then do
            try
            {
                this.handlersList.Remove(selectedHandler);
            }
            catch { }
        }

        private ObservableCollection<ConfigHolder> handlersList;
        public ObservableCollection<ConfigHolder> HandlersList { get { return this.handlersList; } }

        private ConfigHolder selectedHandler;
        public ConfigHolder SelectedHandler
        {
            get { return selectedHandler; }
            set
            {
                selectedHandler = value;
                OnPropertyChanged("SelectedHandler");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
