using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunicationTools;

namespace ImageServiceWeb.Models
{
    public class LogModel
    {
        private List<TitleAndContent> list;

        public LogModel()
        {
            list = new List<TitleAndContent>();
            list.Add(new TitleAndContent("FAIL", "hello"));
        }

    }
}