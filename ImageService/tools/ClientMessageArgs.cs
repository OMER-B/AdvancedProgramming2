using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    class ClientMessageArgs
    {
        private CommandTypeEnum commandId;
        private string[] args;

        public ClientMessageArgs(CommandTypeEnum id, string[] args)
        {
            this.commandId = id;
            this.args = args;
        }
    }
}
