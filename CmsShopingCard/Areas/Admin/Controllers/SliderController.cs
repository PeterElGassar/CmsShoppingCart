using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CmsShopingCard.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        Db db;
        public SliderController()
        {
            db = new Db();
        }

        // GET: Admin/Slider
        public ActionResult Index()
        {
            return View(db.SliderGallerys.ToList());
        }

        // GET: Admin/AddImage
        public ActionResult AddImage()
        {
            return View();
        }
        // POST: Admin/AddImage
        [HttpPost]
        public ActionResult AddImage(SliderGallery model, HttpPostedFileBase sliderImage)
        {

            db.SliderGallerys.Add(model);
            db.SaveChanges();

            if (sliderImage != null && sliderImage.ContentLength > 0)
            {

                model.SaveImage(sliderImage);
                db.SaveChanges();

                TempData["SM"] = "You Added Image ToSlider Successfully..";
            }
            return RedirectToAction("AddImage");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var img = db.SliderGallerys.FirstOrDefault(s => s.SliderId == id);
            if (img == null)
            {
                return HttpNotFound();

            }

            return View(img);
        }

        [HttpPost]
        public ActionResult Edit(SliderGallery model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(model);

            var sliderInDb = db.SliderGallerys.Find(model.SliderId);
            sliderInDb.Title = model.Title;
            sliderInDb.UrlLink = model.UrlLink;


            TempData["SM"] = "Update Success..";

            if (file != null && file.ContentLength > 0)
            {
                model.UpdateImage(file, sliderInDb.ImageName);
                TempData["SM"] = "Update Success with Image..";

            }
            sliderInDb.ImageName = file.FileName;
            db.SaveChanges();

            return RedirectToAction("Edit");
        }


        public ActionResult DeleteSliderImage(int id)
        {
            var imgInDb = db.SliderGallerys.Find(id);
            string imgName = imgInDb.ImageName;
            db.SliderGallerys.Remove(imgInDb);
            db.SaveChanges();


            //remove image file 
            var mainDirec = new
                DirectoryInfo(string.Format(@"{0}\SliderImages\Thumbs", Server.MapPath(@"\")));

            string fullPath = Path.Combine(mainDirec.ToString(), "\\", imgName);

            if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, true);


            return RedirectToAction("index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}