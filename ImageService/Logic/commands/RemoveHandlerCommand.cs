
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageService.Logic.controller.ClientCommandController;

namespace ImageService.Logic
{
    class RemoveHandlerCommand : ICommand
    {
        private CloseHandler closingFunction;

        public RemoveHandlerCommand(CloseHandler closingFunction)
        {
            this.closingFunction = closingFunction;
        }

        public string Execute(string[] args, out bool result)
        {
            string path = args[1];
            throw new NotImplementedException();
        }
    }
}
