using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class BytesToImageCommand : ICommand<byte>
    {

        private IImageModel model;

        public BytesToImageCommand(IImageModel model)
        {
            this.model = model;
        }

        public string Execute(byte[] args, out bool result)
        {
            try
            {
                Image img = this.model.FromByteToPhoto(args);
                this.model.SaveToInputFolder(img);
                result = true;
                return "Recived photo.";
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
