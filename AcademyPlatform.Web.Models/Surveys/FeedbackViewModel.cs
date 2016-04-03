namespace AcademyPlatform.Web.Models.Surveys
{
    using System.Collections.Generic;

    public class FeedbackViewModel
    {
        public string Description { get; set; }

        public IEnumerable<QuestionViewModel> Questions { get; set; }

        public string AdditionalNotes { get; set; }
    }
}