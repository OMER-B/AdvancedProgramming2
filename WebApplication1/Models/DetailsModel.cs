using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebImageService.Models
{
    public class DetailsModel
    {
        private class Student
        {
            string name;
            string id;

            public Student(string name, string id)
            {
                this.name = name;
                this.id = id;
            }
        }

        private List<Student> studentsInfo;
        private string infoFile = "Details.txt";

        public DetailsModel()
        {
            this.studentsInfo = new List<Student>();
            try
            {
                List<string> info = new List<string>();
                using (StreamReader sr = new StreamReader(infoFile))
                {
                    string name, id;
                    while ((name = sr.ReadLine()) != null)
                    {
                        id = sr.ReadLine();
                        studentsInfo.Add(new Student(name, id));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}