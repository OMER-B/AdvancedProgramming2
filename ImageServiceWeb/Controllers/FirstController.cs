using ImageServiceWeb.Models;
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
        static PhotosModel photosModel = new PhotosModel("TODO: Change this in FirstController photosModel");
        static LogModel logModel = new LogModel();

        public FirstController()
        {
            sendPhotos();
            numberOfImages();
        }

        // GET: First
        public ActionResult Index() { return View(detailsModel); }

        public void numberOfImages()
        {
            detailsModel.NumberOfImages = photosModel.Photos.Count;
        }

        // GET: First
        public ActionResult Config() { return View(configModel); }

        // GET: First
        public ActionResult Photos() { return View(photosModel); }

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
            int id = 0;
            List<Photo> list = new List<Photo>();
            string path = configModel.OutputDir + "\\thumbnails\\";
            if (Directory.Exists(path))
            {
                string[] photos = Directory.GetFiles(path, "*.thumb", SearchOption.AllDirectories);
                foreach (string photo in photos)
                {
                    list.Add(new Photo(Path.GetFileNameWithoutExtension(photo),
                        photo,
                        new DirectoryInfo(Path.GetDirectoryName(photo)).Name,
                        new DirectoryInfo(Path.GetDirectoryName(photo)).Parent.Name, id));
                    id++;
                }
            }
            photosModel.Photos = list;
        }

        // GET: First
        public ActionResult Logs() { return View(logModel); }
    }
}