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

            StringBuilder builder = new StringBuilder();
            foreach (string path in Handler)
            {
                builder.Append(path).Append(";");
            }
            string newHandler = builder.ToString().TrimEnd(';');
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("Handler");
            config.AppSettings.Settings.Add("Handler", newHandler);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
