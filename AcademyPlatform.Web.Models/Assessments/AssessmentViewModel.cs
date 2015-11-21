namespace AcademyPlatform.Web.Models.Assessments
{
    using System.Collections.Generic;

    public class AssessmentViewModel
    {
        public int Id { get; set; }

        public string IntroText { get; set; }

        public IEnumerable<QuestionViewModel> Questions { get; set; }

    }
}
