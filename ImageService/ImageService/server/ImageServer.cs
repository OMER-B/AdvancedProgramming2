
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
        private ServerCommunication communication;
        private IController imageController;
        private ILogger logger;
        private string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp", "*.jpeg" };
        #endregion

        public ImageServer(ILogger logger, IImageModel imageModel)
        {
            this.logger = logger;
            this.imageController = new ImageController(imageModel);
            this.communication = new ServerCommunication(logger);
            logger.MessageRecieved += communication.SendClientsLog;
            communication.Connect();
        }

        public void AddNewDirectoryHandler(string path)
        {
            IDirectoryHandler dirHandler = new DirectoyHandler(path, imageController, logger, extensions);
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

        public void EndServer()
        {
            CloseAll();
            communication.Disconnect();
        }

        ~ImageServer()
        {
            EndServer();
        }

    }
}