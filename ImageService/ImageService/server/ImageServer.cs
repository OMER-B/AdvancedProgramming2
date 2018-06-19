
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Logic;
using CommunicationTools;
using server;

namespace ImageService
{
    public class ImageServer
    {
        #region Properties
        public event EventHandler<DirectoryCloseEventArgs> StopHandler;
        #endregion

        #region Members
        private ITCP communication;
        private IController<string> imageController;
        private IController<byte> appController;
        private IClientHandler clientHandler;
        IImageModel imageModel;
        private ILogger logger;
        private string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp", "*.jpeg" };
        #endregion

        public ImageServer(ILogger logger, IImageModel imageModel, ConfigReader reader)
        {
            this.logger = logger;
            this.imageController = new ImageController(imageModel);
            this.appController = new ApplicationController(imageModel);
            this.imageModel = imageModel;
            this.clientHandler = new AndroidHandler(imageModel);
            communication = new BinaryTcp(logger);

            communication.Connect();
            communication.MessageFromClient += this.clientHandler.HandleRequest;

        }


        /// <summary>
        /// Add a new Direcotry handler and register the apropriate events.
        /// </summary>
        /// <param name="path">The path of the new directory to monitor</param>
        public void AddNewDirectoryHandler(string path)
        {
            IDirectoryHandler dirHandler = new DirectoyHandler(path, imageController, logger, extensions);
            StopHandler += dirHandler.CloseFileWatcher;
            dirHandler.StartHandleDirectory();
        }

        /// <summary>
        /// Stop a directory handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void RemoveHandler(object sender, DirectoryCloseEventArgs args)
        {
            StopHandler.Invoke(this, args);
        }


        /// <summary>
        /// Close all the  handlers
        /// </summary>
        public void CloseAll()
        {
            StopHandler.Invoke(this, new DirectoryCloseEventArgs("*", null));
        }

        /// <summary>
        /// Close the handlers and disconnect communication.
        /// </summary>
        public void EndServer()
        {
            CloseAll();
            communication.DisconnectAll();
        }

        ~ImageServer()
        {
            EndServer();
        }

    }
}