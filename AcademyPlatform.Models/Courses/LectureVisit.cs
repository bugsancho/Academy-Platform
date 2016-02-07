namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AcademyPlatform.Models.Base;

    public class LectureVisit : SoftDeletableEntity
    {
        //public int Id { get; set; }

        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        public virtual User User { get; set; }
        
        public int ExternalLectureId { get; set; }

        [Key, Column(Order = 1)]
        public int LectureId { get; set; }

        public Lecture Lecture { get; set; }

        public DateTime LastVisitDate { get; set; }
    }
}
