using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIProject.Model
{
    class MainWindowModel
    {
        private string color;
        public string Color { get => color; set => color = value; }

        public bool Connected { get => connected; set => connected = value; }
        private bool connected;
    }
}
