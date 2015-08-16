namespace AcademyPlatform.Web.Models.Courses
{
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class CategoryEditViewModel : IMapFrom<Category>
    {
        public string Title { get; set; }

    }
}
