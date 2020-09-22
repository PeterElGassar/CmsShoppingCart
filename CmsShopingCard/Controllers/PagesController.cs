using CmsShopingCard.Models;
using CmsShopingCard.Models.Data;
using CmsShopingCard.Models.ViewModels.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShopingCard.Controllers
{
    public class PagesController : Controller
    {
        Db db;
        public PagesController()
        {
            db = new Db();
        }

        // GET: Index/{Page}
        public ActionResult Index(string page = "")
        {
            //Get/Set Page Slug
            if (page == "")
                page = "home";
            
            //Check If  page exists
            //Because If User Enterd Wrong Url returnd to index page
            if (!db.Pages.Any(x => x.Sulg.Equals(page)))
            {
                return RedirectToAction("Index", new { page = "" });
            }
            //Get Page Dto
            Page pageInDb = db.Pages.Where(x => x.Sulg == page).FirstOrDefault();

            //set page titel In ViewBag
            ViewBag.PageTitel = pageInDb.Titel;

            //Here check that Condetion for make Space For Pages that have Siedbar
            if (pageInDb.HasSidebar == true)
            {
                ViewBag.Siedbar = "Yes";
            }
            else
            {
                ViewBag.Siedbar = "No";
            }

            //Init Model 
            var model = new PageVM(pageInDb);
            model.SliderGallery = db.SliderGallerys.ToList();
            return View(model);
        }

        public ActionResult PagesMenuPartial()
        {            
            //get All Pages Except Home Page

            List<PageVM> PageVMList = db.Pages.ToArray()
                .OrderBy(x => x.Sorting)
                .Select(x => new PageVM(x)).ToList();

            return PartialView(PageVMList);
        }

        public ActionResult SidebarPartial()
        {


            Sidebar dto = db.Sidebar.Find(1);
            SidebarVM model = new SidebarVM(dto);


            return PartialView(model);
        }
    }
}