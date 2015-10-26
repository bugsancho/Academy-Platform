namespace AcademyPlatform.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Courses;

    using AutoMapper;

    public class CoursesController : Controller
    {
        private readonly ICoursesService _courses;

        private readonly ICategoryService _categories;


        public CoursesController(ICoursesService courses, ICategoryService categories)
        {
            _courses = courses;
            _categories = categories;
        }

        // GET: Courses/Courses 
        public ActionResult Index()
        {
            ViewBag.Categories = _categories.GetAll();
            var courses = _courses.GetActiveCourses();
            return View(courses.ToList());
        }

        // GET: Courses/Courses/Details/5
        //[DonutOutputCache(VaryByParam = "id", Duration = 30, Location = OutputCacheLocation.Server)]
        //[Route("Courses/{prettyUrl}")]
        //public ActionResult Details(string prettyUrl)
        //{
        //    //Thread.Sleep(3000);
        //    var course = _courses.GetCourseByPrettyUrl(prettyUrl);
        //    if (course == null)
        //    {
        //        return HttpNotFound("Invalid course url");
        //    }

        //    var vm = Mapper.Map<CourseEditViewModel>(course);
        //    return View(vm);
        //}

        [HttpGet]
        [Route("Courses/{prettyUrl}/Apply")]
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