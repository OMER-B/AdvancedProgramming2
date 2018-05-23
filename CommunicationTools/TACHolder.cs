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

        /// <summary>
        /// Constructor for TACHolder.
        /// </summary>
        /// <param name="cmdID">Command ID</param>
        /// <param name="list">List to send.</param>
        public TACHolder(MessageTypeEnum cmdID, List<TitleAndContent> list)
        {
            this.list = list;
            this.commandID = cmdID;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="tac">TACHolder to copy from.</param>
        public TACHolder(TACHolder tac)
        {
            this.list = tac.List;
            this.commandID = tac.CommandID;
        }
        public TACHolder() { }

        /// <summary>
        /// Converts to json
        /// </summary>
        /// <returns>String of JSON.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
