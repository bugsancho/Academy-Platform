using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcademyPlatform.Web.Areas.Student.Controllers
{
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