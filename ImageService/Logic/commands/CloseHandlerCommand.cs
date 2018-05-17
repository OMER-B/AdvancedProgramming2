using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Logic
{
    class CloseHandlerCommand : ICommand
    {
        public event EventHandler<DirectoryCloseEventArgs> CloseDirectory;
        
        public string Execute(string[] args, out bool result)
        {
            DirectoryCloseEventArgs commandargs = new DirectoryCloseEventArgs(args[1], null);
            CloseDirectory.Invoke(this, commandargs);
            result = true;
            return "Closing Directory";
        }
    }
}
