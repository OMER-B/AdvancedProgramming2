
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
        private IImageController imageController;              // The Image Processing Controller
        private ILogger logger;
        private List<FileSystemWatcher> dirWatchers;             // The Watcher of the Dir
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
            StartHandleDirectory();
        }


        public void StartHandleDirectory()
        {
            for(int i = 0; i < extensions.Length; i++)
            {
                FileSystemWatcher dirWatcher = new FileSystemWatcher(dirPath, extensions[i]);
                dirWatcher.EnableRaisingEvents = true;
                dirWatcher.Created += new FileSystemEventHandler(FileCreated);
                dirWatchers.Add(dirWatcher);
            }

            // go through all the files and send to "add file"
        }

        public void CloseFileWatcher()
        {
            // when recieve close command close file watcher
        }

        private void FileCreated(object sender, FileSystemEventArgs args)
        {
            // call on command recieved
            string[] arguments = new string[] { args.FullPath };
            int commandID = (int)CommandTypeEnum.ADD_FILE;
            CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs(commandID, arguments, this.dirPath);

        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.DirPath.Equals(this.dirPath))
            {
                bool success;
                string result = this.imageController.ExecuteCommand(e.CommandID, e.Args, out success);
                if (success)
                {
                    // TODO send to logger
                } else
                {

                }
            }
        }

        // TODO add close function
    }
}
