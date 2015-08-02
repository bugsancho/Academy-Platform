namespace AcademyPlatform.Models.Assessments
{
    public class Answer
    {
        public int Id { get; set; }

        public virtual Question Question { get; set; }

        public int QuestionId { get; set; }

        public string Text { get; set; }

        public int? Points { get; set; }

        public bool IsCorrect { get; set; }
    }
}