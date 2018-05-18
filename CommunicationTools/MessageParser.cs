using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationTools
{
    class MessageParser
    {
        public static TACHolder ParseJsonToTAC(string json)
        {
            TACHolder tac = new TACHolder(JsonConvert.DeserializeObject<TACHolder>(json));
            return tac;
        }

        public static Tuple<int, string[]> parseMessageToCommand(ClientMessage message)
        {
            TACHolder holder = MessageParser.ParseJsonToTAC(message.Message);
            int command = (int)holder.CommandID;
            string[] args;
            if (holder.List != null && holder.List.Count > 0)
            {
                args = new string[holder.List.Count * 2];
                int i = 0;
                foreach (TitleAndContent t in holder.List)
                {
                    args[i] = t.Title;
                    args[i + 1] = t.Content;
                    i += 2;
                }
            }
            else
            {
                args = new string[1];
            }
            return new Tuple<int, string[]>(command, args);
        }
    }
}
