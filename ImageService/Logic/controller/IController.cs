using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using ImageService;

namespace Logic
{
    public interface IController<T>
    {
        string ExecuteCommand(int commandID, T[] args, out bool result);
    }
}
