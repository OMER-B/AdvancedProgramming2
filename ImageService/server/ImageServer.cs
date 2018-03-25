
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
        private IImageModel imageModel;
        private ILogger logger;
        #endregion

        public ImageServer(ILogger logger)
        {
            this.logger = logger;
            this.imageModel = new ImageModel();
            this.controller = new ImageController(imageModel);
        }

        public void AddNewDirectoryHandler(string path)
        {
            IDirectoryHandler dirHandler = new DirectoyHandler(path);
            SendCommand += dirHandler.OnCommandRecieved;
            dirHandler.DirectoryClose += CloseHandler;
            
        }

        public void CloseHandler(object sender, DirectoryCloseEventArgs args)
        {
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