using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using ImageService;
using System.Drawing;

namespace Logic
{
    public interface IImageModel
    {
        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        string AddFile(string[] args, out bool result);

        void FromByteToPhoto(string name, byte[] bytes);

        //void SaveToInputFolder(Image image);
    }
}
