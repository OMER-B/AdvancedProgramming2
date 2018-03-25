
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class ImageController : IImageController
    {
        private IImageModel model;
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageModel model)
        {
            this.model = model;
            commands = new Dictionary<int, ICommand>
            {
                { 1, new NewFileCommand(model) }
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (commands.ContainsKey(commandID))
            {
                ICommand command = commands[commandID];
                return command.Execute(args, out resultSuccesful);
            } else
            {
                resultSuccesful = false;
                return "No Such Command";
            }
        }
    }
}
