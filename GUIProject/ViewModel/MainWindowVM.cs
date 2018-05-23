using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.ViewModel
{
    class MainWindowVM
    {
        public string StatusColor { get { return isConnected(); } }

        public string isConnected()
        {

            if (Tcp.TcpChannel.Instance.Connected)
            {
                return "White";
            }
            return "Gray";

        }

        void DataWindow_Closing(object sender, EventArgs e)
        {
            Tcp.TcpChannel.Instance.Disconnect();
        }
    }
}