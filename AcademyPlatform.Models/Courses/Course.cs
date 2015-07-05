namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course
    {
        public int Id { get; set; }

        public string PrettyUrl { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [DataType(DataType.MultilineText)]
        public string DetailedDescription { get; set; }

        public string ImageUrl { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public CourseDifficultyType Difficulty { get; set; }

        public virtual ICollection<Lecture> Lectures { get; set; }

        public virtual ICollection<User> Participants { get; set; }

        public Course()
        {
            this.Lectures = new HashSet<Lecture>();
            this.Participants = new HashSet<User>();
        }
    }
}