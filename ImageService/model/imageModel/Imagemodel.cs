using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageService
{
    public class ImageModel : IImageModel
    {
        #region Members
        private string outputFolder;            // The Output Folder
        private int thumbnailSize;              // The Size Of The Thumbnail Size

        string IImageModel.AddFile(string path, out bool result)
        {
            if (!File.Exists(path))
            {
                result = false;
                return "Not existing file in Image Model";
            }

            DateTime date = File.GetCreationTime(path);

            result = true;
            return "true";
        }

        //TODO: function create directory, move file to directory, get date

        

        #endregion
    }
}
