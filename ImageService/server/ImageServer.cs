
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
        private ILogger logger;
        #endregion

        public ImageServer(ILogger logger, IImageModel imageModel)
        {
            this.logger = logger;
            this.controller = new ImageController(imageModel);


            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ TODO delete this
            global.Instance.logger.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, "in server constructor"));
            ////////////////

        }

        public void AddNewDirectoryHandler(string path)
        {
            string[] extensions = { "*.jpg", "*.png", "*.gif", "*.bmp" };
            IDirectoryHandler dirHandler = new DirectoyHandler(path, controller, logger, extensions);
            SendCommand += dirHandler.OnCommandRecieved;
            dirHandler.DirectoryClose += CloseHandler;
            dirHandler.StartHandleDirectory();

            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ TODO delete this
            global.Instance.logger.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, "created handlers"));
            ////////////////


        }

        public void CloseHandler(object sender, DirectoryCloseEventArgs args)
        {
            //args.DirectoryPath
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