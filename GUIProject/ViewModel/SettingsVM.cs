using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.ViewModel
{
    class SettingsVM : IViewModel
    {
        private IList<KeyValuePair<String, String>> test;

        public SettingsVM()
        {
            this.test = new List<KeyValuePair<String, String>>();
            this.test.Add(new KeyValuePair<string, string>("Firstly, consider just", "using a"));
            this.test.Add(new KeyValuePair<string, string>("S, just", "a ListBox"));
            this.test.Add(new KeyValuePair<string, string>("T, no just", "ItemsControl"));
            this.test.Add(new KeyValuePair<string, string>("F, consider just", "or"));
            this.test.Add(new KeyValuePair<string, string>("F, that just", "using a ListBox"));
            this.test.Add(new KeyValuePair<string, string>("S, a lot", "using ItemsControl"));
            this.test.Add(new KeyValuePair<string, string>("S, XamlObjectWriter", "ListBox or ItemsControl"));
        }
    }
}
