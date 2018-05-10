using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Tools;
using ImageService;

namespace Logic
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IController imageController;
        private ILogger logger;
        private List<FileSystemWatcher> dirWatchers;
        private string dirPath;
        private readonly string[] extensions;

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        // The Path of directory
        #endregion

        public DirectoyHandler(string path, IController controller, ILogger logger, string[] extensions)
        {
            this.dirPath = path;
            this.imageController = controller;
            this.logger = logger;
            this.extensions = extensions;
            this.dirWatchers = new List<FileSystemWatcher>();
        }

        private void Temp(string message)
        {
            logger.Log(this, new MessageRecievedEventArgs(LogMessageTypeEnum.INFO, message));
        }

        public void StartHandleDirectory()
        {
            for (int i = 0; i < extensions.Length; i++)
            {
                FileSystemWatcher dirWatcher = new FileSystemWatcher(dirPath, extensions[i]);
                dirWatcher.EnableRaisingEvents = true;
                dirWatcher.Created += new FileSystemEventHandler(FileCreated);
                dirWatchers.Add(dirWatcher);
                Temp("Dir: " + dirPath + ", extention: " + extensions[i]);
            }
        }

        public void CloseFileWatcher(object sender, DirectoryCloseEventArgs args)
        {
            if (args.DirectoryPath.Equals(this.dirPath) || args.DirectoryPath.Equals("*"))
            {
                foreach (FileSystemWatcher watcher in dirWatchers)
                {
                    watcher.EnableRaisingEvents = false;
                }
                logger.Log(this, new MessageRecievedEventArgs(LogMessageTypeEnum.INFO, "Closed directory " + this.dirPath));
                DirectoryClose.Invoke(this, new DirectoryCloseEventArgs(this.dirPath, "Recieved close message"));
            }
        }

        private void FileCreated(object sender, FileSystemEventArgs args)
        {
            Temp("in file created");
            string[] arguments = new string[] { args.FullPath, args.Name };
            int commandID = (int)ImageCommandTypeEnum.ADD_FILE;
            CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs(commandID, arguments, this.dirPath);
            OnCommandRecieved(sender, eventArgs);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs args)
        {
            if (args.DirPath.Equals(this.dirPath) || args.DirPath.Equals("*"))
            {
                bool success;
                string result = this.imageController.ExecuteCommand(args.CommandID, args.Args, out success);
                if (success)
                {
                    logger.Log(this, new MessageRecievedEventArgs(LogMessageTypeEnum.INFO, result));
                }
                else
                {
                    logger.Log(this, new MessageRecievedEventArgs(LogMessageTypeEnum.FAIL, result));
                }
            }
        }

    }
}

