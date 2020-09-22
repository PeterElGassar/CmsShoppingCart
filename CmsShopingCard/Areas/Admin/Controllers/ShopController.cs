using CmsShopingCard.Areas.Admin.ViewModels.Brand;
using CmsShopingCard.Areas.Admin.ViewModels.Shop;
using CmsShopingCard.Models;
using CmsShopingCard.Models.Data;
using CmsShopingCard.Models.ViewModels.Shop;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CmsShopingCard.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShopController : Controller
    {
        Db db;
        public ShopController()
        {
            db = new Db();
        }

        // GET: Admin/Shop/Categories
        //use to add category also
        public ActionResult Categories()
        {
            List<CategoriesVM> model = db.Categories.ToArray()
                      .OrderBy(x => x.Sorting)
                      .Select(x => new CategoriesVM(x)).ToList();

            return View(model);
        }

        // POST: Admin/Shop/AddNewCategory
        //بستخدمها عن طريق ajax 
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            //Declare id 
            string id;

            if (db.Categories.Any(x => x.Name == catName))
                return "titletaken";

            Category CategoryInDb = new Category();

            //Add to dto
            CategoryInDb.Name = catName;
            CategoryInDb.Slug = catName.Replace(" ", "-").ToLower();
            CategoryInDb.Sorting = 100;

            //save dto
            db.Categories.Add(CategoryInDb);
            db.SaveChanges();

            //Get the id after save it In DB to called agein
            id = CategoryInDb.Id.ToString();

            //return Id
            return id;
        }

        // POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] ids)
        {
            //Set Init Count
            int count = 1;

            //Declare Dto 
            Category dto;

            //Set Sorting For Each Page
            foreach (var CatId in ids)
            {
                dto = db.Categories.Find(CatId);
                dto.Sorting = count;
                db.SaveChanges();

                count++;
            }

        }

        // GET: Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {

            //get specific Category to remove it
            var dto = db.Categories.Find(id);

            db.Categories.Remove(dto);
            var categoryList = db.CategoryBrands.Where(x => x.CategoryId == id).ToList();
            //remove category from brand table
            foreach (var item in categoryList)
            {
                db.CategoryBrands.Remove(item);
            }
            db.SaveChanges();

            return RedirectToAction("Categories");
        }


        // POST: Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {

            if (db.Categories.Any(x => x.Name == newCatName))
            {
                return "titletaken";
            }

            Category dto = db.Categories.Find(id);
            dto.Name = newCatName;
            dto.Slug = newCatName.Replace(" ", "-").ToLower();

            db.SaveChanges();

            return "ok";
        }

        // GET: Admin/Shop/RenameCategory
        public ActionResult AddProduct()
        {
            ProductVM model = new ProductVM();
            //Create DropDown List To Selecte Category
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            return View(model);
        }

        // POST: Admin/Shop/RenameCategory
        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            //Check Model State
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                return View(model);
            }

            //Make Sure Product Name Is Unique
            if (db.Products.Where(x => x.Id != model.Id).Any(x => x.Name == model.Name))
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                ModelState.AddModelError("", "That Product Name Is Token Before.");
                return View(model);
            }

            //Declare Id
            int id;

            //init and Save ProductDto
            Product ProductInDb = new Product();
            ProductInDb.Name = model.Name;
            ProductInDb.Slug = model.Name.Replace(" ", "-").ToLower();
            ProductInDb.Description = model.Description;
            ProductInDb.Price = model.Price;
            ProductInDb.Quantity = model.Quantity;
            ProductInDb.CategoryId = model.CategoryId;
            if (model.BrandId > 0 || model.BrandId != null)
            {
                ProductInDb.BrandId = model.BrandId;
            }
            Category catDTO = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
            ProductInDb.CategoryName = catDTO.Name;

            db.Products.Add(ProductInDb);
            db.SaveChanges();

            //Get the Id
            id = ProductInDb.Id;


            TempData["SM"] = "You Have Added a Product";

            #region Upload Image
            //Create Necessary directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

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
                Product dto = db.Products.Find(id);
                dto.ImageName = imageName;
                db.SaveChanges();

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
            #endregion

            return RedirectToAction("AddProduct");
        }

        // GET: Admin/Shop/Products
        //This Action Used PagedList PlugIn
        public ActionResult Products(int? page, int? catId)
        {
            //Set Page Number
            var pageNumber = page ?? 1;

            //Init the List ProductVM
            List<ProductVM> listOfProductVm = db.Products.ToArray()
                              .Where(x => catId == 0 || catId == null || x.CategoryId == catId)
                              .Select(x => new ProductVM(x))
                              .ToList();

            //Populate categories select list
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            //To Set Selected Category
            ViewBag.SelectCat = catId.ToString();

            //Set Pagination
            var onePageOfProducts = listOfProductVm.ToPagedList(pageNumber, 5);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(listOfProductVm);
        }


        // GET: Admin/Shop/Products/id
        public ActionResult EditProduct(int id)
        {
            //Get The Product
            Product ProducInDb = db.Products.Find(id);
            //Make Sure Product Exists
            if (ProducInDb == null)
            {
                return Content("This Page Is Not Contant");
            }
            //Init Model
            ProductVM model = new ProductVM(ProducInDb);
            if (ProducInDb.BrandId > 0)
            {
                model.BrandId = ProducInDb.BrandId;
                //Make a Brand Select List
                string brandsString = new BrandController().GetBrandsOfCategory(ProducInDb.CategoryId);

                //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                //List<Brand> brandList = (List<Brand>)javaScriptSerializer.Deserialize(brandsString, typeof(List<Brand>));
                List<Brand> brandList = JsonConvert.DeserializeObject<List<Brand>>(brandsString);

                model.Brands = brandList;
            }

            //Make A Categories Select List
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                .Select(fn => Path.GetFileName(fn));

            return View(model);
        }

        // POST: Admin/Shop/Products
        [HttpPost]
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase file)
        {
            //Get Product Id
            int id = model.Id;
            //Populate Categories List And Gallery Images

            ///Fill List Just One Time
            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            ///          
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                    .Select(fn => Path.GetFileName(fn));
            //Chack Model State
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Make Sure If a Name Product Unique
            //Make sure titel is unique
            if (db.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
            {
                ModelState.AddModelError("", "This Name Or Is Alredy Exists.. Please Change It");
                //model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                return View(model);
            }

            //Update Product
            Product ProductInDb = db.Products.FirstOrDefault(x => x.Id == id);

            ProductInDb.Name = model.Name;
            ProductInDb.Slug = model.Name.Replace(" ", "-").ToLower();
            ProductInDb.Description = model.Description;
            ProductInDb.Price = model.Price;
            ProductInDb.Quantity = model.Quantity;
            ProductInDb.CategoryId = model.CategoryId;
            ProductInDb.ImageName = model.ImageName;
            if (model.BrandId > 0 )
            {
                ProductInDb.BrandId = model.BrandId;
            }
            var CATname = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
            ProductInDb.CategoryName = CATname.Name;

            db.SaveChanges();

            //Set TempDate Message
            TempData["SM"] = "You Have Edited The Product..!";

            #region Upload Images
            //Check For File  Upload
            if (file != null && file.ContentLength > 0)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));


                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file1 in di1.GetFiles())
                    file1.Delete();


                foreach (FileInfo file2 in di2.GetFiles())
                    file2.Delete();

                string imageName = file.FileName;


                Product dto = db.Products.Find(id);
                dto.ImageName = imageName;
                db.SaveChanges();


                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);
                file.SaveAs(path);



                WebImage img = new WebImage(file.InputStream);
                img.Resize(391, 385, false, true).Crop(1, 1, 1, 1).Write();
                img.Save(path2);

            }
            #endregion
            return RedirectToAction("EditProduct");
        }

        // GET: Admin/DeleteProduct/Products/id
        public ActionResult DeleteProduct(int id)
        {
            Product ProductInDb = db.Products.Find(id);
            db.Products.Remove(ProductInDb);
            db.SaveChanges();

            //Remove Product Folder
            var OriginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string PathString = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString());
            if (Directory.Exists(PathString))
                Directory.Delete(PathString, true);

            return RedirectToAction("Products");
        }

        public void SaveGalleryImages(int id)
        {
            //Loop Through files
            foreach (string fileName in Request.Files)
            {

                //init the file
                HttpPostedFileBase file = Request.Files[fileName];

                //Check it's not null
                if (fileName != null && file.ContentLength > 0)
                {
                    //Set Directory Path
                    var originalDirectory = new DirectoryInfo(string.Format("{0}\\Images\\Uploads", Server.MapPath(@"\")));
                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

                    //Set Image Path
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    //Save Original Ana thumb
                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(720, 480, false, true).Crop(1, 1, 1, 1).Write();
                    img.Save(path2);

                }

            }

        }

        // POST: Admin/DeleteProduct/Products/id
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }

        public ActionResult Orders()
        {
            //Inital List Of OrderForAdmin
            List<OrdersForAdmin> OrdersForAdmin = new List<OrdersForAdmin>();

            //Initial List OrderVM From OrdersDto By Prametriz Constractor
            var orders = db.Orders.ToList().Select(x => new OrderVM(x)).ToList();

            //Loop Through List Of OrdersVM
            foreach (var order in orders)
            {
                //Initial Producs Dictianary
                Dictionary<string, int> ProductsAndQty = new Dictionary<string, int>();
                decimal total = 0m;

                //Inial List of  OrderDetailsDTO
                List<OrderDetails> orderDetialsList = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                //Get Username

                User User = db.Users.Where(x => x.UserId == order.UserId).FirstOrDefault();
                string username = User.UserName;

                //Loop Through the List of OrderDetails
                foreach (var orderDetials in orderDetialsList)
                {
                    //Get Spacefic Product
                    Product product = db.Products.Where(x => x.Id == orderDetials.ProductId).FirstOrDefault();

                    //Get Product Price
                    decimal price = product.Price;
                    //Name Of Product
                    string productName = product.Name;
                    //Add to Product  Dictionary
                    ProductsAndQty.Add(productName, orderDetials.Quantity);
                    //Get Total
                    total += orderDetials.Quantity + price;
                }

                //Add For OrdersForAdminVM
                OrdersForAdmin.Add(new OrdersForAdmin()
                {
                    OrderNumber = order.OrderId,
                    UserName = username,
                    Total = total,
                    ProductAndQuantity = ProductsAndQty,
                    CreatedAt = order.CreateAt
                });
            }

            return View(OrdersForAdmin);
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