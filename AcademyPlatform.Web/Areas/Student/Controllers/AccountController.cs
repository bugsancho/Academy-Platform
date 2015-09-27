namespace AcademyPlatform.Web.Areas.Student.Controllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Courses;

    using AutoMapper.QueryableExtensions;

    public class AccountController : Controller
    {
        private ICoursesService _courses;

        public AccountController(ICoursesService courses)
        {
            _courses = courses;
        }

        // GET: Student/Home
        public ActionResult Index()
        {
            return View("~/Areas/Student/Views/Account/Index.cshtml", _courses.GetActiveCourses().Project().To<CoursesListViewModel>());
        }
    }
}