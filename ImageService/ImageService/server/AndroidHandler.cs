using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationTools;
using Logic;

namespace server
{
    class AndroidHandler : IClientHandler
    {
        private IImageModel imageModel;

        public AndroidHandler(IImageModel model)
        {
            this.imageModel = model;
        }

        public void HandleRequest(object sender, ClientMessage message)
        {
            this.imageModel.FromByteToPhoto(message.Name, message.Message);
        }
    }
}
