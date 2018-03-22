using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public interface ILogger
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        void Log(object sender, MessageRecievedEventArgs args);           // Logging the Message
    }
}
