namespace AcademyPlatform.Web.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Infrastructure.Filters;
    using AcademyPlatform.Web.Infrastructure.Helpers;
    using AcademyPlatform.Web.Infrastructure.Sanitizers;
    using AcademyPlatform.Web.Models.Courses;

    using AutoMapper;

    using log4net;

    //[CustomAuthorize(Roles = "admin")]
    public class CoursesController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Error(filterContext.Exception);
            base.OnException(filterContext);
        }

        private const string ImagesFolderFormat = "~\\Images\\CourseImages\\{0}";
        private readonly ICoursesService _coursesService;
        private readonly ICategoryService _categories;
        private readonly IHtmlSanitizer _sanitizer;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CoursesController(ICoursesService coursesService, IHtmlSanitizer sanitizer, ICategoryService categories)
        {
            _sanitizer = sanitizer;
            _categories = categories;
            _coursesService = coursesService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var courses = _coursesService.GetActiveCourses();
            return View(courses);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetRelatedItemsInViewBag(null);
            return View();
        }

        [HttpPost]
        public ActionResult Create(CourseEditViewModel courseEditViewModel)
        {
            if (ModelState.IsValid)
            {
                //var imagePath = FileUploadHelper.UploadImage(courseViewModel.CourseImage, string.Format(ImagesFolderFormat, courseViewModel.CourseImage.FileName));
                var course = Mapper.Map<Course>(courseEditViewModel);
                //course.ImageUrl = imagePath;
                _coursesService.Create(course);

                return RedirectToAction("Index");
            }

            SetRelatedItemsInViewBag(null);
            return View(courseEditViewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var courseInDb = _coursesService.GetById(id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }

            var course = Mapper.Map<CourseEditViewModel>(courseInDb);
            SetRelatedItemsInViewBag(courseInDb);
            return View(course);
        }

        [HttpPost]
        public ActionResult Edit(int id, CourseEditViewModel courseEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(courseEditViewModel);
            }

            var courseInDb = _coursesService.GetById(id);
            

            var course = Mapper.Map(courseEditViewModel, courseInDb);
            _coursesService.Update(course);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var course = _coursesService.GetById(id);
            if (course == null)
            {
                return HttpNotFound("Invalid course id");
            }

            _coursesService.Delete(course);
            return RedirectToAction("Index");
        }

        private void SetRelatedItemsInViewBag(Course course)
        {
            ViewBag.PossibleCategories =
                _categories.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Title, Selected = (course != null && course.CategoryId == x.Id) });
        }
    }
}