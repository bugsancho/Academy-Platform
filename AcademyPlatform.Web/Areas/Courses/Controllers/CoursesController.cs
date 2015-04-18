namespace AcademyPlatform.Web.Areas.Courses.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services;
    using AcademyPlatform.Web.Models.Courses;
    using AutoMapper;

    public class CoursesController : Controller
    {
        private ICoursesService coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            this.coursesService = coursesService;
        }

        // GET: Courses/Courses
        public ActionResult Index()
        {
            return View();
        }

        // GET: Courses/Courses/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Courses/Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Courses/Create
        [HttpPost]
        public ActionResult Create(CreateCourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                var course = Mapper.Map<Course>(courseViewModel);
                coursesService.CreateCourse(course);

                return RedirectToAction("Index");
            }

            return View(courseViewModel);
        }

        // GET: Courses/Courses/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Courses/Courses/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Courses/Courses/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Courses/Courses/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}