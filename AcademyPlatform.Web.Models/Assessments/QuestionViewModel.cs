namespace AcademyPlatform.Web.Models.Assessments
{
    using System.Collections.Generic;
    
    public class QuestionViewModel
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public IEnumerable<QuestionAnswer> Answers { get; set; }
        
    }
}
