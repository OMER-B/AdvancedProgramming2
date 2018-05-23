using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Logic
{
    class DebugLogger : ILogger
    {
        private static DebugLogger instance;
        private DebugLogger() { }
        public static DebugLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DebugLogger();
                }
                return instance;
            }
        }
        private List<LogMessageArgs> history;

        public List<LogMessageArgs> History { get => this.history; set { } }

        public event EventHandler<LogMessageArgs> MessageRecieved;

        public void Log(object sender, LogMessageArgs args)
        {
            MessageRecieved.Invoke(sender, args);

        }
    }
}
