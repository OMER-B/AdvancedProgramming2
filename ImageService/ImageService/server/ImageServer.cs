
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Logic;

namespace ImageService
{
    public class ImageServer
    {

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> SendCommand;
        public event EventHandler<DirectoryCloseEventArgs> StopHandler;
        #endregion

        #region Members
        private IImageController controller;
        private ILogger logger;
        #endregion

        public ImageServer(ILogger logger, IImageModel imageModel)
        {
            this.logger = logger;
            this.controller = new ImageController(imageModel);
        }

        public void AddNewDirectoryHandler(string path)
        {
            string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp" };
            IDirectoryHandler dirHandler = new DirectoyHandler(path, controller, logger, extensions);
            SendCommand += dirHandler.OnCommandRecieved;
            StopHandler += dirHandler.CloseFileWatcher;
            dirHandler.DirectoryClose += CloseHandler;
            dirHandler.StartHandleDirectory();
        }

        public void CloseHandler(object sender, DirectoryCloseEventArgs args)
        {
            //args.DirectoryPath
            if (sender is IDirectoryHandler)
            {
                IDirectoryHandler h = (IDirectoryHandler)sender;
                SendCommand -= h.OnCommandRecieved;
            }
        }

        public void CloseAll()
        {
            StopHandler.Invoke(this, new DirectoryCloseEventArgs("*", null));
        }

    }
}