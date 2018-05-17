using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationTools;
using ImageService;
using Tools;

namespace Logic
{
    class GetLogHistoryCommand : ICommand
    {
        private ILogger logger;
        public event EventHandler<ClientMessage> SendHistory;

        public GetLogHistoryCommand(ILogger logger)
        {
            this.logger = logger;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                List<LogMessageArgs> history = logger.History;
                List<TitleAndContent> list = new List<TitleAndContent>();
                foreach (LogMessageArgs s in history)
                {
                    list.Add(new TitleAndContent(s.Status.ToString(), s.Message));
                }
                TACHolder holder = new TACHolder(MessageTypeEnum.SEND_HISTORY, list);
                string stringHistory = holder.ToJson();
                SendHistory.Invoke(this, new ClientMessage(stringHistory));
                result = true;
                return "";
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
