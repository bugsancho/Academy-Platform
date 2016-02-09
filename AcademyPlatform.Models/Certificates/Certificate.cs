namespace AcademyPlatform.Models.Certificates
{
    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Base;
    using AcademyPlatform.Models.Courses;

    public class Certificate : SoftDeletableEntity
    {
        public int Id { get; set; }

        public string UniqueCode { get; set; }

        public string FilePath { get; set; }

        //public int UserId { get; set; }

        //public virtual User User { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public int AssesmentSubmissionId { get; set; }

        public AssessmentSubmission AssesmentSubmission { get; set; }
    }
}
