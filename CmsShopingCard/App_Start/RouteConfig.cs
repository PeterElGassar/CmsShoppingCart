using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CmsShopingCard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute("Logout", "User/{action}", new { controller = "User", action = "Logout" });

            //routes.MapRoute("VerifyAccount", "User/{action}/{id}", new { controller = "User", action = "VerifyAccount", id = UrlParameter.Optional }, new[] { "CmsShopingCard.Controllers" });


            //routes.MapRoute("User", "User/{action}/{name}", new { controller = "User", action = "Registration", name = UrlParameter.Optional }, new[] { "CmsShopingCard.Controllers" });
            /////////////


            routes.MapRoute("Account", "Account/{action}/{name}", new { controller = "Account", action = "Index", name = UrlParameter.Optional }, new[] { "CmsShopingCard.Controllers" });
            routes.MapRoute("Account_UserProfile", "Account/user-profile/{name}",new { controller = "Account",action= "user-profile", name = UrlParameter.Optional },new[] { "CmsShopingCard.Controllers" });
            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name = UrlParameter.Optional }, new[] { "CmsShopingCard.Controllers" });


            routes.MapRoute("Cart", "Cart/{action}", new { controller = "Cart", action = "PlaceOrder", }, new[] { "CmsShopingCard.Controllers" });


            routes.MapRoute("SidebarPartial", "Pages/SidebarPartial", new { controller = "Pages", action = "SidebarPartial" }, new[] { "CmsShopingCard.Controllers" });
            routes.MapRoute("PagesMenuPartial", "Pages/PagesMenuPartial", new { controller = "Pages", action = "PagesMenuPartial" }, new[] { "CmsShopingCard.Controllers" });
            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "CmsShopingCard.Controllers" });
            //Name Space here To Spcifiec Pages/Index in User Area Not Admin 
            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "CmsShopingCard.Controllers" });

            

        }
    }
}
