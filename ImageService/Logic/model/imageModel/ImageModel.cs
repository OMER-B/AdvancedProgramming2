using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using Tools;
using ImageService;
using ImageService.Tools;
using System.Threading;

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
            try
            {
                if (args.Length != 2)
                {
                    result = false;
                    return "Error with file name or path: " + args[0] + ", " + args[1] + ".";
                }
                string filePath = args[0];
                string fileName = args[1];
                Thread.Sleep(500);
                if (File.Exists(filePath))
                {
                    if (!Directory.Exists(outputFolder))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(outputFolder);
                        di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }
                    Thread.Sleep(500);
                    return MoveFile(filePath, fileName, outputFolder, out result);
                }
                else
                {
                    result = false;
                    return "Path " + filePath + " does not exist.";
                }
            }
            catch (Exception ex)
            {
                result = false;
                return "Exception: " + ex.ToString();
            }
        }

        /// <summary>
        /// Moves the photo folder.
        /// </summary>
        /// <param name="filePath">Path of the folder</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="outputFolder">Where to move the file</param>
        /// <param name="result">if succeeded</param>
        /// <returns></returns>
        public string MoveFile(string filePath, string fileName, string outputFolder, out bool result)
        {
            string newFilePath = " ";
            string year = ImageProperties.GetDateTakenFromImage(filePath).Year.ToString();
            //string month = ImageProperties.GetDateTakenFromImage(filePath).Month.ToString();
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

    }
}
