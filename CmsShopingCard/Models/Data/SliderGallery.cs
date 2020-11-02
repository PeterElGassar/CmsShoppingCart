using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
namespace CmsShopingCard.Models.Data
{
    public class SliderGallery
    {
        [Key]
        public int SliderId { get; set; }

        public string ImageName { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Url Link")]
        [Required]
        public string UrlLink { get; set; }

        public void SaveImage(HttpPostedFileBase sliderImage)
        {
            var originalDirectory =
                new DirectoryInfo(string.Format("{0}Images\\Uploads", HttpContext.Current.Server.MapPath(@"\")));

            string sliderPath1 = Path.Combine(originalDirectory.ToString(), "SliderImages");
            string sliderPathThumbs = Path.Combine(originalDirectory.ToString(), "SliderImages\\" + "Thumbs");

            if (!Directory.Exists(sliderPath1))
                Directory.CreateDirectory(sliderPath1);

            if (!Directory.Exists(sliderPathThumbs))
                Directory.CreateDirectory(sliderPathThumbs);

            //Get Name Of Image
            string imageName = sliderImage.FileName;

            this.ImageName = imageName;

            string path = string.Format("{0}\\{1}", sliderPath1, imageName);
            var pathThumbs = string.Format("{0}\\{1}", sliderPathThumbs, imageName);

            sliderImage.SaveAs(path);

            //Create and save thumbs Images            
            WebImage img = new WebImage(sliderImage.InputStream);
            img.Resize(img.Width, img.Height, false, true).Crop(1, 1, 1, 1).Write();
            img.Save(pathThumbs);
        }

        public void UpdateImage(HttpPostedFileBase file, string oldImgName)
        {
            string newImgName = file.FileName;


            var paths = GetPaths(oldImgName);

            //delete Old paths
            if (File.Exists(paths[0].ToString()))
                File.Delete(paths[0].ToString());

            if (File.Exists(paths[1]))
                File.Delete(paths[1]);

            //Create new Img
            paths = GetPaths(newImgName);

            //originalImgPath = string.Format("{0}\\{1}", pathString1, newImgName);
            //thumbImgPath = string.Format("{0}\\{1}", pathString2, newImgName);

            file.SaveAs(paths[0]);

            //Resiz Img And Save in thumb folder
            WebImage img = new WebImage(file.InputStream);
            img.Resize(391, 385, false, true).Crop(1, 1, 1, 1).Write();
            img.Save(paths[1]);


        }

        //stand by
        private string[] GetPaths(string imgName)
        {
            var mainDirec = new
                DirectoryInfo(string.Format(@"{0}Images\Uploads", HttpContext.Current.Server.MapPath(@"\")));

            string originalImgPath =
                Path.Combine(mainDirec.ToString(), "SliderImages\\", imgName);
            string thumbImgPath =
                Path.Combine(mainDirec.ToString(), "SliderImages\\" + "Thumbs\\", imgName);

            string[] paths = { originalImgPath, thumbImgPath };
            return paths;
        }

    }
}