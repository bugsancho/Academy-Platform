namespace AcademyPlatform.Models.Courses
{
    using AcademyPlatform.Models.Base;

    public class Category : SoftDeletableLoggedEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

    }
}