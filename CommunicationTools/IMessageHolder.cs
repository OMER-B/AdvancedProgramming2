using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationTools
{
    interface IMessageHolder
    {
        List<TitleAndContent> List { get; set; }
        MessageTypeEnum CommandID { get; set; }
        string ToJson();
        IMessageHolder FromJson(string json);
    }
}
