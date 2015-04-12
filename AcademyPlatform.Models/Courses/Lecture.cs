using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcademyPlatform.Models.Courses
{
    public class Lecture
    {
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

        public virtual ICollection<LectureResource> Resources { get; set; }

        public Lecture()
        {
            this.Resources = new HashSet<LectureResource>();
        }
    }
}