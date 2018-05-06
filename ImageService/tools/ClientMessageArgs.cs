using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    class ClientMessageArgs
    {
        private ImageCommandTypeEnum commandId;
        private string[] args;

        public ClientMessageArgs(ImageCommandTypeEnum id, string[] args)
        {
            this.commandId = id;
            this.args = args;
        }
    }
}
