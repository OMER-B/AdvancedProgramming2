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
        #endregion

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
            // create new photo directory
            string dateOutputFolder = outputFolder + "\\" + year + "\\" + month;
            Directory.CreateDirectory(dateOutputFolder);
            //create thumb directory
            string outputThumbFolder = outputFolder + "\\thumbnails\\" + year + "\\" + month;
            Directory.CreateDirectory(outputThumbFolder);


            string underscore = "_";
            string newFileName = dateOutputFolder + "\\" + fileName;
            string newThumbName = outputThumbFolder + "\\" + fileName;

            // if a file with same name exists, add "_" to the name
            while (File.Exists(newFileName))
            {
                newFileName = underscore + newFileName;
                newThumbName = underscore + newThumbName;
            }

            File.Copy(filePath, newFileName);

            Image image = Image.FromFile(filePath);
            Image thumb = image.GetThumbnailImage(thumbnailSize, thumbnailSize, () => false, IntPtr.Zero);

            thumb.Save(Path.ChangeExtension(newThumbName, "thumb"));

            result = true;
            string statusResult = "Image " + fileName + " was added to " + year + ", " + month + ".";
            return statusResult;
        }


    }
}
