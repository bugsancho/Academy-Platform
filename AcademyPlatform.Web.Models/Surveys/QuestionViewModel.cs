namespace AcademyPlatform.Web.Models.Surveys
{
    using System.Collections.Generic;

    public class QuestionViewModel
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public IEnumerable<QuestionAnswer> Answers { get; set; }
        
    }
}
