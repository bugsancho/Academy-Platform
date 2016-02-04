namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Common;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class CourseController : RenderMvcController
    {
        private readonly ICoursesService _courses;
        private readonly ISubscriptionsService _subscriptions;
        private readonly ICategoryService _categories;
        private readonly IUmbracoMapper _mapper;


        public CourseController(ICoursesService courses, ICategoryService categories, IUmbracoMapper mapper, ISubscriptionsService subscriptions)
        {
            _courses = courses;
            _categories = categories;
            _mapper = mapper;
            _subscriptions = subscriptions;
        }

        [HttpGet]
        public ActionResult Course(RenderModel model)
        {
            var coursePublishedContentViewModel = new Course();
            _mapper.AddCustomMapping(typeof(ImageViewModel).FullName, UmbracoMapperMappings.MapMediaFile)
                   .Map(model.Content.AncestorOrSelf(), coursePublishedContentViewModel);

            var course = _courses.GetById(coursePublishedContentViewModel.CourseId);
            var joinCourseUrl = Url.RouteUrl("JoinCourse", new { courseNiceUrl = model.Content.UrlName });

            var courseDetailsViewModel = new CourseDetailsViewModel
            {
                CourseId = course.Id,
                Category = course.Category,
                Title = course.Title,
                LecturerName = "The great Lecturer",
                ImageUrl = coursePublishedContentViewModel.CoursePicture.Url,
                CoursesPageUrl = model.Content.Parent.Url,
                JoinCourseUrl = joinCourseUrl,
                DetailedDescription = coursePublishedContentViewModel.DetailedDescription,
                ShortDescription = coursePublishedContentViewModel.ShortDescription,

            };
            //========================================================================================
            var modulesPublishedContent = new List<Module>();
            var modulesContent = model.Content.DescendantsOrSelf(nameof(Module)).ToList();
            _mapper.Map(model.Content, coursePublishedContentViewModel)
                .MapCollection(modulesContent, modulesPublishedContent);
            foreach (var moduleContent in modulesContent)
            {
                var module = new ModuleViewModel();
                module.Name = moduleContent.Name;
                var lecturesContent = moduleContent.DescendantsOrSelf(nameof(Lecture)).ToList();
                foreach (var lectureContent in lecturesContent)
                {
                    var lecture = new LectureLinkViewModel();
                    _mapper.Map(lectureContent, lecture);
                    lecture.IsVisited = _subscriptions.IsLectureVisited(User.Identity.Name, lectureContent.Id);
                    module.LectureLinks.Add(lecture);
                }
                courseDetailsViewModel.Modules.Add(module);
            }
            //====================================================================

            if (User.Identity.IsAuthenticated && _subscriptions.HasActiveSubscription(User.Identity.Name, courseDetailsViewModel.CourseId))
            {
                var studentPageUrl = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(StudentPage)).FirstOrDefault()?.Url + model.Content.UrlName;
                ViewBag.StudentPageUrl = studentPageUrl;
            }

            return CurrentTemplate(courseDetailsViewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult StudentCoursePage(RenderModel model)
        {
            if (!_subscriptions.HasActiveSubscription(User.Identity.Name, model.Content.GetPropertyValue<int>(nameof(Models.Umbraco.DocumentTypes.Course.CourseId))))
            {
                return HttpNotFound();
            }
            var coursePublishedContentViewModel = new Course();
            var modulesPublishedContent = new List<Module>();
            var courseViewModel = new CourseStudentPageViewModel();
            var modulesContent = model.Content.DescendantsOrSelf(nameof(Module)).ToList();

            _mapper.Map(model.Content, coursePublishedContentViewModel)
                .MapCollection(modulesContent, modulesPublishedContent);

            var course = _courses.GetById(coursePublishedContentViewModel.CourseId);

            courseViewModel.Title = course.Title;
            courseViewModel.DetailedDescription = coursePublishedContentViewModel.DetailedDescription;



            foreach (var moduleContent in modulesContent)
            {
                var module = new ModuleViewModel();
                module.Name = moduleContent.Name;
                var lecturesContent = moduleContent.DescendantsOrSelf(nameof(Lecture)).ToList();
                foreach (var lectureContent in lecturesContent)
                {
                    var lecture = new LectureLinkViewModel();
                    _mapper.Map(lectureContent, lecture);
                    lecture.IsVisited = _subscriptions.IsLectureVisited(User.Identity.Name, lectureContent.Id);
                    module.LectureLinks.Add(lecture);
                }
                courseViewModel.Modules.Add(module);
            }


            return CurrentTemplate(courseViewModel);
        }



    }
}