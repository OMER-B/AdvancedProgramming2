﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Logic;
using ImageService.Logic.controller;
using CommunicationTools;

namespace ImageService
{
    public class ImageServer
    {
        #region Properties
        public event EventHandler<CommandRecievedEventArgs> SendCommand;
        public event EventHandler<DirectoryCloseEventArgs> StopHandler;
        #endregion

        #region Members
        private ITcpServer communication;
        private IController imageController;
        private IController serverController;
        private ILogger logger;
        private string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp", "*.jpeg" };
        #endregion

        public ImageServer(ILogger logger, IImageModel imageModel, ConfigReader reader)
        {
            this.logger = logger;
            this.imageController = new ImageController(imageModel);

            this.communication = new TcpServer(logger);
            logger.MessageRecieved += communication.OnClientsLog;
            communication.Connect();
            communication.MessageFromClient += ExecCommandFromClient;
            StopHandler += communication.OnClientRemoveHandler;

            // Creating the commands
            CloseHandlerCommand closeHandler = new CloseHandlerCommand();
            closeHandler.CloseDirectory += RemoveHandler;
            GetLogHistoryCommand getLogCommmand = new GetLogHistoryCommand(logger);
            getLogCommmand.SendHistory += communication.MessageClients;
            GetConfigCommand configCommand = new GetConfigCommand(reader);
            configCommand.SendConfig += communication.MessageClients;

            // create the controller with the commands
            this.serverController = new ServerController(closeHandler, getLogCommmand, configCommand);
        }

        /// <summary>
        /// pass a command from the client to the controller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message">The message to pass</param>
        private void ExecCommandFromClient(object sender, ClientMessage message)
        {
            Tuple<int, string[]> args = MessageParser.parseMessageToCommand(message);
            serverController.ExecuteCommand(args.Item1, args.Item2, out bool result);
        }

        /// <summary>
        /// Add a new Direcotry handler and register the apropriate events.
        /// </summary>
        /// <param name="path">The path of the new directory to monitor</param>
        public void AddNewDirectoryHandler(string path)
        {
            IDirectoryHandler dirHandler = new DirectoyHandler(path, imageController, logger, extensions);
            SendCommand += dirHandler.OnCommandRecieved;
            StopHandler += dirHandler.CloseFileWatcher;
            dirHandler.DirectoryClose += HandlerClosed;
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
        /// The message that the handler itself sends the server
        /// to let it know that the directory is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandlerClosed(object sender, DirectoryCloseEventArgs args)
        {
            //args.DirectoryPath
            if (sender is IDirectoryHandler h)
            {
                SendCommand -= h.OnCommandRecieved;
            }
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