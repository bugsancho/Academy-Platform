namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HomeworkAssignment
    {
        private ICollection<HomeworkSubmission> _submissions;

        [ForeignKey("Module")]
        public int Id { get; set; }

        public virtual Module Module { get; set; }

        public int LectureId { get; set; }

        public DateTime Deadline { get; set; }

        public HomeworkAssignmentType Type { get; set; }

        public virtual ICollection<HomeworkSubmission> Submissions
        {
            get { return _submissions; }
            set { _submissions = value; }
        }

        public HomeworkAssignment()
        {
            _submissions = new HashSet<HomeworkSubmission>();
        }
    }
}