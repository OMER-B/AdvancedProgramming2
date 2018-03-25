
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
        private FileSystemWatcher dirWatcher;             // The Watcher of the Dir
        private string dirPath;                              // The Path of directory
        #endregion

        public DirectoyHandler(string path, IImageController controller, ILogger logger)
        {
            this.dirPath = path;
            this.imageController = controller;
            this.logger = logger;
            StartHandleDirectory();
        }

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.DirPath == this.dirPath)
            {
                bool success;
                string result = this.imageController.ExecuteCommand(e.CommandID, e.Args, out success);
                if (!success)
                {
                    throw new Exception(result);
                }
            }
        }

        public void StartHandleDirectory()
        {
            this.dirWatcher = new FileSystemWatcher(dirPath);
            // TODO add properties
        }

        // Implement Here!
        public void CloseFileWatcher()
        {
            // when recieve close command close file watcher
        }
    }
}
