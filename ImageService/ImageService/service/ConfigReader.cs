using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Tools;
using Logic;

namespace ImageService
{
    public class ConfigReader
    {
        public List<string> Handler { get; }
        public string OutputDir { get; }
        public string SourceName { get; }
        public string LogName { get; }
        public int ThumbnailSize { get; }

        public ConfigReader()
        {
            this.Handler = new List<string>(ConfigurationManager.AppSettings["Handler"].Split(';'));
            this.OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }

        public void Remove(string directoryPath)
        {
            if (Handler.Contains(directoryPath))
            {
                Handler.Remove(directoryPath);
            }

        }
    }
}
