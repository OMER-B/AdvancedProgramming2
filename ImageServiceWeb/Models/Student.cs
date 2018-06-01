using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    public class Student
    {
        private string name;
        private string id;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public string ID { get => id; set => id = value; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get => name; set => name = value; }

        public Student(string name, string id)
        {
            this.name = name;
            this.id = id;
        }
    }
}