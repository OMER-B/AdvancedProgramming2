using System;
using System.Windows;


namespace GUIProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void DataWindow_Closing(object sender, EventArgs e)
        {
            Tcp.TcpChannel.Instance.Disconnect();
        }
    }
}
