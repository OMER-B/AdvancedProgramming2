
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace ImageService.Logic.controller
{
    class ClientCommandController : IController
    {
       public delegate void CloseHandler(string path);
       private Dictionary<int, ICommand> commands;

       public ClientCommandController(CloseHandler closingFunction)
        {
            commands = new Dictionary<int, ICommand>
            {
                { (int)CommandTypeEnum.REMOVE_HANDLER, new RemoveHandlerCommand(closingFunction) }
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool result)
        {
            string resultString = "";
            if (commands.ContainsKey(commandID))
            {
                ICommand command = commands[commandID];
                resultString = command.Execute(args, out result);
                return resultString;
            }
            else
            {
                result = false;
                return "No such command.";
            }
        }
    }
}
