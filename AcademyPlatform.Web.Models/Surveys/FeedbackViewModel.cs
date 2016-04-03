namespace AcademyPlatform.Web.Models.Surveys
{
    using System.Collections.Generic;

    using AcademyPlatform.Web.Models.Assessments;

    public class FeedbackViewModel
    {
        public string Description { get; set; }

        public IEnumerable<QuestionViewModel> Questions { get; set; }

        public string AdditionalNotes { get; set; }
    }
}