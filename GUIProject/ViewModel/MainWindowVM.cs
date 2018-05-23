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

        /// <summary>
        /// Checks if service is online to set background color.
        /// </summary>
        /// <returns>Background color as string</returns>
        public string isConnected()
        {

            if (Tcp.TcpChannel.Instance.Connected)
            {
                return "White";
            }
            return "Gray";

        }

        /// <summary>
        /// Disconnect when window closes.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
        void DataWindow_Closing(object sender, EventArgs e)
        {
            Tcp.TcpChannel.Instance.Disconnect();
        }
    }
}