namespace AcademyPlatform.Web.Areas.Courses.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class LecturesController : Controller
    {
        // GET: Courses/Lectures
        public ActionResult Create()
        {
            return View();
        }
    }
}