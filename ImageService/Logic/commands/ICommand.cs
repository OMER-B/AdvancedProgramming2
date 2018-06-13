using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using ImageService;

namespace Logic
{
    public interface ICommand<T>
    {
        string Execute(T[] args, out bool result);          // The Function That will Execute The 
    }
}
