namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Assessments;
    using AcademyPlatform.Web.Models.Surveys;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using nuPickers;

    using Newtonsoft.Json;

    using Zone.UmbracoMapper;

    using Course = AcademyPlatform.Web.Models.Umbraco.DocumentTypes.Course;

    [Authorize]
    public class FeedbackController : UmbracoController
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ICoursesContentService _coursesContentService;
        private readonly IUserService _userService;
        private readonly ISubscriptionsService _subscriptionsService;
        private readonly IUmbracoMapper _mapper;

        public FeedbackController(IFeedbackService feedbackService, ICoursesContentService coursesContentService, IUmbracoMapper mapper, IUserService userService, ISubscriptionsService subscriptionsService)
        {
            _feedbackService = feedbackService;
            _coursesContentService = coursesContentService;
            _mapper = mapper;
            _userService = userService;
            _subscriptionsService = subscriptionsService;
        }

        [HttpGet]
        public ActionResult Feedback()
        {
            int courseId = _coursesContentService.GetCourseId(Umbraco.AssignedContentItem);
            string username = User.Identity.Name;
            if (!_subscriptionsService.HasActiveSubscription(username, courseId))
            {
                return Redirect(Umbraco.AssignedContentItem.Url);
            }

            if (_feedbackService.UserHasSentFeedback(username, courseId))
            {
                return RedirectToRoute("Assessment", new { courseNiceUrl = Umbraco.AssignedContentItem.UrlName });
            }

            FeedbackViewModel feedbackViewModel = GetFeedbackViewModel();
            return View(feedbackViewModel);
        }

        [HttpPost]
        public ActionResult Feedback(FeedbackViewModel feedbackViewModel)
        {
            int courseId = _coursesContentService.GetCourseId(Umbraco.AssignedContentItem);
            string username = User.Identity.Name;
            if (!_subscriptionsService.HasActiveSubscription(username, courseId))
            {
                return Redirect(Umbraco.AssignedContentItem.Url);
            }

            if (!_feedbackService.UserHasSentFeedback(username, courseId))
            {
                FeedbackViewModel feedbackViewModelTemplate = GetFeedbackViewModel();
                MergeFeedbackModels(feedbackViewModel, feedbackViewModelTemplate);

                if (!ModelState.IsValid)
                {
                    return View(feedbackViewModel);
                }

                User user = _userService.GetByUsername(username);
                Feedback feedback = new Feedback
                {
                    CourseId = courseId,
                    UserId = user.Id,
                    Submission = JsonConvert.SerializeObject(feedbackViewModel.Questions),
                    AdditionalNotes = feedbackViewModel.AdditionalNotes
                };

                _feedbackService.Create(feedback);
            }

            return RedirectToRoute("Assessment", new { courseNiceUrl = Umbraco.AssignedContentItem.UrlName });
        }

        private static void MergeFeedbackModels(FeedbackViewModel feedbackViewModel, FeedbackViewModel feedbackViewModelTemplate)
        {
            //TODO move and reuse in AssessmentController
            feedbackViewModel.Description = feedbackViewModelTemplate.Description;
            foreach (QuestionViewModel questionInDb in feedbackViewModelTemplate.Questions)
            {
                QuestionViewModel questionAnswered = feedbackViewModel.Questions.Single(x => x.Id == questionInDb.Id);
                questionAnswered.QuestionText = questionInDb.QuestionText;
                foreach (QuestionAnswer answerInDb in questionInDb.Answers)
                {
                    QuestionAnswer answer = questionAnswered.Answers.Single(x => x.Index == answerInDb.Index);
                    answer.Text = answerInDb.Text;
                }
            }
        }

        private FeedbackViewModel GetFeedbackViewModel()
        {
            //TODO move and reuse logic in AssessmentController
            object feedbackFormId = Umbraco.AssignedContentItem.GetPropertyValue<Picker>(nameof(Course.FeedbackForm)).SavedValue;
            IPublishedContent feedbackContent = Umbraco.TypedContent(feedbackFormId);

            var feedbackViewModel = new FeedbackViewModel();
            List<QuestionViewModel> questionViewModel = new List<QuestionViewModel>();
            IEnumerable<IPublishedContent> questions = feedbackContent.Descendants(1).OrderBy(x => Guid.NewGuid()).ToList();

            _mapper.AddCustomMapping(
                typeof(IEnumerable<QuestionAnswer>).FullName,
                UmbracoMapperMappings.MapQuestionAnswer)
                   .MapCollection(questions, questionViewModel)
                   .Map(feedbackContent, feedbackViewModel);

            feedbackViewModel.Questions = questionViewModel;
            return feedbackViewModel;
        }
    }
}