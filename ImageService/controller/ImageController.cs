
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService
{
    public class ImageController : IImageController
    {
        private IImageModel model;
        private Dictionary<int, ICommand> commands;
        private class TaskResult
        {
            public TaskResult(string result, bool boolean)
            {
                this.result = result;
                this.boolean = boolean;
            }
            public string result { get; set; }
            public bool boolean { get; set; }
        }
        public ImageController(IImageModel model)
        {
            this.model = model;
            commands = new Dictionary<int, ICommand>
            {
                { (int)CommandTypeEnum.ADD_FILE, new NewFileCommand(model) }
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (commands.ContainsKey(commandID))
            {
                Task<TaskResult> task = new Task<TaskResult>(() =>
                {
                    bool boolean;
                    ICommand command = commands[commandID];

                    string result = command.Execute(args, out boolean);
                    return new TaskResult(result, boolean);
                });
                task.Start();
                TaskResult t = task.Result;
                resultSuccesful = t.boolean;
                return t.result;
            }
            else
            {
                resultSuccesful = false;
                return "No such command.";
            }
        }
    }
}
