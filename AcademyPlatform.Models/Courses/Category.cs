namespace AcademyPlatform.Models.Courses
{
    using AcademyPlatform.Models.Base;

    public class Category : SoftDeletableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

    }
}