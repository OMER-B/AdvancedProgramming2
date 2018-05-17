using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationTools
{
    static class MessageParser
    {
        public static TACHolder ParseJsonToTAC(string json)
        {
            return new TACHolder(JsonConvert.DeserializeObject<TACHolder>(json));
        }

        public static Tuple<int, string[]> parseMessageToCommand(ClientMessage message)
        {
            TACHolder holder = ParseJsonToTAC(message.Message);
            int command = (int)holder.CommandID;
            string[] args = null;
            if (holder.List.Count > 0)
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
            return new Tuple<int, string[]>(command, args);
        }
    }
}
