namespace AcademyPlatform.Models.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class LectureVisit
    {
        //public int Id { get; set; }

        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Key, Column(Order = 1)]
        public int LectureId { get; set; }

        public DateTime LastVisitDate { get; set; }
    }
}
