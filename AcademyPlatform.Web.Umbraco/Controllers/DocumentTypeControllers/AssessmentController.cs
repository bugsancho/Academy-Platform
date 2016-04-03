namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Assessments;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using nuPickers;

    using Newtonsoft.Json;

    using Zone.UmbracoMapper;

    [Authorize]
    public class AssessmentController : UmbracoController
    {
        private readonly IUmbracoMapper _mapper;
        private readonly IAssessmentsService _assessments;
        private readonly IFeedbackService _feedbackService;
        private readonly ICertificatesService _certificates;
        private readonly ICoursesContentService _coursesContentService;
        private readonly IUserService _users;

        //TODO improve code quality:
        // * Use xxxContentService to retrieve umbraco content
        // * Extract duplicate code between GET and POST methods
        public AssessmentController(IUmbracoMapper mapper, IAssessmentsService assessments, IUserService users, ICertificatesService certificates, ICoursesContentService coursesContentService, IFeedbackService feedbackService)
        {
            _mapper = mapper;
            _assessments = assessments;
            _users = users;
            _certificates = certificates;
            _coursesContentService = coursesContentService;
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public ActionResult Assessment()
        {
            int courseId = _coursesContentService.GetCourseId(Umbraco.AssignedContentItem);
            string username = User.Identity.Name;
            if (!_feedbackService.UserHasSentFeedback(username, courseId))
            {
                return RedirectToRoute("Feedback", new { courseNiceUrl = Umbraco.AssignedContentItem.UrlName });
            }

            if (_assessments.GetEligibilityStatus(username, courseId) != AssessmentEligibilityStatus.Eligible)
            {
                TempData["ErrorMessage"] = $"В момента нямате достъп до изпита за този курс. Моля посетете <a href=\"{Url.RouteUrl("Profile")}\" title=\"Профил\">Вашия профил </a> за повече информация.";
                return Redirect(Umbraco.AssignedContentItem.Url);
            }

            object assessmentId = Umbraco.AssignedContentItem.GetPropertyValue<Picker>(nameof(Course.Assessment)).SavedValue;
            IPublishedContent assessmentContnet = Umbraco.TypedContent(assessmentId);
            var user = _users.GetByUsername(username);
            AssessmentViewModel assessment = new AssessmentViewModel();
            List<QuestionViewModel> questionViewModel = new List<QuestionViewModel>();
            IEnumerable<IPublishedContent> questions;
            var assessmentRequest = _assessments.GetLatestForUser(user.Username, assessmentContnet.Id);
            if (assessmentRequest == null)
            {
                var numberOfQuestions =
                    assessmentContnet.GetPropertyValue<int>(
                        nameof(Models.Umbraco.DocumentTypes.Assessment.NumberOfQuestions));
                if (numberOfQuestions == default(int))
                {
                    throw new ArgumentException($"Number of questions is not set for assessment with ID: {assessmentId}");
                }

                questions = assessmentContnet.Descendants(1).OrderBy(x => Guid.NewGuid()).Take(numberOfQuestions).ToList();

                assessmentRequest = new AssessmentRequest
                {
                    AssessmentExternalId = assessmentContnet.Id,
                    QuestionIds = string.Join(",", questions.Select(x => x.Id)),
                    IsCompleted = false,
                    UserId = user.Id

                };
                _assessments.CreateAssesmentRequest(assessmentRequest);
            }
            else
            {
                questions = Umbraco.TypedContent(assessmentRequest.QuestionIds.Split(',')).ToList();
            }

            _mapper.AddCustomMapping(
                    typeof(IEnumerable<QuestionAnswer>).FullName,
                    UmbracoMapperMappings.MapQuestionAnswer)
                       .MapCollection(questions, questionViewModel)
                       .Map(assessmentContnet, assessment);

            assessment.Questions = questionViewModel;
            assessment.AssessmentRequestId = assessmentRequest.Id;

            return View(assessment);
        }

        [HttpPost]
        public ActionResult Assessment(AssessmentViewModel assessment)
        {
            int courseId = _coursesContentService.GetCourseId(Umbraco.AssignedContentItem);
            string username = User.Identity.Name;
            if (!_feedbackService.UserHasSentFeedback(username, courseId))
            {
                return RedirectToRoute("Feedback", new { courseNiceUrl = Umbraco.AssignedContentItem.UrlName });
            }

            if (_assessments.GetEligibilityStatus(username, courseId) != AssessmentEligibilityStatus.Eligible)
            {
                return Redirect(Umbraco.AssignedContentItem.Url);
            }

            if (assessment.AssessmentRequestId == default(int))
            {
                throw new ArgumentException("Assessment Id is missing from Assessment Submission. Cannot evaluate assessment");
            }

            var assessmentRequest = _assessments.GetAssessmentRequest(assessment.AssessmentRequestId);

            IEnumerable<IPublishedContent> questions = Umbraco.TypedContent(assessmentRequest.QuestionIds.Split(',').ToList());
            IPublishedContent assessmentContnet = Umbraco.TypedContent(assessmentRequest.AssessmentExternalId);
            var requiredCorrectAnswers =
                assessmentContnet.GetPropertyValue<int>(
                    nameof(Models.Umbraco.DocumentTypes.Assessment.RequiredCorrectAnswers));

            if (requiredCorrectAnswers == default(int))
            {
                throw new ArgumentException($"Number of required correct answers is not set for assessment with ID: {assessmentRequest.AssessmentExternalId}");
            }

            AssessmentViewModel assessmentViewModel = new AssessmentViewModel();
            List<QuestionViewModel> questionViewModel = new List<QuestionViewModel>();


            _mapper.AddCustomMapping(typeof(IEnumerable<QuestionAnswer>).FullName, UmbracoMapperMappings.MapQuestionAnswer)
                                .MapCollection(questions, questionViewModel)
                                .Map(assessmentContnet, assessmentViewModel);

            assessmentViewModel.Questions = questionViewModel;
            int correctAnswers = 0;
            foreach (QuestionViewModel questionInDb in assessmentViewModel.Questions)
            {
                QuestionViewModel questionAnswered = assessment.Questions.Single(x => x.Id == questionInDb.Id);
                questionAnswered.QuestionText = questionInDb.QuestionText;
                bool isCorrectlyAnswered = true;
                foreach (QuestionAnswer answerInDb in questionInDb.Answers)
                {
                    QuestionAnswer answer = questionAnswered.Answers.Single(x => x.Index == answerInDb.Index);
                    answer.Text = answerInDb.Text;
                    if (answer.IsCorrect != answerInDb.IsCorrect)
                    {
                        isCorrectlyAnswered = false;
                        break;
                    }
                }

                if (isCorrectlyAnswered)
                {
                    correctAnswers++;
                }
            }


            var submission = new AssessmentSubmission
            {
                CourseId = courseId,
                AssessmentRequestId = assessmentRequest.Id,
                Submission = JsonConvert.SerializeObject(assessment.Questions)
            };

            if (correctAnswers >= requiredCorrectAnswers)
            {
                submission.IsSuccessful = true;
            }

            _assessments.CreateAssessmentSubmission(submission);

            if (submission.IsSuccessful)
            {
                object certificateId = Umbraco.AssignedContentItem.GetPropertyValue<Picker>(nameof(Course.Certificate)).SavedValue;
                IPublishedContent certificateContent = Umbraco.TypedContent(certificateId);
                var certificateGenerationInfo = new CertificateGenerationInfo();

                var certificateTemplate = UmbracoContext.Current.MediaCache.GetById(
                    certificateContent.GetPropertyValue<int>(
                        nameof(AcademyPlatform.Web.Models.Umbraco.DocumentTypes.Certificate.CertificateTemplate)));

                _mapper.AddCustomMapping(
                        typeof(PlaceholderInfo).FullName,
                        UmbracoMapperMappings.MapPlaceholder)
                    .Map(certificateContent, certificateGenerationInfo);

                certificateGenerationInfo.TemplateFilePath = Server.MapPath(certificateTemplate.Url);
                certificateGenerationInfo.BaseFilePath = Server.MapPath("/");
                var certificate = _certificates.GenerateCertificate(User.Identity.Name, courseId, submission, certificateGenerationInfo);

                return RedirectToRoute("Certificate", new { certificateUniqueCode = certificate.UniqueCode });
            }



            //ViewBag.AssessmentSuccessful = true;
            //return View("~/Views/Certificate/Certificate.cshtml", model: $"\\certificates\\{certificate.UniqueCode}.jpeg");
            return View("AssesmentCompletion", new AssessmentSubmissionResult { CorrectlyAnswered = correctAnswers });
        }
    }

    public class AssessmentSubmissionResult
    {
        public int CorrectlyAnswered { get; set; }

        public int IncorrectlyAnswered { get; set; }
    }
}