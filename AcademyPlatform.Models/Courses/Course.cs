namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course
    {
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        //[StringLength(100, MinimumLength = 10)]
        public string Title { get; set; }

        //[StringLength(500, MinimumLength = 50)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
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