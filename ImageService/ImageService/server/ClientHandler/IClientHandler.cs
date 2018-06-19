using CommunicationTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    interface IClientHandler
    {
        void HandleRequest(object sender, ClientMessage message);
    }
}
