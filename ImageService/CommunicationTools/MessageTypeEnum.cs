using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationTools
{
    public enum MessageTypeEnum : int
    {
        LOG_HISTORY,
        APP_CONFIG,
        CLOSE_HANDLER,
        SEND_LOG,
        SUCCESS,
        FAIL,
        SERVICE_STATUS,
        DISCONNECT
    }
}
