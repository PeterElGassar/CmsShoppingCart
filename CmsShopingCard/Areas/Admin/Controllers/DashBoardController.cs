﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShopingCard.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashBoardController : Controller
    {
        // GET: Admin/DashBoard
        public ActionResult Index()
        {
            ViewBag.DashBoardIndex = true;
            return View();
        }
    }
}