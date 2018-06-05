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

        private List<Photo> photos = new List<Photo>();

        [DataType(DataType.Text)]
        [Display(Name = "ThumbPath")]
        public string ThumbPath { get => thumbPath; set => thumbPath = value; }

        [Display(Name = "Photos")]
        public List<Photo> Photos { get => photos; set => photos = value; }
    }
}