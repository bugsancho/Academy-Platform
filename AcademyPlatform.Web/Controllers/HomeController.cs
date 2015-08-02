namespace AcademyPlatform.Web.Controllers
{
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

        [Route("ComingSoon")]
        [AllowAnonymous]
        public ActionResult ComingSoon()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RenderNavigation()
        {
            return View("_MainNavigation");
        }
    }
}