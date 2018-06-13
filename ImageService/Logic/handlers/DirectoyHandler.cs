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
        private IController<string> imageController;
        private ILogger logger;
        private List<FileSystemWatcher> dirWatchers;
        private string dirPath;
        private readonly string[] extensions;

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        // The Path of directory
        #endregion

        public DirectoyHandler(string path, IController<string> controller, ILogger logger, string[] extensions)
        {
            this.dirPath = path;
            this.imageController = controller;
            this.logger = logger;
            this.extensions = extensions;
            this.dirWatchers = new List<FileSystemWatcher>();
        }

        /// <summary>
        /// Create file system watchers for the directory path.
        /// </summary>
        public void StartHandleDirectory()
        {
            for (int i = 0; i < extensions.Length; i++)
            {
                FileSystemWatcher dirWatcher = new FileSystemWatcher(dirPath, extensions[i]);
                dirWatcher.EnableRaisingEvents = true;
                dirWatcher.Created += new FileSystemEventHandler(FileCreated);
                dirWatchers.Add(dirWatcher);
            }
        }

        /// <summary>
        /// Disable the file system watchers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">the path of the directory that need to close the watcher</param>
        public void CloseFileWatcher(object sender, DirectoryCloseEventArgs args)
        {
            if (args.DirectoryPath.Equals(this.dirPath) || args.DirectoryPath.Equals("*"))
            {
                foreach (FileSystemWatcher watcher in dirWatchers)
                {
                    watcher.EnableRaisingEvents = false;
                }
                logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.WARNING, "Closed directory " + this.dirPath));
                DirectoryClose.Invoke(this, new DirectoryCloseEventArgs(this.dirPath, "Recieved close message"));
            }
        }

        /// <summary>
        /// Activated by the file system watcher on create event - when a new file is added.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void FileCreated(object sender, FileSystemEventArgs args)
        {
            string[] arguments = new string[] { args.FullPath, args.Name };
            int commandID = (int)ImageCommandTypeEnum.ADD_FILE;
            CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs(commandID, arguments, this.dirPath);
            OnCommandRecieved(sender, eventArgs);
        }

        /// <summary>
        /// Execute a ned command from the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs args)
        {
            if (args.DirPath.Equals(this.dirPath) || args.DirPath.Equals("*"))
            {
                bool success;
                string result = this.imageController.ExecuteCommand(args.CommandID, args.Args, out success);
                if (success)
                {
                    logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.INFO, result));
                }
                else
                {
                    logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.FAIL, result));
                }
            }
        }

    }
}

