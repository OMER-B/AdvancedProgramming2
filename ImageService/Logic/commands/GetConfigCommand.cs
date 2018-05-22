using CommunicationTools;
using ImageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class GetConfigCommand : ICommand
    {
        public event EventHandler<ClientMessage> SendConfig;
        private ConfigReader reader;

        public GetConfigCommand(ConfigReader reader)
        {
            this.reader = reader;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                List<TitleAndContent> list = new List<TitleAndContent>();
                for(int i = 0; i < reader.Handler.Count; i++)
                {
                    list.Add(new TitleAndContent("Path", reader.Handler[i]));
                }
                list.Add(new TitleAndContent("OutputDir", reader.OutputDir));
                list.Add(new TitleAndContent("SourceName", reader.SourceName));
                list.Add(new TitleAndContent("LogName", reader.LogName));
                list.Add(new TitleAndContent("ThumbnailSize", reader.ThumbnailSize.ToString()));

                TACHolder holder = new TACHolder(MessageTypeEnum.APP_CONFIG, list);
                string stringConfig = holder.ToJson();
                SendConfig.Invoke(this, new ClientMessage(stringConfig));
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
