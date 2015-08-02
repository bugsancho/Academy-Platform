namespace AcademyPlatform.Models.Assessments
{
    using System.Collections.Generic;

    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public QuestionType QuestionType { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual Assessment Assessment { get; set; }

        public int AssessmentId { get; set; }
    }
}
