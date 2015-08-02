namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course
    {
        private ICollection<Lecture> _lectures;
        private ICollection<User> _participants;

        public int Id { get; set; }

        public string PrettyUrl { get; set; }

        public string Title { get; set; }

        public string TagLine { get; set; }

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

        public virtual ICollection<Lecture> Lectures
        {
            get { return _lectures; }
            set { _lectures = value; }
        }

        public virtual ICollection<User> Participants
        {
            get { return _participants; }
            set { _participants = value; }
        }

        public Course()
        {
            _lectures = new HashSet<Lecture>();
            _participants = new HashSet<User>();
        }
    }
}