using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using Tools;
using ImageService;
using ImageService.Tools;

namespace Logic
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
            string newFilePath = " ";
            try
            {
                if (args.Length != 2)
                {
                    result = false;
                    return "Error with file name or path: " + args[0] + ", " + args[1] + ".";
                }
                string filePath = args[0];
                string fileName = args[1];
                if (!File.Exists(filePath))
                {
                    result = false;
                    return "Path " + args[0] + " does not exist.";
                }

                string year = ImageProperties.GetDateTakenFromImage(filePath).Year.ToString();
                string month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(ImageProperties.GetDateTakenFromImage(filePath).Month).ToString();
                // create new photo directory
                string dateOutputFolder = outputFolder + "\\" + year + "\\" + month;
                Directory.CreateDirectory(dateOutputFolder);
                //create thumb directory
                string outputThumbFolder = outputFolder + "\\thumbnails\\" + year + "\\" + month;
                Directory.CreateDirectory(outputThumbFolder);

                string underscore = "_";
                newFilePath = dateOutputFolder + "\\" + fileName;

                // if a file with same name exists, add "_" to the name
                while (File.Exists(newFilePath))
                {
                    fileName = underscore + fileName;
                    newFilePath = dateOutputFolder + "\\" + fileName;
                }

                string thumbPath = outputThumbFolder + "\\" + fileName;

                File.Move(filePath, newFilePath);
                
                Image image = Image.FromFile(newFilePath);
                Image thumb = image.GetThumbnailImage(thumbnailSize, thumbnailSize, () => false, IntPtr.Zero);

                thumb.Save(Path.ChangeExtension(thumbPath, "thumb"));

                result = true;
                string statusResult = "Image " + fileName + " was added to " + year + ", " + month + ".";
                image.Dispose();
                thumb.Dispose();
                return statusResult;
            }
            catch (Exception ex)
            {
                result = false;
                return "Exception: " + newFilePath + ": " + ex.ToString();
            }
        }

    }
}
