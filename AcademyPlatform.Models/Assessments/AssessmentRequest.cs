namespace AcademyPlatform.Models.Assessments
{
    using AcademyPlatform.Models.Base;

    public class AssessmentRequest : SoftDeletableLoggedEntity
    {
        public int Id { get; set; }

        public int AssessmentExternalId { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
        
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Comma-delimited string of external question Ids selected for this exam
        /// </summary>
        public string QuestionIds { get; set; }
    }
}
