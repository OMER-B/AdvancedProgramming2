using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using ImageService;

namespace Logic
{
    public class Logger : ILogger
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(object sender, MessageRecievedEventArgs args)
        {
            MessageRecieved.Invoke(sender, args);
            //string message, MessageTypeEnum type
        }
    }
}
