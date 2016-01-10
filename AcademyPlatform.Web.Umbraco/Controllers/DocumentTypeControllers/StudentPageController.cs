namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Common;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class StudentPageController : RenderMvcController
    {
        private readonly ICoursesService _courses;
        private readonly ICategoryService _categories;
        private readonly IUmbracoMapper _mapper;

         public StudentPageController(ICoursesService courses, ICategoryService categories, IUmbracoMapper mapper)
        {
            _courses = courses;
            _categories = categories;
            _mapper = mapper;
        }
        
        [HttpGet]
        public override ActionResult Index(RenderModel model)
        {
            // TODO improve query
            var coursesContentCollection = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(Course));
            var coursesContentViewModels = new List<Course>();
            _mapper.AddCustomMapping(typeof(ImageViewModel).FullName, UmbracoMapperMappings.MapMediaFile)
                   .MapCollection(coursesContentCollection, coursesContentViewModels);

            var courses = _courses.GetActiveCourses();

            var coursesViewModels = courses.Join(
                coursesContentViewModels,
                course => course.Id,
                coursesContent => coursesContent.CourseId,
                (course, coursesContent) => new CoursesListViewModel
                {
                    Title = course.Title,
                    CourseUrl = coursesContent.Url,
                    ImageUrl = coursesContent.CoursePicture.Url,
                    ShortDescription = coursesContent.ShortDescription,
                    Category = course.Category
                }).ToList();

            //TODO Find out why(if) distinct works without implementing IEquitable<T>
            ViewBag.Categories = coursesViewModels.Select(x => x.Category).Distinct();
            return CurrentTemplate(coursesViewModels);
        }
    }
}