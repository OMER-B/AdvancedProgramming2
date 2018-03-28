using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace ImageService.service
{
    class ConfigReader
    {
        private string Handler;
        private string OutputDir;
        private string SourceName;
        private string LogName;
        private int ThumbnailSize;

        public ConfigReader()
        {
            this.Handler = ConfigurationManager.AppSettings["Handler"];
            this.OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }
    }
}
