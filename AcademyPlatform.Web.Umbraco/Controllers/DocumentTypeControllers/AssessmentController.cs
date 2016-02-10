namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Assessments;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using ImageProcessor;
    using ImageProcessor.Imaging;

    using nuPickers;

    using Newtonsoft.Json;

    using Zone.UmbracoMapper;

    public class AssessmentController : UmbracoController
    {
        private readonly IUmbracoMapper _mapper;
        private readonly IAssessmentsService _assessments;
        private readonly ICertificatesService _certificates;
        private readonly IUserService _users;

        public AssessmentController(IUmbracoMapper mapper, IAssessmentsService assessments, IUserService users, ICertificatesService certificates)
        {
            _mapper = mapper;
            _assessments = assessments;
            _users = users;
            _certificates = certificates;
        }

        [HttpGet]
        public ActionResult Assessment()
        {
            object assessmentId = Umbraco.AssignedContentItem.GetPropertyValue<Picker>(nameof(Course.Assessment)).SavedValue;
            IPublishedContent assessmentContnet = Umbraco.TypedContent(assessmentId);
            var user = _users.GetByUsername(User.Identity.Name);
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
                questions = Umbraco.TypedContent(assessmentRequest.QuestionIds.Split(',').ToList());
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
            int wrongAnswers = 0;
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
                else
                {
                    wrongAnswers++;
                }
            }

            var courseId = Umbraco.AssignedContentItem.GetPropertyValue<int>(nameof(Course.CourseId));
            var submission = new AssessmentSubmission
            {
                CourseId = courseId,
                AssessmentRequestId = assessmentRequest.Id,
                SubmissionDate = DateTime.Now,
                Submission = JsonConvert.SerializeObject(assessment.Questions)
            };

            if (correctAnswers >= requiredCorrectAnswers)
            {
                submission.IsSuccessful = true;
            }

            _assessments.CreateAssessmentSubmission(submission);

            

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
            //ViewBag.AssessmentSuccessful = true;
            //return View("~/Views/Certificate/Certificate.cshtml", model: $"\\certificates\\{certificate.UniqueCode}.jpeg");
            //return View("AssesmentCompletion", new AssessmentSubmissionResult { CorrectlyAnswered = correctAnswers, IncorrectlyAnswered = wrongAnswers });
        }
    }

    public class AssessmentSubmissionResult
    {
        public int CorrectlyAnswered { get; set; }

        public int IncorrectlyAnswered { get; set; }
    }
}