using CommunicationTools;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logic.controller
{
    class ServerController : IController
    {
        private Dictionary<int, ICommand> commands;
        public ServerController(ICommand removeHandler, ICommand logHistory, ICommand config)
        {
            this.commands = new Dictionary<int, ICommand>();
            commands[(int)MessageTypeEnum.CLOSE_HANDLER] = removeHandler;
            commands[(int)MessageTypeEnum.REQ_HISTORY] = logHistory;
            commands[(int)MessageTypeEnum.REQ_CONFIG] = config;
        }

        public string ExecuteCommand(int commandID, string[] args, out bool result)
        {
            string resultString;
            if (commands.ContainsKey(commandID))
            {
                ICommand command = commands[commandID];
                resultString = command.Execute(args, out result);
            } else
            {
                result = false;
                resultString = "No such command";
            }
            return resultString;
        }
    }
}
