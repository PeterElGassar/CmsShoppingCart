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
            List<ProductVM> productListVM;
            var CategoryInDb = db.Categories.Where(x => x.Slug == categoryName).SingleOrDefault();

            if (brandId > 0)
            {
                //Here Get the product if it concerns a specific brand
                catId = CategoryInDb.Id;

                productListVM = db.Products.ToArray()
                   .Where(x => x.CategoryId == catId && x.BrandId == brandId)
                   .Select(x => new ProductVM(x))
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
                   .Select(x => new ProductVM(x))
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
            Product ProdectInDb = db.Products.Where(p => p.Slug == name).FirstOrDefault();
            id = ProdectInDb.Id;

            //Get Id Of this Product
            model = new ProductVM(ProdectInDb);



            //Get Gallery Images
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                  .Select(fn => Path.GetFileName(fn));
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

            return PartialView(db.Categories.ToList());
        }

        // GET: QuickViewProduct
        [HttpGet]
        public ActionResult QuickViewProduct(int Id)
        {
            Product ProdectInDb = db.Products.Where(x => x.Id == Id).FirstOrDefault();
            int ID = ProdectInDb.Id;

            var model = new ProductVM(ProdectInDb);

            //To Fill Image Gallery OF this is Entity
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + ID + "/Gallery/Thumbs"))
                   .Select(fn => Path.GetFileName(fn));
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchText)
        {
            var result = db.Products.ToArray()
                .Where(x => x.Name.Contains(searchText)
            || x.Description.Contains(searchText)
            || x.Slug.Contains(searchText)
            || x.Category.Name.Contains(searchText)).Select(x => new ProductVM(x)).ToList();


            return View(result);
        }


        // GET: Shop/GetProductQuantity
        //public int GetProductQuantity2(int productId)
        //{
        //    Product ProductInDb = db.Products.Where(x => x.Id == productId).FirstOrDefault();

        //    int Quantity = ProductInDb.Quantity;
        //    ////return Redirect("Category", productListVM);
        //    //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //    //string jsonString = javaScriptSerializer.Serialize(ProductInDb);
        //    return Quantity;
        //}

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