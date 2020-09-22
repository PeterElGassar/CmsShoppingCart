using CmsShopingCard.Models;
using CmsShopingCard.Models.Data;
using CmsShopingCard.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CmsShopingCard.Areas.Admin.ViewModels.Brand;
using System.Web.Script.Serialization;
using PagedList;

namespace CmsShopingCard.Controllers
{
    public class ShopController : Controller
    {

        Db db;

        public ShopController()
        {
            db = new Db();
        }
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            //init List Of CategoryVM

            List<CategoriesVM> CategoryVmList = db.Categories
                .OrderBy(c => c.Name).ToArray()
                .Select(x => new CategoriesVM(x)).ToList();

            foreach (var category in CategoryVmList)
            {
                //var brands = from b in db.Brands
                //             join cTOb in db.CategoryBrands
                //             on b.BrandId equals cTOb.BrandId
                //             where cTOb.CategoryId == category.Id && b.BrandId == cTOb.BrandId
                //             select new
                //             {
                //                 b.Name,
                //                 b.BrandId,
                //                 b.ImageName
                //             };

                var brands = from b in db.Brands
                             from cTOb in db.CategoryBrands
                             where cTOb.CategoryId == category.Id && b.BrandId == cTOb.BrandId
                             select new
                             {
                                 b.BrandId,
                                 b.Name,
                                 b.ImageName
                             };

                var BrandCheckBox = new List<CheckBoxVM>();
                foreach (var brand in brands)
                {
                    BrandCheckBox.Add(new CheckBoxVM()
                    {
                        Id = brand.BrandId,
                        Name = brand.Name,
                        ImageName = brand.ImageName
                    });
                }
                category.Brands = BrandCheckBox;
            }


            //return Partial View With 
            return PartialView(CategoryVmList);
        }

        // GET: Shop/Category/name
        public ActionResult Category(string categoryName, int? brandId, int? page)
        {
            int catId;
            Lookup<int, WishList> wishlistProducts = null;
            //check if product in wishlist or not
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;
                int userId = db.Users.FirstOrDefault(u => u.UserName == userName).UserId;

                //get all product in wishList to this user
                wishlistProducts = (Lookup<int, WishList>)db.WishLists
                   .Where(w => w.UserId == userId)
                   .ToList()
                   .ToLookup(w => w.productId);
            }


            List<ProductVM> productListVM;
            var CategoryInDb = db.Categories
                .Where(x => x.Slug == categoryName)
                .SingleOrDefault();


            if (brandId > 0)
            {
                //Here Get the product if it's in specific brand
                catId = CategoryInDb.Id;
                productListVM = db.Products
                   .ToArray()
                   .Where(x => x.CategoryId == catId && x.BrandId == brandId)
                   .Select(x => new ProductVM(x)
                   {
                       WishLists = wishlistProducts
                   })
                   .ToList();

                var brand = db.Brands.Find(brandId);
                ViewBag.BrandName = brand.Name;
                ViewBag.BrandId = brandId;
            }
            else
            {
                //Here Get the product Without brand
                catId = CategoryInDb.Id;
                //Init the List Of Product
                productListVM = db.Products.ToArray()
                   .Where(x => x.CategoryId == catId)
                   .Select(x => new ProductVM(x)
                   {
                       WishLists = wishlistProducts
                   })
                   .ToList();
            }



