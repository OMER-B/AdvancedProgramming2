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
        
        /// <summary>
        /// find the path of the directory to close and invoke an event to update client.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            DirectoryCloseEventArgs commandargs = new DirectoryCloseEventArgs(args[1], null);
            CloseDirectory.Invoke(this, commandargs);
            result = true;
            return "Closing Directory";
        }
    }
}
