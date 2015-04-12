namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HomeworkAssignment
    {
        [ForeignKey("Lecture")]
        public int Id { get; set; }

        public virtual Lecture Lecture { get; set; }

        public int LectureId { get; set; }

        public DateTime Deadline { get; set; }

        public HomeworkAssignmentType Type { get; set; }

        public virtual ICollection<HomeworkSubmission> Submissions { get; set; }

        public HomeworkAssignment()
        {
            this.Submissions = new HashSet<HomeworkSubmission>();
        }
    }
}