﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using ImageService;

namespace Logic
{
    public interface IImageController
    {
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}