namespace AcademyPlatform.Web
{
    using System.Web.Optimization;
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                        "~/Content/project-theme.css",
                        "~/Content/animate.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/project-theme-scripts").Include(
                //"~/Scripts/Plugins/jasny-bootstrap.js",
                "~/Scripts/Plugins/SmoothScroll.js",
                "~/Scripts/Plugins/jquery.themepunch.revolution.js",
                "~/Scripts/Plugins/jquery.themepunch.tools.min.js",
                "~/Scripts/Plugins/jquery.waypoints.js",
                "~/Scripts/Plugins/jquery.browser.js",
                "~/Scripts/template.js",
                "~/Scripts/Plugins/isotope.pkgd.js",
                "~/Scripts/Plugins/isotope.pkgd.min.js",
                "~/Scripts/Plugins/jquery.vide.js"));

            bundles.Add(new StyleBundle("~/project-theme").Include(
                       "~/Content/project-theme.css",
                       "~/Content/ProjectThemeSkins/light_blue.css",
                       "~/Content/Fonts/font-awesome.css",
                       "~/Content/hover.css",
                       "~/Content/rs-slider.css",
                       "~/Content/jasny-bootstrap.css",
                       "~/Content/animate.css"));
        }
    }
}
