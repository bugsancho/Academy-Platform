namespace AcademyPlatform.Models.Courses
{
    using AcademyPlatform.Models.Base;

    public class Lecture : SoftDeletableLoggedEntity
    {
        public int Id { get; set; }

        public int ExternalId { get; set; }

        public string Title { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public bool IsActive { get; set; }
    }
}
