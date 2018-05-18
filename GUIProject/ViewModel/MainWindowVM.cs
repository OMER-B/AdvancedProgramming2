using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.ViewModel
{
    class MainWindowVM
    {
        private Model.MainWindowModel model;
        public string StatusColor { get { return isConnected(); } }

        public MainWindowVM()
        {
            this.model = new Model.MainWindowModel();
        }

        public string isConnected()
        {
            try
            {
                System.Windows.MessageBox.Show("Connecting");
                Tcp.TcpChannel.Instance.Connect();
                return "White";
            }
            catch
            {
                return "Gray";
            }
        }
    }
}