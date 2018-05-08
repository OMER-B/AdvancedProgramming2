using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationTools
{
    class TACHolder
    {
        private List<TitleAndContent> list;
        private MessageTypeEnum commandID;

        [JsonProperty("List")]
        public List<TitleAndContent> List { get { return this.list; } set { this.list = value; } }

        [JsonProperty("CommandID")]
        public MessageTypeEnum CommandID { get { return this.commandID; } set { this.commandID = value; } }

        public TACHolder(MessageTypeEnum cmdID, List<TitleAndContent> list)
        {
            this.list = list;
            this.commandID = cmdID;
        }

        public TACHolder(TACHolder tac)
        {
            this.list = tac.List;
            this.commandID = tac.CommandID;
        }
        public TACHolder() { }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
