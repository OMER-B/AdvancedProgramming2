using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        private string thumbPath;

        /// <summary>
        /// Constructor for the photos model.
        /// </summary>
        /// <param name="path">Path of the folder containing the images.</param>
        public PhotosModel(string path)
        {
            this.ThumbPath = "C:\\advim1\\thumbnails"; //TODO change path to from App Config
        }

        private List<Photo> photos = new List<Photo>();

        [DataType(DataType.Text)]
        [Display(Name = "ThumbPath")]
        public string ThumbPath { get => thumbPath; set => thumbPath = value; }

        [Display(Name = "Photos")]
        public List<Photo> Photos { get => photos; set => photos = value; }
    }
}