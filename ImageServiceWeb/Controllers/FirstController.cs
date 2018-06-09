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

        /// <summary>
        /// Main page.
        /// </summary>
        public ActionResult Index()
        {
            return View(detailsModel);
        }

        /// <summary>
        /// Returns the number of images for the image gallery.
        /// </summary>
        public void numberOfImages()
        {
            detailsModel.NumberOfImages = photosModel.Photos.Count;
        }

        /// <summary>
        /// Config page.
        /// </summary>
        /// <returns>view of the config page.</returns>
        public ActionResult Config()
        {
            configModel.Initialize();
            return View(configModel);
        }

        /// <summary>
        /// Handler removal confirmation screen.
        /// </summary>
        /// <param name="handler">Handler to remove.</param>
        /// <returns>view of the hander removal.</returns>
        public ActionResult SureToRemoveDir(string handler)
        {
            configModel.SelectedHandler = handler;
            return View(configModel);
        }

        /// <summary>
        /// Gallery page.
        /// </summary>
        /// <returns>view of the photo gallery page.</returns>
        public ActionResult Gallery()
        {
            configModel.Initialize();
            photosModel.GetPhotos(configModel.OutputDir, configModel.ThumbSize);
            return View(photosModel);
        }

        /// <summary>
        /// Accept the dir removal button
        /// </summary>
        /// <param name="name">name of handler to remove.</param>
        /// <returns>redirect to config screen.</returns>
        public ActionResult AcceptDirRemoval(string name)
        {
            configModel.Remove(name);
            System.Threading.Thread.Sleep(2000);
            return RedirectToAction("Config");

        }

        /// <summary>
        /// Big photo screen.
        /// </summary>
        /// <param name="id">ID of photo to display.</param>
        /// <returns>view of the photo</returns>
        public ActionResult PhotoScreen(int id)
        {
            foreach (Photo photo in photosModel.Photos)
            {
                if (photo.ID.Equals(id))
                    return View(photo);
            }
            return null;
        }

        /// <summary>
        /// Accept the photo removal button
        /// </summary>
        /// <param name="id">ID of photo to remove.</param>
        /// <returns>redirects to gallery page.</returns>
        public ActionResult AcceptPhotoRemoval(int id)
        {
            foreach (Photo photo in photosModel.Photos)
            {
                if (photo.ID.Equals(id))
                {
                    System.IO.File.Delete(photo.FullPath);
                    System.IO.File.Delete(photo.FullThumbPath);
                    return RedirectToAction("Gallery");
                }
            }
            return null;
        }

        /// <summary>
        /// Photo removal confimation screen.
        /// </summary>
        /// <param name="id">ID of image to remove.</param>
        /// <returns>view of the image confimration screen.</returns>
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

        /// <summary>
        /// Logs screen.
        /// </summary>
        /// <returns>view of the logs.</returns>
        public ActionResult Logs()
        {
            logModel.Initialize();
            return View(logModel);
        }

        /// <summary>
        /// Get list of logs for filter.
        /// </summary>
        /// <param name="type">type to filter by.</param>
        /// <returns>list of logs for filter.</returns>
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