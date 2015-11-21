namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Web.Models.Assessments;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class AssessmentController : RenderMvcController
    {
        private readonly IUmbracoMapper _mapper;

        public AssessmentController(IUmbracoMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Assessment(RenderModel model)
        {
            var questions = model.Content.Descendants(1);

            var assessment = new AssessmentViewModel();
            var questionViewModel = new List<QuestionViewModel>();


            _mapper.AddCustomMapping(typeof(IEnumerable<QuestionAnswer>).FullName, UmbracoMapperMappings.MapQuestionAnswer)
                                .MapCollection(questions, questionViewModel)
                                .Map(model.Content, assessment);
             questionViewModel.ForEach(x =>
                {
                    x.QuestionType = x.Answers == null ? QuestionType.FreeText : QuestionType.MultipleChoice;
                });
            assessment.Questions = questionViewModel;

            return View(assessment);
        }

        [HttpPost]
        public ActionResult Assessment(AssessmentViewModel assessment)
        {
            var questions = Umbraco.TypedContent(assessment.Id).Descendants(1);

            var assessmentViewModel = new AssessmentViewModel();
            var questionViewModel = new List<QuestionViewModel>();


            _mapper.AddCustomMapping(typeof(IEnumerable<QuestionAnswer>).FullName, UmbracoMapperMappings.MapQuestionAnswer)
                                .MapCollection(questions, questionViewModel)
                                .Map(Umbraco.TypedContent(assessment.Id), assessmentViewModel);

            questionViewModel.ForEach(x =>
                {
                    x.QuestionType = x.Answers == null ? QuestionType.FreeText : QuestionType.MultipleChoice;
                });

            assessmentViewModel.Questions = questionViewModel;
            int correctAnswers = 0;
            int wrongAnswers = 0;
            foreach (var questionInDb in assessmentViewModel.Questions.Where(x => x.QuestionType != QuestionType.FreeText))
            {
                var questionAnswered = assessment.Questions.First(x => x.Id == questionInDb.Id);
                var isCorrectlyAnswered = true;
                foreach (var answerInDb in questionInDb.Answers)
                {
                    var answer = questionAnswered.Answers.First(x => x.Index == answerInDb.Index);
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
            return View("AssesmentCompletion", new AssessmentSubmissionResult { CorrectlyAnswered = correctAnswers, IncorrectlyAnswered = wrongAnswers});
        }
    }

    public class AssessmentSubmissionResult
    {
        public int CorrectlyAnswered { get; set; }

        public int IncorrectlyAnswered { get; set; }

        public IEnumerable<string> FreeTextAnswers { get; set; }
    }
}