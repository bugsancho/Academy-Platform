namespace AcademyPlatform.Web.Models.Assessments
{
    using System.Collections.Generic;

    public class AssessmentViewModel
    {
        public int AssessmentRequestId { get; set; }

        public string Description { get; set; }

        public int NumberOfQuestions { get; set; }

        public int RequiredCorrectAnswers { get; set; }

        public IEnumerable<QuestionViewModel> Questions { get; set; }

    }
}
