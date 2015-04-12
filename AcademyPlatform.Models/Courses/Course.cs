namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.Collections.Generic;

    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual ICollection<Lecture> Lectures { get; set; }

        public virtual ICollection<User> Participants { get; set; }

        public Course()
        {
            this.Lectures = new HashSet<Lecture>();
            this.Participants = new HashSet<User>();
        }
    }
}