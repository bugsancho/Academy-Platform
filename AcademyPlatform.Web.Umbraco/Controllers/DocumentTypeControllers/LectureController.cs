namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    [Authorize]
    public class LectureController : RenderMvcController
    {
        private readonly ILecturesService _lectures;
        private readonly IUmbracoMapper _mapper;
        private readonly ISubscriptionsService _subscriptions;

        public LectureController(IUmbracoMapper mapper, ISubscriptionsService subscriptions, ILecturesService lectures)
        {
            _mapper = mapper;
            _subscriptions = subscriptions;
            _lectures = lectures;
        }

        public override ActionResult Index(RenderModel model)
        {
            if (!_subscriptions.HasActiveSubscription(User.Identity.Name, model.Content.AncestorOrSelf(nameof(Course)).GetPropertyValue<int>(nameof(Course.CourseId))))
            {
                return HttpNotFound();
            }
            
            var lectures = model.Content.Siblings();
            var lectureViewModel = new LectureViewModel();
            
            var precedingLecture = model.Content.PrecedingSibling(nameof(Lecture));
            if (precedingLecture != null)
            {
                lectureViewModel.PreviousLecture = new LectureLinkViewModel();
                _mapper.Map(precedingLecture, lectureViewModel.PreviousLecture);
            }

            var followingLecture = model.Content.FollowingSibling(nameof(Lecture));
            if (followingLecture != null)
            {
                lectureViewModel.NextLecture = new LectureLinkViewModel();
                _mapper.Map(followingLecture, lectureViewModel.NextLecture);
            }

            lectureViewModel.Content = model.Content.GetPropertyValue<string>(nameof(Lecture.Content));
            foreach (var lectureContent in lectures)
            {
                var lecture = new LectureLinkViewModel();
                _mapper.Map(lectureContent, lecture);
                lectureViewModel.OtherLectures.Add(lecture);
            }

            _lectures.TrackLectureVisit(User.Identity.Name, model.Content.Id);
            return CurrentTemplate(lectureViewModel);
        }
    }
}