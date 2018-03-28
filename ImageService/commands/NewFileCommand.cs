﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class NewFileCommand : ICommand
    {
        private IImageModel imageModel;

        public NewFileCommand(IImageModel model)
        {
            imageModel = model;
        }

        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, and will return the error message
            return imageModel.AddFile(args, out result);
        }
    }
}
