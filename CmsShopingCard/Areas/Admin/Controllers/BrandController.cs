using CmsShopingCard.Areas.Admin.ViewModels.Brand;
using CmsShopingCard.Models.Data;
using CmsShopingCard.Models.Dtos;
using CmsShopingCard.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CmsShopingCard.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        Db db;
        public BrandController()
        {
            db = new Db();
        }
        // GET: Admin/Brand
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        //GET: Admin/Brand/GetBrands
        public JsonResult GetBrands()
        {
            var brands = db.Brands
                .Select(x => new BrandDto()
                {
                    Name = x.Name,
                    ImageName = x.ImageName,
                    BrandId = x.BrandId
                })
                .ToList();

            return Json(new { data = brands }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brand/Create
        [HttpPost]
        public ActionResult Create(Brand model, HttpPostedFileBase BrandImage)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Make Sure Product Name Is Unique
            if (db.Brands.Where(x => x.BrandId != model.BrandId).Any(x => x.Name == model.Name))
            {
                ModelState.AddModelError("BrandName", "That Product Name Is Token Before.");
                return View(model);
            }

            db.Brands.Add(model);
            db.SaveChanges();
            int id = model.BrandId;
            TempData["SM"] = "You Have Added a New Brand";

            #region Upload Image
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var brandsPath1 = Path.Combine(originalDirectory.ToString(), "Brands");
            var brandsPath2 = Path.Combine(originalDirectory.ToString(), "Brands\\" + id.ToString());
            var brandsPath3 = Path.Combine(originalDirectory.ToString(), "Brands\\" + id.ToString() + "\\Thumbs");

            if (!Directory.Exists(brandsPath1))
                Directory.CreateDirectory(brandsPath1);

            if (!Directory.Exists(brandsPath2))
                Directory.CreateDirectory(brandsPath2);

            if (!Directory.Exists(brandsPath3))
                Directory.CreateDirectory(brandsPath3);

            //////////////////
            if (BrandImage != null && BrandImage.ContentLength > 0)
            {
                //get Name of Image
                string imageName = BrandImage.FileName;

                // Save Image to dto Or Anthor Word  In DataBase
                Brand BrandIbDb = db.Brands.Find(id);
                BrandIbDb.ImageName = imageName;
                db.SaveChanges();

                //Path That file is named Contans Number Like 10 15 17 etc..
                var path = string.Format("{0}\\{1}", brandsPath2, imageName);
                var path2 = string.Format("{0}\\{1}", brandsPath3, imageName);

                BrandImage.SaveAs(path);

                //Create and save thumb
                WebImage img = new WebImage(BrandImage.InputStream);
                img.Resize(300, 300, false, true).Crop(1, 1, 1, 1).Write();
                img.Save(path2);
            }
            #endregion

            return RedirectToAction("Create");

        }

        // GET: Admin/Brand/DeleteBrand/id
        public ActionResult DeleteBrand(int id)
        {
            //get specific Category to remove it
            var dto = db.Brands.Find(id);

            db.Brands.Remove(dto);
            db.SaveChanges();

            //Remove Product Folder
            var OriginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string PathString = Path.Combine(OriginalDirectory.ToString(), "Brands\\" + id.ToString());
            if (Directory.Exists(PathString))
                Directory.Delete(PathString, true);

            return RedirectToAction("Index");
        }

        // GET: Admin/Brand/EditBrand/id
        public ActionResult EditBrand(int id)
        {
            //get specific Category to remove it
            var BrandInDb = db.Brands.Find(id);

            return View(BrandInDb);
        }


        // POST: Admin/Shop/EditBrand/id
        [HttpPost]
        public ActionResult EditBrand(Brand model, HttpPostedFileBase BrandImage)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int id = model.BrandId;

            //Make sure titel is unique
            if (db.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
            {
                ModelState.AddModelError("BrandName", "This Name Or Is Alredy Exists.. Please Change It");
                return View(model);
            }
            //get specific Category to remove it
            var BrandInDb = db.Brands.FirstOrDefault(x => x.BrandId == id);
            BrandInDb.Name = model.Name;
            BrandInDb.ImageName = model.ImageName;
            db.SaveChanges();

            TempData["SM"] = "You Have Edited The Product..!";
            #region Upload Edit Image
            if (BrandImage != null && BrandImage.ContentLength > 0)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Brands\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Brands\\" + id.ToString() + "\\Thumbs");

                DirectoryInfo directory1 = new DirectoryInfo(pathString1);
                DirectoryInfo directory2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file in directory1.GetFiles())
                {
                    file.Delete();
                }

                foreach (FileInfo file2 in directory2.GetFiles())
                {
                    file2.Delete();
                }


                string imageName = BrandImage.FileName;


                Brand dto = db.Brands.Find(id);
                BrandInDb.ImageName = imageName;
                db.SaveChanges();


                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);
                BrandImage.SaveAs(path);



                WebImage img = new WebImage(BrandImage.InputStream);
                img.Resize(391, 385, false, true).Crop(1, 1, 1, 1).Write();
                img.Save(path2);
            }

            #endregion
            return RedirectToAction("EditBrand");
        }

        // GET: Admin/Shop/EditCategory/id
        public ActionResult EditCategory(int id)
        {
            //get specific Category
            var CategoryInDb = db.Categories.Find(id);

            if (CategoryInDb == null)
            {
                return HttpNotFound();
            }
            ////////~~~~~~

            var Result = from B in db.Brands
                         select new
                         {
                             B.BrandId,
                             B.Name,
                             B.ImageName,
                             Checked = (
                             (from CtoB in db.CategoryBrands
                              where (CtoB.CategoryId == id) & (CtoB.BrandId == B.BrandId)
                              select CtoB).Count() > 0
                                         )
                         };
            ///////~~~~~~~~~~
            var CatViewModel = new CategoriesVM()
            {
                Id = id,
                Name = CategoryInDb.Name,
            };
            /////~~~~~~~
            var checkBoxList = new List<CheckBoxVM>();

            foreach (var item in Result)
            {
                checkBoxList.Add(new CheckBoxVM
                {
                    Id = item.BrandId,
                    ImageName = item.ImageName,
                    Name = item.Name,
                    Checked = item.Checked
                });
            }
            ///////~~~~~~~~~~
            CatViewModel.Brands = checkBoxList;
            return View(CatViewModel);
        }

        [HttpGet]
        public ActionResult EditCategoryPartial(int id)
        {
            //get specific Category
            var CategoryInDb = db.Categories.Find(id);

            if (CategoryInDb == null)
            {
                return HttpNotFound();
            }
            ////////~~~~~~

            var Result = from B in db.Brands
                         select new
                         {
                             B.BrandId,
                             B.Name,
                             B.ImageName,
                             Checked = (
         (from CtoB in db.CategoryBrands
          where (CtoB.CategoryId == id) & (CtoB.BrandId == B.BrandId)
          select CtoB).Count() > 0
                     )
                         };
            ///////~~~~~~~~~~
            var CatViewModel = new CategoriesVM()
            {
                Id = id,
                Name = CategoryInDb.Name,
            };
            /////~~~~~~~
            var checkBoxList = new List<CheckBoxVM>();

            foreach (var item in Result)
            {
                checkBoxList.Add(new CheckBoxVM
                {
                    Id = item.BrandId,
                    ImageName = item.ImageName,
                    Name = item.Name,
                    Checked = item.Checked
                });
            }
            ///////~~~~~~~~~~
            CatViewModel.Brands = checkBoxList;
            return PartialView("", CatViewModel);
        }

        [HttpPost]
        public JsonResult EditCategoryPartial(CategoriesVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, message = "Form invalid.." }, JsonRequestBehavior.AllowGet);
            }

            //Delete All Brand underneath this Category
            foreach (var item in db.CategoryBrands)
            {
                if (item.CategoryId == model.Id)
                {
                    db.Entry(item).State = EntityState.Deleted;
                }
            }
            //Assign all new brand for this Category
            foreach (var item in model.Brands)
            {
                if (item.Checked)
                {
                    db.CategoryBrands.Add(
                        new CategoryBrand() { CategoryId = model.Id, BrandId = item.Id }
                        );
                }
            }

            db.SaveChanges();

            return Json(new { status = true, message = "Category Updated." }, JsonRequestBehavior.AllowGet);
        }


        // GET: Admin/Shop/EditCategory
        [HttpPost]
        public ActionResult EditCategory(CategoriesVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var CategoryInDb = db.Categories.Find(model.Id);
            CategoryInDb.Name = model.Name;
            CategoryInDb.Slug = model.Name.Replace(" ", "-").ToLower();

            //Delete All Brand underneath this Category
            foreach (var item in db.CategoryBrands)
            {
                if (item.CategoryId == model.Id)
                {
                    db.Entry(item).State = EntityState.Deleted;
                }
            }
            //Assign all new brand for this Category
            foreach (var item in model.Brands)
            {
                if (item.Checked)
                {
                    db.CategoryBrands.Add(
                        new CategoryBrand() { CategoryId = model.Id, BrandId = item.Id }
                        );
                }
            }

            db.SaveChanges();

            return RedirectToAction("Categories", "Shop");
        }

        public ActionResult DetailsCategory(int id)
        {
            Category CategoryInDb = db.Categories.Find(id);
            if (CategoryInDb == null)
            {
                return HttpNotFound();
            }
            //===================
            var Result = from B in db.Brands
                         select new
                         {
                             B.BrandId,
                             B.Name,
                             B.ImageName,
                             //For Check if brand underneath this category or not
                             Checked = (
                             (from CtoB in db.CategoryBrands
                              where (CtoB.CategoryId == id) & (CtoB.BrandId == B.BrandId)
                              select CtoB).Count() > 0
                                         )
                         };

            var CategoryVM = new CategoriesVM()
            {
                Id = id,
                Name = CategoryInDb.Name,
            };

            var BrandCheckBox = new List<CheckBoxVM>();
            foreach (var item in Result)
            {
                BrandCheckBox.Add(new CheckBoxVM
                {
                    Id = item.BrandId,
                    Name = item.Name,
                    ImageName = item.ImageName,
                    Checked = item.Checked
                });
            }

            CategoryVM.Brands = BrandCheckBox;

            return View(CategoryVM);
        }

        [HttpGet]
        public string GetBrandsOfCategory(int catId)
        {
            //get specific
            //var CategoryInDb = db.Categories.Find(catId);


            var brands = from b in db.Brands
                         from cTOb in db.CategoryBrands
                         where cTOb.CategoryId == catId && b.BrandId == cTOb.BrandId
                         select new
                         {
                             b.Name,
                             b.BrandId,
                         };

            //هياخد IList<Btand>
            //var productList2 = new List<Brand>();
            var brandList = new List<object>();

            foreach (var brand in brands)
            {
                brandList.Add(new { brandId = brand.BrandId, Name = brand.Name });

            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(brandList);

            return jsonString;
        }

        [HttpGet]
        public ActionResult CreateByPopup()
        {
            var Brand = new Brand();
            return PartialView("BrandForm", Brand);
        }

        [HttpPost]
        public ActionResult CreateByPopup(Brand model, HttpPostedFileBase BrandImage)
        {

            var result = false;

            if (!ModelState.IsValid)
                return Json(result, JsonRequestBehavior.AllowGet);

            //Make Sure Product Name Is Unique
            if (db.Brands.Where(x => x.BrandId != model.BrandId).Any(x => x.Name == model.Name))
            {
                //return Content("That Product Name Is Token Before.");
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            db.Brands.Add(model);
            db.SaveChanges();

            if (SaveBrandImg(model, BrandImage))
                return Content("success");

            // Return json with data
            return Content("success");
            //return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult EditBrandPartial(int id)
        {
            var brandInDb = db.Brands.SingleOrDefault(b => b.BrandId == id);

            if (brandInDb == null)
            {
                return HttpNotFound();
            }


            return PartialView("BrandForm", brandInDb);
        }

        [HttpPost]
        public ActionResult EditBrandPartial(Brand model)
        {
            var result = false;

            if (!ModelState.IsValid)
            {
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //Make Sure Product Name Is Unique
            if (db.Brands.Where(x => x.BrandId != model.BrandId).Any(x => x.Name == model.Name))
            {
                //return Content("That Product Name Is Token Before.");
                return Json(result, JsonRequestBehavior.AllowGet);
            }


            var brandInDb = db.Brands.SingleOrDefault(b => b.BrandId == model.BrandId);

            if (brandInDb == null)
                return HttpNotFound();
            brandInDb.Name = model.Name;
            brandInDb.ImageName = model.ImageName;
            db.SaveChanges();

            int id = model.BrandId;
            #region update Image
            if (model.BrandImage != null && model.BrandImage.ContentLength > 0)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Brands\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Brands\\" + id.ToString() + "\\Thumbs");

                DirectoryInfo directory1 = new DirectoryInfo(pathString1);
                DirectoryInfo directory2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file in directory1.GetFiles())
                {
                    file.Delete();
                }

                foreach (FileInfo file2 in directory2.GetFiles())
                {
                    file2.Delete();
                }

                string imageName = model.BrandImage.FileName;
                brandInDb.ImageName = imageName;
                db.SaveChanges();


                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);
                model.BrandImage.SaveAs(path);


                WebImage img = new WebImage(model.BrandImage.InputStream);
                img.Resize(391, 385, false, true).Crop(1, 1, 1, 1).Write();
                img.Save(path2);
            }

            #endregion

            return Content("success");
        }

        [NonAction]
        private bool SaveBrandImg(Brand model, HttpPostedFileBase BrandImage)
        {
            int id = model.BrandId;
            bool result = false;

            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var brandsPath1 = Path.Combine(originalDirectory.ToString(), "Brands");
            var brandsPath2 = Path.Combine(originalDirectory.ToString(), "Brands\\" + id.ToString());
            var brandsPath3 = Path.Combine(originalDirectory.ToString(), "Brands\\" + id.ToString() + "\\Thumbs");

            if (!Directory.Exists(brandsPath1))
                Directory.CreateDirectory(brandsPath1);

            if (!Directory.Exists(brandsPath2))
                Directory.CreateDirectory(brandsPath2);

            if (!Directory.Exists(brandsPath3))
                Directory.CreateDirectory(brandsPath3);

            //////////////////
            if (BrandImage != null && BrandImage.ContentLength > 0)
            {
                //get Name of Image
                string imageName = BrandImage.FileName;

                // Save Image to dto Or Anthor Word  In DataBase
                Brand BrandIbDb = db.Brands.Find(id);
                BrandIbDb.ImageName = imageName;
                db.SaveChanges();

                //Path That file is named Contans Number Like 10 15 17 etc..
                var path = string.Format("{0}\\{1}", brandsPath2, imageName);
                var path2 = string.Format("{0}\\{1}", brandsPath3, imageName);

                BrandImage.SaveAs(path);

                //Create and save thumb
                WebImage img = new WebImage(BrandImage.InputStream);
                img.Resize(300, 300, false, true).Crop(1, 1, 1, 1).Write();
                img.Save(path2);
                result = true;
            }

            return result;

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
