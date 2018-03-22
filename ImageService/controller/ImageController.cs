
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class ImageController : IImageController
    {
        private IImageModel model;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageModel model)
        {
            this.model = model;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
				// For Now will contain NEW_FILE_COMMAND
            };
        }
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            // Write Code Here
        }
    }
}
