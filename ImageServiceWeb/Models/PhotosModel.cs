using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        private List<Photo> photosList = new List<Photo>();

        [Display(Name = "Photos")]
        public List<Photo> Photos { get => photosList; set => photosList = value; }

        public void GetPhotos(string path, int size)
        {
            photosList.Clear();
            string thumbPath = path + "\\thumbnails";
            int id = 0;
            if (!Directory.Exists(path) || !Directory.Exists(thumbPath)) return;
            DirectoryInfo directory = new DirectoryInfo(path);

            List<DirectoryInfo> yearDirs = directory.GetDirectories().ToList();
            foreach (DirectoryInfo year in yearDirs)
            {
                List<DirectoryInfo> monthDirs = year.GetDirectories().ToList();
                foreach (DirectoryInfo month in monthDirs)
                {
                    List<FileInfo> photos = month.GetFiles().ToList();
                    foreach (FileInfo photo in photos)
                    {
                        string yearString = year.Name;
                        string monthString = month.Name;
                        string relativePath = year.Parent.Name +
                            "\\" + yearString + "\\" + monthString + "\\" + photo.Name;
                        string thumbRelPath = year.Parent.Name +
                            "\\thumbnails\\" + yearString + "\\" + monthString + "\\" + photo.Name;
                        thumbRelPath = thumbRelPath.Replace(photo.Extension, ".thumb");
                        string fullPath = photo.FullName;
                        string fullThumbPath = fullPath.Replace(photo.Extension, ".thumb").Replace(yearString, "\\thumbnails\\" + yearString);

                        photosList.Add(new Photo(photo.Name, relativePath, thumbRelPath, fullPath, fullThumbPath, yearString, monthString, size, id++));
                    }
                }

            }
        }

    }
}