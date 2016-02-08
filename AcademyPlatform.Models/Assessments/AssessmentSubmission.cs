namespace AcademyPlatform.Models.Assessments
{
    using System;
    using System.Collections.Generic;

    using AcademyPlatform.Models.Base;

    public class AssessmentSubmission :SoftDeletableEntity
    {
        public int Id { get; set; }

        public int AssessmentRequestId { get; set; }

        public AssessmentRequest AssessmentRequest { get; set; }

        public DateTime SubmissionDate { get; set; }

        public string Submission { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
