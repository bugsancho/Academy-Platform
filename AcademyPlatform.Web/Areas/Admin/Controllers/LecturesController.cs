namespace AcademyPlatform.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    public class LecturesController : Controller
    {

        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Courses");
        }

        [HttpGet]
        public ActionResult List(int courseId)
        {

            return View();
        }
    }
}