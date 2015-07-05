namespace AcademyPlatform.Web.Areas.Student.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AcademyPlatform.Services;
    using AcademyPlatform.Web.Models.Courses;
    using AutoMapper;

    public class CoursesController : Controller
    {
        private readonly ICoursesService coursesService;


        public CoursesController(ICoursesService coursesService)
        {
            this.coursesService = coursesService;
        }

        // GET: Courses/Courses
        public ActionResult Index()
        {
            var courses = coursesService.GetActiveCourses();
            return View(courses.ToList());
        }

        // GET: Courses/Courses/Details/5
        //[DonutOutputCache(VaryByParam = "id", Duration = 30, Location = OutputCacheLocation.Server)]
        public ActionResult Details(string id)
        {
            //Thread.Sleep(3000);
            var course = coursesService.GetCourseByPrettyUrl(id);
            if (course == null)
            {
                return HttpNotFound("Invalid course id");
            }

            var vm = Mapper.Map<CourseViewModel>(course);
            return View("~/Areas/Student/Views/Courses/Details.cshtml", vm);
        }


        [HttpGet]
        public ActionResult Apply()
        {
            return View();
        }

        public ActionResult ApplicationCompleted()
        {
            return View();
        }
    }
}