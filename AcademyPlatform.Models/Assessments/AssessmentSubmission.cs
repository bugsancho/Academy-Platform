namespace AcademyPlatform.Models.Assessments
{
    using System;
    using System.Collections.Generic;

    public class AssessmentSubmission
    {
        public int Id { get; set; }

        public int AssessmentId { get; set; }

        public virtual Assessment Assessment { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime SubmissionDate { get; set; }

        public ICollection<QuestionSubmission> QuestionSubmissions { get; set; }
    }
}
