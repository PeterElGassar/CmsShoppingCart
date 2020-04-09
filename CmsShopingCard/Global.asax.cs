using CmsShopingCard.Models;
using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CmsShopingCard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        protected void Application_AuthenticateRequest()
        {
            //ckeck if user is login
            if (User == null) { return; }
            //Get UserName
            string UserName = Context.User.Identity.Name;
            //Declare array for Roles
            string[] roles = null;

            using (Db db = new Db())
            {
                //Poplate Roles
                User UserInDbDTO = db.Users.FirstOrDefault(x => x.UserName == UserName);

                roles = db.UserRoles.Where(x => x.UserId == UserInDbDTO.UserId)
                    .Select(x => x.Role.Name)
                    .ToArray();

            }
            //Build IPrincipal object
            IIdentity userIdentity = new GenericIdentity(UserName);
            IPrincipal NewUserObj = new GenericPrincipal(userIdentity, roles);

            //Update Context.User
            Context.User = NewUserObj;
        }
    }
}
