using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
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
            return View();
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
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                string sliderPath1 = Path.Combine(originalDirectory.ToString(), "SliderImages");
                string sliderPathThumbs = Path.Combine(originalDirectory.ToString(), "SliderImages\\" + "Thumbs");

                if (!Directory.Exists(sliderPath1))
                    Directory.CreateDirectory(sliderPath1);

                if (!Directory.Exists(sliderPathThumbs))
                    Directory.CreateDirectory(sliderPathThumbs);

                //Get Name Of Image
                string imageName = sliderImage.FileName;

                var SliderGallery = db.SliderGallerys.Find(model.SliderId);

                SliderGallery.ImageName = imageName;
                db.SaveChanges();

                string path = string.Format("{0}\\{1}", sliderPath1, imageName);
                var pathThumbs = string.Format("{0}\\{1}", sliderPathThumbs, imageName);

                sliderImage.SaveAs(path);

                //Create and save thumbs Images            
                WebImage img = new WebImage(sliderImage.InputStream);
                img.Resize(img.Width, img.Height, false, true).Crop(1, 1, 1, 1).Write();
                img.Save(pathThumbs);
                TempData["SM"] = "You Added Image ToSlider Successfully..";
            }
            return RedirectToAction("AddImage");
        }

        public ActionResult DeleteSliderImage() {

            return View(db.SliderGallerys.ToList());
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