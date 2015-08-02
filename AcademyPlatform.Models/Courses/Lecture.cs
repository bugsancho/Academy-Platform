namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Lecture
    {
        private ICollection<LectureResource> _resources;

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual Course Course { get; set; }

        public int CourseId { get; set; }

        [Required]
        public virtual User Lecturer { get; set; }

        public string LecturerId { get; set; }

        public virtual HomeworkAssignment Homework { get; set; }

        public int HomeworkId { get; set; }

        public virtual ICollection<LectureResource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        public Lecture()
        {
            _resources = new HashSet<LectureResource>();
        }
    }
}