using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace CommunicationTools
{
    class Log
    {
        public Log(string type, string msg)
        {
            this.type = type;
            this.message = msg;
            this.color = ParseColor();
        }

        private Brush ParseColor()
        {
            switch (this.type.ToLower())
            {
                case "info":
                    return Brushes.LawnGreen;
                case "fail":
                    return Brushes.OrangeRed;
                case "warning":
                    return Brushes.Yellow;
                default:
                    return Brushes.White;
            }
        }

        private string type;
        private string message;
        private Brush color;
        public string Type { get { return this.type; } set { this.type = value; } }
        public string Message { get { return this.message; } set { this.message = value; } }
        public Brush Color { get { return this.color; } set { this.color = value; } }

        
    }
}
