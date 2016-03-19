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
    using AcademyPlatform.Web.Umbraco.Services.Contracts;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class CourseController : RenderMvcController
    {
        private readonly ICoursesService _courses;
        private readonly ICoursesContentService _coursesContentService;
        private readonly ISubscriptionsService _subscriptions;
        private readonly ICategoryService _categories;
        private readonly IAssessmentsService _assessmentsService;
        private readonly ILecturesService _lectures;
        private readonly IUmbracoMapper _mapper;


        public CourseController(ICoursesService courses, ICategoryService categories, IUmbracoMapper mapper, ISubscriptionsService subscriptions, ILecturesService lectures, ICoursesContentService coursesContentService, IAssessmentsService assessmentsService)
        {
            _courses = courses;
            _categories = categories;
            _mapper = mapper;
            _subscriptions = subscriptions;
            _lectures = lectures;
            _coursesContentService = coursesContentService;
            _assessmentsService = assessmentsService;
        }

        [HttpGet]
        public ActionResult Course(RenderModel model)
        {
            Course coursePublishedContentViewModel = new Course();
            _mapper.AddCustomMapping(typeof(ImageViewModel).FullName, UmbracoMapperMappings.MapMediaFile)
                   .AddCustomMapping(typeof(int).FullName, UmbracoMapperMappings.MapPicker, nameof(Models.Umbraco.DocumentTypes.Course.CourseId))
                   .Map(model.Content, coursePublishedContentViewModel);

            AcademyPlatform.Models.Courses.Course course = _courses.GetById(coursePublishedContentViewModel.CourseId);
            string joinCourseUrl = Url.RouteUrl("JoinCourse", new { courseNiceUrl = model.Content.UrlName });
            string assessmentUrl = Url.RouteUrl("Assessment", new { courseNiceUrl = model.Content.UrlName });
            string profileUrl = Url.RouteUrl("Profile");
            var files = model.Content.GetPropertyValue(nameof(Models.Umbraco.DocumentTypes.Course.Files));
            CourseDetailsViewModel courseDetailsViewModel = new CourseDetailsViewModel
            {
                CourseId = course.Id,
                Category = course.Category,
                Title = course.Title,
                ImageUrl = coursePublishedContentViewModel.CoursePicture.Url,
                CoursesPageUrl = model.Content.Parent.Url,
                JoinCourseUrl = joinCourseUrl,
                AssessmentUrl = assessmentUrl,
                ProfileUrl = profileUrl,
                AssessmentEligibilityStatus = _assessmentsService.GetEligibilityStatus(User.Identity.Name, coursePublishedContentViewModel.CourseId),
                DetailedDescription = coursePublishedContentViewModel.DetailedDescription,
                ShortDescription = coursePublishedContentViewModel.ShortDescription,

            };
            //========================================================================================
            List<Module> modulesPublishedContent = new List<Module>();
            List<IPublishedContent> modulesContent = model.Content.DescendantsOrSelf(nameof(Module)).ToList();
            _mapper.Map(model.Content, coursePublishedContentViewModel)
                .MapCollection(modulesContent, modulesPublishedContent);
            foreach (IPublishedContent moduleContent in modulesContent)
            {
                ModuleViewModel module = new ModuleViewModel { Name = moduleContent.Name };
                List<IPublishedContent> lecturesContent = moduleContent.DescendantsOrSelf(nameof(Lecture)).ToList();
                foreach (IPublishedContent lectureContent in lecturesContent)
                {
                    LectureLinkViewModel lecture = new LectureLinkViewModel();
                    _mapper.Map(lectureContent, lecture);
                    lecture.IsVisited = _lectures.IsLectureVisited(User.Identity.Name, lectureContent.Id);
                    module.LectureLinks.Add(lecture);
                }
                courseDetailsViewModel.Modules.Add(module);
            }
            //====================================================================

            courseDetailsViewModel.HasActiveSubscription = User.Identity.IsAuthenticated
                                     && _subscriptions.HasActiveSubscription(
                                         User.Identity.Name,
                                         courseDetailsViewModel.CourseId);

            if (TempData.ContainsKey("ErrorMessage"))
            {
                courseDetailsViewModel.ErrorMessage = (string)TempData["ErrorMessage"];
            }
            return CurrentTemplate(courseDetailsViewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult StudentCoursePage(RenderModel model)
        {
            throw new ArgumentException("This should never be called!!!" + Request.Url.AbsolutePath);
        }
    }
}