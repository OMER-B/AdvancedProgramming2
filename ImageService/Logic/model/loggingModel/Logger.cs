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
        private List<LogMessageArgs> history;
        public List<LogMessageArgs> History
        {
            get { return this.history; }
            set { }
        }

        public event EventHandler<LogMessageArgs> MessageRecieved;

        public Logger()
        {
            this.history = new List<LogMessageArgs>();
        }

        public void Log(object sender, LogMessageArgs args)
        {
            MessageRecieved.Invoke(sender, args);
            history.Add(args);
        }
    }
}
