﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Tools;
using Logic;

namespace ImageService
{
    class ConfigReader
    {
        public string[] Handler { get; }
        public string OutputDir { get; }
        public string SourceName { get; }
        public string LogName { get; }
        public int ThumbnailSize { get; }

        public ConfigReader()
        {
            this.Handler = ConfigurationManager.AppSettings["Handler"].Split(';');
            this.OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }
    }
}