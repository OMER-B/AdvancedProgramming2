using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using ImageService;

namespace Logic
{
    public interface ILogger
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        void Log(object sender, MessageRecievedEventArgs args);           // Logging the Message
    }
}
