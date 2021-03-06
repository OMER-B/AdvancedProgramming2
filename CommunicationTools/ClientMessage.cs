﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationTools
{
    public class ClientMessage : EventArgs
    {
        private string message;

        public string Message { get { return this.message; } }

        /// <summary>
        /// The client message. Used as holder class.
        /// </summary>
        /// <param name="message">string of the message.</param>
        public ClientMessage(string message)
        {
            this.message = message;
        }
    }
}
