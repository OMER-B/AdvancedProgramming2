using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



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

        private string ParseColor()
        {
            switch (this.title.ToLower())
            {
                case "info":
                    return "LawnGreen";
                case "fail":
                    return "OrangeRed";
                case "warning":
                    return "Yellow";
                default:
                    return "White";
            }
        }

        private string title;
        private string content;
        private string color;

        [JsonProperty("Title")]
        public string Title { get { return this.title; } set { this.title = value; } }

        [JsonProperty("Content")]
        public string Content { get { return this.content; } set { this.content = value; } }

        [JsonProperty("Color")]
        public string Color { get { return this.color; } set { this.color = value; } }
    }
}
