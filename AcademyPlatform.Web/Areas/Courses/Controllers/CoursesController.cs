using AcademyPlatform.Models.Courses;
using AcademyPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcademyPlatform.Web.Areas.Courses.Controllers
{
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
        public ActionResult Create(Course course)
        {
            try
            {
                if (!coursesService.CreateCourse(course))
                {
                    foreach (var error in coursesService.GetErrors(course))
                    {
                        ModelState.AddModelError(error.MemberNames.First(), error.ErrorMessage);
                    }

                    return View();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
