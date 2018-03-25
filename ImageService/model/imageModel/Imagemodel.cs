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
            //TODO implement get the file and put it in a new directory
            throw new NotImplementedException();
        }

        //TODO: function create directory, move file to directory, get date

            /**
           
        private Regex r = new Regex(":");

        private DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }
    **/

        #endregion
    }
}
