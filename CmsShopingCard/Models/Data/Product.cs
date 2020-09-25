using CmsShopingCard.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CmsShopingCard.Models.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        [AllowHtml]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }


        public string ImageName { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }


        public int? BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }


        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ILookup<int, WishList> UserWishLists { get; set; }

        public void AddImage(ProductVM model, HttpPostedFileBase file)
        {
            //Create Necessary directories
            var originalDirectory = new
                DirectoryInfo(string.Format("{0}Images\\Uploads", HttpContext.Current.Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + this.Id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + this.Id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + this.Id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + this.Id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            //check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {

                //init Image Name
                string imageName = file.FileName;

                //Save Image to dto Or Anthor Word  In DataBase
                this.ImageName = imageName;

                //Set Original And thumb Image Paths
                //Path That file is named Contans Number Like 10 15 17 etc..
                var path = string.Format("{0}\\{1}", pathString2, imageName);

                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                //Save original
                file.SaveAs(path);

                //Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(img.Width, img.Height, false, true).Crop(1, 1, 1, 1).Write();
                img.Save(path2);

            }


        }
    }
}