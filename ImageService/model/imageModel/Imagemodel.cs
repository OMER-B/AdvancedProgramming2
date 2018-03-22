using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService
{
    public class ImageModel : IImageModel
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        string IImageModel.AddFile(string path, out bool result)
        {
            //TODO implement get the file and put it in a new directory
            throw new NotImplementedException();
        }

        //TODO: function create directory, move file to directory, get date

        #endregion

    }
}
