
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class ImageServer
    {

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> SendCommand;
        
        #endregion

        #region Members
        private IImageController controller;
        private ILogger logger;
        #endregion

        public ImageServer(ILogger logger)
        {
            this.logger = logger;
            // TODO update the variables
            this.controller = new ImageController(new ImageModel(null, 0));
        }

        public void AddNewDirectoryHandler(string path)
        {
            string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp" };
            IDirectoryHandler dirHandler = new DirectoyHandler(path, controller, logger, extensions);
            SendCommand += dirHandler.OnCommandRecieved;
            dirHandler.DirectoryClose += CloseHandler;
            dirHandler.StartHandleDirectory();
            
        }

        public void CloseHandler(object sender, DirectoryCloseEventArgs args)
        {
            //args.DirectoryPath
            if(sender is IDirectoryHandler)
            {
                IDirectoryHandler h = (IDirectoryHandler)sender;
                SendCommand -= h.OnCommandRecieved;
            }
        }

        public void CloseAll()
        {

        }
       
    }
}