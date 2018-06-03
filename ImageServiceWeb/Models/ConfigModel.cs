using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        private string outputDir;

        public string OutputDir { get => "/advim1"; set => outputDir = value; }
    }

}