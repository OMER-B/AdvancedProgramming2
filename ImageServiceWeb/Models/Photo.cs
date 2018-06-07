using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Photo
    {
        public Photo(string name, string path, string thumbPath, string year, string month, int size, int id)
        {
            this.name = name;
            this.path = path;
            this.thumbPath = thumbPath;
            this.year = year;
            this.month = month;
            this.size = size;
            this.id = id;
        }

        private string name;
        private string path;
        private string thumbPath;
        private string year;
        private string month;
        private int size;
        private int id;


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get => name; set => name = value; }

        [Required]
        [Display(Name = "Name")]
        public int Size { get => size; set => size = value; }

        [Required]
        [Display(Name = "ID")]
        public int ID { get => this.id; set => this.id = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get => month; set => month = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get => year; set => year = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Path")]
        public string Path { get => path; set => path = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbPath")]
        public string ThumbPath { get => thumbPath; set => thumbPath = value; }

    }
}