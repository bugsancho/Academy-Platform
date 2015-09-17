namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Module
    {
        //private ICollection<LectureResource> _resources;

        public int Id { get; set; }

        public string Title { get; set; }

        public virtual Course Course { get; set; }

        public int CourseId { get; set; }

        //public virtual HomeworkAssignment Homework { get; set; }

        //public int HomeworkId { get; set; }

        //public virtual ICollection<LectureResource> Resources
        //{
        //    get { return _resources; }
        //    set { _resources = value; }
        //}

        //public Module()
        //{
        //    _resources = new HashSet<LectureResource>();
        //}
    }
}