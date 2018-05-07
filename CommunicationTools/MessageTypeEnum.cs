﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationTools
{
    public enum MessageTypeEnum : int
    {
        REQ_HISTORY,
        SEND_HISTORY,

        REQ_CONFIG,
        SEND_CONFIG,

        CLOSE_HANDLER,
        SEND_LOG,

        SUCCESS,
        FAIL,

        REQ_SERVICE_STATUS,
        SEND_SERVICE_STATUS,

        DISCONNECT
    }
}
