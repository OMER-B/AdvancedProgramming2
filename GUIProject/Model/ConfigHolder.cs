using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Model
{
    class ConfigHolder
    {
        public ConfigHolder(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        private string key;
        private string value;
        public string Key { get { return this.key; } set { this.Key = value; } }
        public string Value { get { return this.value; } set { this.Value = value; } }
    }
}
