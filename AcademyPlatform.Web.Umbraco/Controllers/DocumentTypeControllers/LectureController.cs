namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class LectureController : RenderMvcController
    {
        private readonly ILecturesService _lectures;
        private readonly IUmbracoMapper _mapper;
        private readonly ISubscriptionsService _subscriptions;
        private readonly ICoursesContentService _coursesContentService;

        public LectureController(IUmbracoMapper mapper, ISubscriptionsService subscriptions, ILecturesService lectures, ICoursesContentService coursesContentService)
        {
            _mapper = mapper;
            _subscriptions = subscriptions;
            _lectures = lectures;
            _coursesContentService = coursesContentService;
        }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            var isDemo = UmbracoContext.Current.PublishedContentRequest.PublishedContent.GetPropertyValue<bool>(
                nameof(Lecture.IsDemo));
            if (!isDemo && !User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public override ActionResult Index(RenderModel model)
        {
            IPublishedContent course = model.Content.AncestorOrSelf(nameof(Course));
            int courseId = _coursesContentService.GetCourseId(course);
            bool isDemoLecture = model.Content.GetPropertyValue<bool>(nameof(Lecture.IsDemo));
            if (!isDemoLecture && !_subscriptions.HasActiveSubscription(User.Identity.Name, courseId))
            {
                TempData["ErrorMessage"] = "Моля, запишете се за курса, за да имате достъп до лекциите";
                return Redirect(course.Url);
            }

            IPublishedContent[] lectures = model.Content.AncestorsOrSelf(nameof(Course)).DescendantsOrSelf(nameof(Lecture)).ToArray();
            LectureViewModel lectureViewModel = new LectureViewModel();
            IPublishedContent precedingLecture = null;
            IPublishedContent followingLecture = null;
            for (int i = 0; i < lectures.Length; i++)
            {
                if (lectures[i].Id == model.Content.Id)
                {
                    if (i > 0)
                    {
                        precedingLecture = lectures[i - 1];
                    }
                    if (i < lectures.Length - 1)
                    {
                        followingLecture = lectures[i + 1];
                    }
                    break;

                }
            }

            if (precedingLecture != null)
            {
                lectureViewModel.PreviousLecture = new LectureLinkViewModel();
                _mapper.Map(precedingLecture, lectureViewModel.PreviousLecture);
            }

            if (followingLecture != null)
            {
                lectureViewModel.NextLecture = new LectureLinkViewModel();
                _mapper.Map(followingLecture, lectureViewModel.NextLecture);
            }

            lectureViewModel.Content = model.Content.GetPropertyValue<string>(nameof(Lecture.Content));

            Dictionary<int, ModuleViewModel> modulesDictionary = new Dictionary<int, ModuleViewModel>();
            foreach (IPublishedContent lectureContent in lectures)
            {
                int currentModuleId = lectureContent.Parent.Id;
                if (!modulesDictionary.ContainsKey(currentModuleId))
                {
                    modulesDictionary.Add(currentModuleId, new ModuleViewModel { Name = lectureContent.Parent.Name });
                }
                LectureLinkViewModel lecture = new LectureLinkViewModel();
                _mapper.Map(lectureContent, lecture);

                if (lectureContent.Id == model.Content.Id)
                {
                    lecture.IsCurrent = true;
                }

                modulesDictionary[currentModuleId].LectureLinks.Add(lecture);
            }

            lectureViewModel.Modules = modulesDictionary.Values;

            if (User.Identity.IsAuthenticated)
            {
                _lectures.TrackLectureVisit(User.Identity.Name, model.Content.Id);
            }

            return CurrentTemplate(lectureViewModel);
        }
    }
}