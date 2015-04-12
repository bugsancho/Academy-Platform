namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class HomeworkSubmission
    {
        public int Id { get; set; }

        public DateTime SubmissionTime { get; set; }

        public string FilePath { get; set; }

        [Required]
        public virtual User User { get; set; }

        public string UserId { get; set; }

        public HomeworkAssignment Assignment { get; set; }

        public int AssignmentId { get; set; }
    }
}