            //Get Category Name
            ViewBag.CategoryName = CategoryInDb.Name;
            //Pagination
            var pageNumber = page ?? 1;
            var onePageOfProducts = productListVM.ToPagedList(pageNumber, 12);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            return View(productListVM);
        }


        // GET: Shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            ProductVM model;

            int id = 0;

            //If  Not Found This Product Return to Indix Page
            if (!db.Products.Any(x => x.Slug.Equals(name)))
                return RedirectToAction("Index", "Pages");

            //Declare Dto
            Product ProdectInDb = db.Products
                .Where(p => p.Slug == name)
                .FirstOrDefault();

            //Get Id Of this Product
            id = ProdectInDb.Id;

            model = new ProductVM(ProdectInDb);

            //Get Gallery Images
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                  .Select(fn => Path.GetFileName(fn));

            //check if product in wishlist or not
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;
                int userId = db.Users.FirstOrDefault(u => u.UserName == userName).UserId;
                model.InWishlist = db.WishLists.Any(w => w.UserId == userId && w.productId == id);
            }
            //Retur View With Model
            return View("ProductDetails", model);
        }



        // GET: Shop/product-details/name
        public ActionResult GetProductQuantity(int productId)
        {
            Product ProductInDb = db.Products.Where(x => x.Id == productId).FirstOrDefault();
            ProductVM model = new ProductVM(ProductInDb);

            return PartialView(model);
        }

        // GET: GetProductByCategory
        public ActionResult GetProductByCategory()
        {
            CategoriesVM catVm = new CategoriesVM();

            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var userId = db.Users
                    .FirstOrDefault(u => u.UserName == userName)
                    .UserId;

                //get all product in wishList to this user
                var wishlistProducts = db.WishLists
                    .Where(w => w.UserId == userId)
                    .ToList()
                    .ToLookup(w => w.productId);

                catVm = new CategoriesVM()
                {
                    IsAuth = User.Identity.IsAuthenticated,
                    WishLists = wishlistProducts,
                    Categories = db.Categories.ToList()

                };
                return PartialView(catVm);

            }

            catVm.Categories = db.Categories.ToList();
            return PartialView(catVm);
        }

        // GET: QuickViewProduct
        [HttpGet]
        public ActionResult QuickViewProduct(int Id)
        {
            Product ProdectInDb = db.Products.Where(x => x.Id == Id).FirstOrDefault();
            int id = ProdectInDb.Id;

            var model = new ProductVM(ProdectInDb);


            //check if product in wishlist or not
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;
                int userId = db.Users.FirstOrDefault(u => u.UserName == userName).UserId;
                model.InWishlist = db.WishLists.Any(w => w.UserId == userId && w.productId == id);
            }


            //To Fill Image Gallery OF this is Entity
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                   .Select(fn => Path.GetFileName(fn));
            return PartialView(model);
        }



        // GET: Shop/Search
        [HttpGet]
        public ActionResult Search(int? page, string prefix)
        {

            var result = db.Products.ToArray()
                            .Where(x => x.Name.Contains(prefix)
                        || x.Description.Contains(prefix)
                        || x.Slug.Contains(prefix)
                        || x.Category.Name.Contains(prefix))
                        .Select(x => new ProductVM(x)).ToList();

            //prefix to seconde page
            ViewBag.prefix = prefix;
            //Pagination
            var pageNumber = page ?? 1;
            var onePageOfProducts = result.ToPagedList(pageNumber, 12);

            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(result);
        }


        // POST: Shop/Search
        [HttpPost]
        public ActionResult Search(string prefix, int? page)
        {
            var result = db.Products.ToArray()
                .Where(x => x.Name.Contains(prefix)
            || x.Description.Contains(prefix)
            || x.Slug.Contains(prefix)
            || x.Category.Name.Contains(prefix))
            .Select(x => new ProductVM(x)).ToList();

            //prefix to seconde page
            ViewBag.prefix = prefix;
            //Pagination
            var pageNumber = page ?? 1;
            var onePageOfProducts = result.ToPagedList(pageNumber, 12);

            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(result);
        }




        //GET: Shop/GetProductQuantity
        //public int GetProductQuantity2(int productId)
        //{
        //    Product ProductInDb = db.Products.Where(x => x.Id == productId).FirstOrDefault();

        //    int Quantity = ProductInDb.Quantity;
        //    ////return Redirect("Category", productListVM);
        //    //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //    //string jsonString = javaScriptSerializer.Serialize(ProductInDb);
        //    return Quantity;
        //}


        [HttpGet]
        public JsonResult AutoComplete(string prefix)
        {
            var product2s = (from pro in db.Products
                             where pro.Name.StartsWith(prefix)
                             || pro.Name.ToLower().Contains(prefix.ToLower())
                             select new { ProductName = pro.Name, ProductImg = pro.ImageName, ProductId = pro.Id, ProductSlug = pro.Slug })
                             .Take(8)
                             .ToList();

            //var products = db.Products.Where(p => p.Name.StartsWith(prefix)).Select(p => p.Name).ToList();

            return Json(product2s, JsonRequestBehavior.AllowGet);
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