namespace AcademyPlatform.Models.Assessments
{
    using System;

    using AcademyPlatform.Models.Base;
    using AcademyPlatform.Models.Courses;

    public class AssessmentSubmission :SoftDeletableEntity
    {
        public int Id { get; set; }

        public int AssessmentRequestId { get; set; }

        public virtual AssessmentRequest AssessmentRequest { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public DateTime SubmissionDate { get; set; }

        /// <summary>
        /// JSON representation of all the answers including the questions themseves.
        /// </summary>
        public string Submission { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
