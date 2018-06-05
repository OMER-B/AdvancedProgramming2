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
            numberOfImages();
            
        }

        // GET: First
        public ActionResult Index() { return View(detailsModel); }

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

        public ActionResult SureToRemove(string handler)
        {
            configModel.SelectedHandler = handler;
            return View(configModel);
        }

        // GET: First
        public ActionResult Photos() {
            photosModel.ThumbPath = configModel.OutputDir;
            sendPhotos();
            return View(photosModel); }

        public ActionResult AcceptDirRemoval(string name)
        {
            configModel.Remove(name);
            return RedirectToAction("SureToRemove");

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

        public ActionResult DeletePhoto(int id)
        {
            foreach (Photo photo in photosModel.Photos)
            {
                if (photo.ID.Equals(id))
                    return View(photo);
            }
            return null;
        }

        /// <summary>
        /// Sends the photos to the photos model.
        /// </summary>
        public void sendPhotos()
        {
            if (configModel.OutputDir == null)
            {
                System.Threading.Thread.Sleep(1000);
            }
            int id = 0;
            List<Photo> list = new List<Photo>();
            string path = configModel.OutputDir + "\\thumbnails\\";
            if (Directory.Exists(path))
            {
                string[] photos = Directory.GetFiles(path, "*.thumb", SearchOption.AllDirectories);
                foreach (string photo in photos)
                {
                    list.Add(new Photo(Path.GetFileNameWithoutExtension(photo),
                        Path.GetFullPath(photo),
                        new DirectoryInfo(Path.GetDirectoryName(photo)).Name,
                        new DirectoryInfo(Path.GetDirectoryName(photo)).Parent.Name, id));
                    id++;
                }
            }
            photosModel.Photos = list;
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