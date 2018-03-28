using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ImageService
{
    public class ImageModel : IImageModel
    {
        #region Members
        private string outputFolder;            // The Output Folder
        private int thumbnailSize;              // The Size Of The Thumbnail Size

        public ImageModel(string output, int size)
        {
            this.outputFolder = output;
            this.thumbnailSize = size;
        }

        public string AddFile(string[] args, out bool result)
        {
            if (args.Length != 2)
            {
                result = false;
                return "Error with file name or path";
            }
            string filePath = args[0];
            string fileName = args[1];
            if (!File.Exists(filePath))
            {
                result = false;
                return "Not existing file in Image Model";
            }

            string year = File.GetCreationTime(filePath).Year.ToString();
            string month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(File.GetCreationTime(filePath).Month).ToString();

            string dateOutputFolder = outputFolder + "\\" + year + "\\" + month;
            Directory.CreateDirectory(dateOutputFolder);

            File.Copy(filePath, dateOutputFolder + "\\" + fileName);

            string outputThumbFolder = dateOutputFolder + "\\" + "thumbnails";
            Directory.CreateDirectory(outputThumbFolder);
            Image image = Image.FromFile(filePath);
            Image thumb = image.GetThumbnailImage(thumbnailSize, thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(Path.ChangeExtension(outputThumbFolder + "\\" + fileName, "thumb"));

            result = true;
            string statusResult = "Image " + fileName + " was added to " + year + ", " + month + ".";
            return statusResult;
        }


        #endregion
    }
}
