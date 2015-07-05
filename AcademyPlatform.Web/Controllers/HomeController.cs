namespace AcademyPlatform.Web.Controllers
{
    using DevTrends.MvcDonutCaching;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ComingSoon()
        {
            return View();
        }

        public ActionResult RenderNavigation()
        {
            return View("_MainNavigation");
        }
    }
}