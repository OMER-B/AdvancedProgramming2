using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using CommunicationTools;

namespace ImageServiceWeb.Models
{
    public class DetailsModel
    {
        private List<Student> students;
        private string path;
        public DetailsModel(string path)
        {
            this.path = path;
            this.Students = new List<Student>();
            parse();
        }

        public List<Student> Students { get => students; set => students = value; }

        private int numberOfImages;
        public int NumberOfImages { get => numberOfImages; set => numberOfImages = value; }

        /// <summary>
        /// Returns status of the service.
        /// </summary>
        /// <returns>online if service is online, offline otherwise.</returns>
        private string Status() { return TcpClientChannel.Instance.Connected ? "online" : "offline"; }
        public String ServerStatus { get => Status(); }

        public void parse()
        {
            using (StreamReader sr = File.OpenText("C:\\Users\\H\\source\\repos\\AdvancedProgramming23\\ImageServiceWeb\\Models\\details.txt"))
            {
                string s = String.Empty;
                string name = String.Empty;
                string id = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    name = s;
                    s = sr.ReadLine();
                    id = s;
                    this.Students.Add(new Student(name, id));
                }
            }
        }
    }
}