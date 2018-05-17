using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    class ListStringArgs : EventArgs
    {
        private List<string> args;

        public List<string> Args
        {
            get { return this.args; }
        }

        public ListStringArgs(List<string> args)
        {
            this.args = args;
        }
    }
}
