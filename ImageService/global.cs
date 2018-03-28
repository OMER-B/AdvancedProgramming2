using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class global
    {
        private static global instance;

        private global() { }
        public ILogger logger { get; set; }

        public static global Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new global();
                }
                return instance;
            }
        }
    }

}
