namespace AcademyPlatform.Web.Models.Assessments
{
    using System.Collections.Generic;

    using AcademyPlatform.Models.Assessments;

    public class QuestionViewModel
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public IEnumerable<QuestionAnswer> Answers { get; set; }

        public QuestionType QuestionType { get; set; }

        
    }
}
