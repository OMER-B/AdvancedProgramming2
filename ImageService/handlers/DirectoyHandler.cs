
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ImageService
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController imageController;
        private ILogger logger;
        private List<FileSystemWatcher> dirWatchers;
        private string dirPath;
        private readonly string[] extensions;

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        // The Path of directory
        #endregion

        public DirectoyHandler(string path, IImageController controller, ILogger logger, string[] extensions)
        {
            this.dirPath = path;
            this.imageController = controller;
            this.logger = logger;
            this.extensions = extensions;
            this.dirWatchers = new List<FileSystemWatcher>();
           
        }

        public void StartHandleDirectory()
        {
            for (int i = 0; i < extensions.Length; i++)
            {
                FileSystemWatcher dirWatcher = new FileSystemWatcher(dirPath, extensions[i]);
                dirWatcher.EnableRaisingEvents = true;
                dirWatcher.Created += new FileSystemEventHandler(FileCreated);
                dirWatchers.Add(dirWatcher);
            }

        
            // go through all the files and send to "add file"
        }

        public void CloseFileWatcher(object sender, DirectoryCloseEventArgs args)
        {
            if (args.DirectoryPath.Equals(this.dirPath) || args.DirectoryPath.Equals("*"))
            {
                foreach (FileSystemWatcher watcher in dirWatchers)
                {
                    watcher.EnableRaisingEvents = false;
                }
                logger.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, "Closed directory " + this.dirPath));

            }
        }

        private void FileCreated(object sender, FileSystemEventArgs args)
        {
            // call on command recieved
            string[] arguments = new string[] { args.FullPath, args.Name };
            int commandID = (int)CommandTypeEnum.ADD_FILE;
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
                    logger.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, result));
                }
                else
                {
                    logger.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.FAIL, result));
                }
            }
        }

    }
}

