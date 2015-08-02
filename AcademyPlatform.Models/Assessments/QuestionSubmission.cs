namespace AcademyPlatform.Models.Assessments
{
    public class QuestionSubmission
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }



    }
}