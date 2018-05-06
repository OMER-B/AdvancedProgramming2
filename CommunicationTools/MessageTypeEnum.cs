using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationTools
{
    public enum MessageTypeEnum : int
    {
        REQ_HISTORY,
        REQ_CONFIG,
        CLOSE_HANDLER,
        SEND_HISTORY,
        SEND_LOG,
        SEND_CONFIG,
        SUCCESS,
        FAIL,
        DISCONNECT
    }
}
