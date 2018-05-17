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
        List<LogMessageArgs> History { get; set; }

        event EventHandler<LogMessageArgs> MessageRecieved;
        void Log(object sender, LogMessageArgs args);           // Logging the Message
    }
}
