using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace CommunicationTools
{
    class TitleAndContent
    {
        public TitleAndContent(string title, string content)
        {
            this.title = title;
            this.content = content;
            this.color = ParseColor();
        }

        private Brush ParseColor()
        {
            switch (this.title.ToLower())
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

        private string title;
        private string content;
        private Brush color;

        [JsonProperty("Title")]
        public string Title { get { return this.title; } set { this.title = value; } }

        [JsonProperty("Content")]
        public string Content { get { return this.content; } set { this.content = value; } }

        [JsonProperty("Color")]
        public Brush Color { get { return this.color; } set { this.color = value; } }
    }
}
