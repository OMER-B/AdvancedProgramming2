using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using ImageService;

namespace Tools
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public LogMessageTypeEnum Status { get; set; }
        public string Message { get; set; }
        public MessageRecievedEventArgs(LogMessageTypeEnum status, string message)
        {
            this.Status = status;
            this.Message = message;            
        }
    }
}
