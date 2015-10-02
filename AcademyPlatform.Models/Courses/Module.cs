namespace AcademyPlatform.Models.Courses
{
    public class Module
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual Course Course { get; set; }

        public int CourseId { get; set; }
    }
}