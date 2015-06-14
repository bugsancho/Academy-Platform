namespace AcademyPlatform.Web.Areas.Student.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services;
    using AcademyPlatform.Web.Models.Courses;
    using AutoMapper;
    using System.Collections.Generic;

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
            return View();
        }

        // GET: Courses/Courses/Details/5
        public ActionResult Details(int id)
        {
            var course = coursesService.GetCourseById(id);
            if (course == null)
            {
                return HttpNotFound("Invalid course id");
            }

            var vm = Mapper.Map<CourseViewModel>(course);
            return View(vm);
        }

        // GET: Courses/Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Courses/Create
        [HttpPost]
        public ActionResult Create(CourseViewModel courseViewModel)
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
            var courseInDb = coursesService.GetCourseById(id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }

            var course = Mapper.Map<CourseViewModel>(courseInDb);
            return View(course);
        }

        // POST: Courses/Courses/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                var courseInDb = coursesService.GetCourseById(id);
                var course = Mapper.Map(courseViewModel, courseInDb);
                coursesService.UpdateCourse(course);

                return RedirectToAction("Index");
            }

            return View(courseViewModel);
        }


        // POST: Courses/Courses/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var course = coursesService.GetCourseById(id);
            if (course == null)
            {
                return HttpNotFound("Invalid course id");
            }

            coursesService.DeleteCourse(course);
            return RedirectToAction("Index");
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