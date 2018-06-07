using CommunicationTools;
using ImageServiceWeb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class FirstController : Controller
    {
        static DetailsModel detailsModel = new DetailsModel("details.txt");
        static ConfigModel configModel = new ConfigModel();
        static PhotosModel photosModel = new PhotosModel();
        static LogModel logModel = new LogModel();

        public FirstController()
        {
            configModel.Initialize();
            photosModel.GetPhotos(configModel.OutputDir, configModel.ThumbSize);
            numberOfImages();
        }

        // GET: First
        public ActionResult Index()
        {
            return View(detailsModel);
        }

        public void numberOfImages()
        {
            detailsModel.NumberOfImages = photosModel.Photos.Count;
        }

        // GET: First
        public ActionResult Config()
        {
            configModel.Initialize();
            return View(configModel);
        }

        public ActionResult SureToRemoveDir(string handler)
        {
            configModel.SelectedHandler = handler;
            return View(configModel);
        }

        // GET: First
        public ActionResult Photos()
        {
            configModel.Initialize();
            photosModel.GetPhotos(configModel.OutputDir, configModel.ThumbSize);
            return View(photosModel);
        }

        public ActionResult AcceptDirRemoval(string name)
        {
            configModel.Remove(name);
            System.Threading.Thread.Sleep(2000);
            return RedirectToAction("Config");

        }

        public ActionResult PhotoScreen(int id)
        {
            foreach (Photo photo in photosModel.Photos)
            {
                if (photo.ID.Equals(id))
                    return View(photo);
            }
            return null;
        }

        public ActionResult AcceptPhotoRemoval(int id)
        {
            foreach (Photo photo in photosModel.Photos)
            {
                if (photo.ID.Equals(id))
                {
                    System.IO.File.Delete(photo.FullPath);
                    System.IO.File.Delete(photo.FullThumbPath);
                    return RedirectToAction("Photos");
                }
            }
            return null;
        }

        public ActionResult SureToRemovePhoto(int id)
        {
            foreach (Photo photo in photosModel.Photos)
            {
                if (photo.ID.Equals(id))
                {
                    return View(photo);
                }
            }
            return null;
        }

        // GET: First
        public ActionResult Logs()
        {
            logModel.Initialize();
            return View(logModel);
        }

        [HttpPost]
        public JObject GetLogs(string type)
        {
            foreach (var log in logModel.List)
            {
                if (log.Title == type)
                {
                    JObject data = new JObject();
                    data["Type"] = log.Title;
                    data["Log"] = log.Content;
                    return data;
                }
            }
            return null;
        }
    }
}