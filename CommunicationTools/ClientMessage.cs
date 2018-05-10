using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationTools
{
    class ClientMessage : EventArgs
    {
        private string message;

        public string Message { get { return this.message; } }

        public ClientMessage(string message)
        {
            this.message = message;
        }
    }
}
