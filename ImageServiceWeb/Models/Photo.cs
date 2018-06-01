using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Photo
    {
        static int count = 0;
        public void Reset()
        {
            count = 0;
        }

        public Photo(string name, string path, string month, string year)
        {
            this.id = count;
            this.name = name;
            this.path = path;
            this.month = month;
            this.year = year;
            count++;
        }

        private string path;
        private string year;
        private string month;
        private string name;
        private int id;

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
        [Display(Name = "Name")]
        public string Name { get => name; set => name = value; }
    }
}