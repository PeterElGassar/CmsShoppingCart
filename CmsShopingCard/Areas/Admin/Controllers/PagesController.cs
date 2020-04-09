using CmsShopingCard.Models.ViewModels.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CmsShopingCard.Models;
using CmsShopingCard.Models.Data;

namespace CmsShopingCard.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        Db db;
        public PagesController()
        {
            db = new Db();
        }


        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare list of PageVM and //Initiail the list

            List<PageVM> PageList = db.Pages
                    .OrderBy(x => x.Sorting).ToArray()
                    .Select(p => new PageVM(p))
                    .ToList();

            return View(PageList);
        }

        // GET: Admin/Pages/AddPage
        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //check model State
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Declare slug
            string slug;
            //Initial  pageDto
            Page pageDto = new Page();
            //Dto Titel
            pageDto.Titel = model.Titel;

            //Check for and set slug if need be
            if (string.IsNullOrWhiteSpace(model.Sulg))
            {
                slug = model.Titel.Replace(" ", "-").ToLower();
            }
            else
            {
                slug = model.Sulg.Replace(" ", "-").ToLower();
            }

            //Check is slug and titel is unique
            if (db.Pages.Any(x => x.Titel == model.Titel) || db.Pages.Any(x => x.Sulg == model.Sulg))
            {
                ModelState.AddModelError("", "This Titel Or Sulg Is Alredy Exists..");
                return View(model);
            }

            //Dto the rest
            pageDto.Sulg = slug;
            pageDto.Body = model.Body;
            pageDto.HasSidebar = model.HasSidebar;
            pageDto.Sorting = 100;
            //Save DTO
            db.Pages.Add(pageDto);
            db.SaveChanges();

            //Set Tamp Data Message
            TempData["SM"] = "You have added new Page..";
            //Redirect
            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/Editpage/id
        public ActionResult Editpage(int id)
        {
            //get the page
            Page dto = db.Pages.FirstOrDefault(x => x.Id == id);

            //confirm page exisces
            if (dto == null)
            {
                return Content("This Page Is Not Exist.");
            }
            // init PageVM
            PageVM model = new PageVM(dto);

            return View(model);
        }
        [HttpPost]
        public ActionResult Editpage(PageVM model)
        {
            //Check State 
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int id = model.Id;
            string slug = "home";

            Page PageInDb = db.Pages.Find(id);

            PageInDb.Titel = model.Titel;

            //Check Slug And And Set It If Need Be
            if (model.Sulg != "home")
            {
                if (string.IsNullOrWhiteSpace(model.Sulg))
                {
                    slug = model.Titel.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Sulg.Replace(" ", "-").ToLower();
                }
            }

            //Make sure titel and slug is unique
            // هنا بتفحص id != any other pages 
            //علشان تقارن لو اي صفحة تانية كان عندها نفس العنوان وليس الصفحة التي يتم تعديلها
            if (db.Pages.Where(x => x.Id != id).Any(x => x.Titel == model.Titel)
                || db.Pages.Where(x => x.Id != id).Any(x => x.Sulg == slug))
            {
                ModelState.AddModelError("", "This Titel Or Sulg Is Alredy Exists..");
                return View(model);
            }
            //Dto the rest
            PageInDb.Sulg = slug;
            PageInDb.Body = model.Body;
            PageInDb.HasSidebar = model.HasSidebar;
            db.SaveChanges();

            //Set TempDate Message
            TempData["Sm"] = " Page Is Updated Success.";
            return RedirectToAction("Editpage");
        }

        public ActionResult PageDetails(int id)
        {
            PageVM model;

            var dto = db.Pages.SingleOrDefault(x => x.Id == id);
            if (dto == null)
            {
                return Content("The Page Is Not Exist.");
            }
            model = new PageVM(dto);

            return View(model);
        }

        // GET: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {

            //get specific page you want to remove it
            var PageIdDb = db.Pages.Find(id);

            db.Pages.Remove(PageIdDb);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ReorderPages(int[] ids)
        {

            //Set Init Count
            int count = 1;
            //Declare Dto 
            Page dto;
            //Set Sorting For Each Page
            foreach (var PageId in ids)
            {
                dto = db.Pages.Find(PageId);
                dto.Sorting = count;
                db.SaveChanges();

                count++;
            }

        }
        // GET: Admin/Pages/EditSidebar

        [HttpGet]
        public ActionResult EditSidebar()
        {

            Sidebar sideBarInDb = db.Sidebar.FirstOrDefault(x => x.Id == 1);

            SidebarVM model = new SidebarVM()
            {
                Id = sideBarInDb.Id,
                body = sideBarInDb.body
            };

            return View(model);
        }
        // POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {

            Sidebar dto = db.Sidebar.Find(1);

            dto.body = model.body;
            db.SaveChanges();

            TempData["SM"] = "You Have Eidt Sidebar.!";

            return RedirectToAction("EditSidebar");
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