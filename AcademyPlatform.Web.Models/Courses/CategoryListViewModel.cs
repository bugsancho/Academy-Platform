namespace AcademyPlatform.Web.Models.Courses
{
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class CategoryListViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

    }
}
