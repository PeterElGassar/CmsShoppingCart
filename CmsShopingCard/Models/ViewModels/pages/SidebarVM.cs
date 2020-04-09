using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShopingCard.Models.ViewModels.pages
{
    public class SidebarVM
    {
        public SidebarVM(Sidebar row)
        {
            Id = row.Id;
            body = row.body;
        }

        public SidebarVM()
        {

        }
        public int Id { get; set; }

        [AllowHtml]
        public string body { get; set; }
    }
}