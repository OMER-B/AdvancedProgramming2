using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Logic
{
    class ApplicationController : IController<byte>
    {
        Dictionary<int, ICommand<byte>> commands;

        public ApplicationController(IImageModel model)
        {
            this.commands = new Dictionary<int, ICommand<byte>>();
            commands.Add((int)ImageCommandTypeEnum.RECIEVED_PHOTO, new BytesToImageCommand(model) );
            
        }

        public string ExecuteCommand(int commandID, byte[] args, out bool result)
        {
            if (commands.ContainsKey(commandID))
            {
                ICommand<byte> toExecute = commands[commandID];
                string stringResult = toExecute.Execute(args, out result);
                return stringResult;
            }
            result = false;
            return "No such command.";
        }
    }
}
