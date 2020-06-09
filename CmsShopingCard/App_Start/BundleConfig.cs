using System.Web;
using System.Web.Optimization;

namespace CmsShopingCard
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/newTemplet/js/popper.min.js",
                        "~/Content/newTemplet/js/bootstrap.min.js",
                        "~/Content/newTemplet/js/plugins.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/scripts/datatables/dataTables.bootstrap.js",
                        "~/Content/newTemplet/js/main.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/Modules").Include(
                 "~/Scripts/App/Services/NotificationsService.js",
                        "~/Scripts/App/Services/AddToCartService.js",
                        "~/Scripts/App/Controllers/CartController.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/content/datatables/css/dataTables.bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/DashboardStyle.css",
                      "~/Content/toastr.css"
                      ));
        }
    }
}
